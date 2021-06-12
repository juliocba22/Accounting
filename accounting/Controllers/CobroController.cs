using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using accounting.Models;
using accounting.Helpers;
using accounting.Repositories;
using accounting.ViewModels;
using accounting.Infra;
using static accounting.Helpers.Enumerables;

namespace accounting.Controllers
{
    [Authorize]
    [HandleError(View = "Error")]
    public class CobroController : Controller
    {
       
        #region Variables
        private AccountingEntities1 db = new AccountingEntities1();
        private IRepoCustom _repo = new RepoCustom();
        int _pageSize = 20;

        #endregion

        #region Listado
        // GET: clients
        public ActionResult Index(string nroFactura, int page = 1)
        {
            CobroVMIndex model = new CobroVMIndex { nroFactura = nroFactura, page = page };
            try
            {
                IEnumerable<ListCobros> list = _repo.CobrosList(model.nroFactura);

                model.list = list.OrderByDescending(o => o.id).Skip((page - 1) * _pageSize).Take(_pageSize);
                model.pagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = _pageSize,
                    TotalItems = list.Count()
                };
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }

            return View(model);
        }
        #endregion


        #region Nuevo
        // GET: clients/Create
        public ActionResult Create()
        {
            CobroVM model = new CobroVM{ nroRecibo = GetNroRecibo(), fechaFactura = DateTime.Now }; 
            GetComboYN();
            GetComboClientes();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CobroVM cobro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    long NroRecibo = cobro.nroRecibo;
                    cobros c = new cobros()
                    {
                        nro_factura = cobro.nroFactura,
                        fecha_factura = cobro.fechaFactura,
                        monto = cobro.monto,
                        cobro_parcial = cobro.cobroParcial,
                        observaciones = cobro.Observaciones,
                        subtotal_recibo = cobro.subtotalRecibo,
                        total = cobro.total,
                        update_date = DateTime.Now,
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                        activo = 1,
                        cliente_id = cobro.clienteId,
                        nro_recibo = NroRecibo,// cobro.nroRecibo,
                    };

                    db.cobros.Add(c);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            GetComboYN();
            GetComboClientes();
            return View();
        }
        #endregion

        #region editar
        // GET: Compra/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cobros co = db.cobros.Find(id);

          
            CobroVM c = new CobroVM()
            {
                id = co.id,
                nroFactura = co.nro_factura,
                fechaFactura = co.fecha_factura,
                monto = co.monto,
                cobroParcial = (double)co.cobro_parcial,
                Observaciones = co.observaciones,
                subtotalRecibo = (double)co.subtotal_recibo,
                total = co.total,
                clienteId = co.cliente_id,
                nroRecibo = co.nro_recibo,
            };
            GetComboYN();
            GetComboClientes();

            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CobroVM cobroVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    cobros co = new cobros()
                    {
                        id = cobroVM.id,
                        nro_factura = cobroVM.nroFactura,
                        fecha_factura = cobroVM.fechaFactura,
                        monto = cobroVM.monto,
                        cobro_parcial = cobroVM.cobroParcial,
                        observaciones = cobroVM.Observaciones,
                        subtotal_recibo = cobroVM.subtotalRecibo,
                        total = cobroVM.total,
                        cliente_id = cobroVM.clienteId,
                        nro_recibo = cobroVM.nroRecibo,
                        activo = 1,
                        update_date = DateTime.Now,
                        update_user_id = int.Parse(Session["UserID"].ToString())
                    };

                    db.Entry(co).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }

            GetComboYN();
            GetComboClientes();
            return View();
        }
        #endregion

        #region Detalle
        // GET: clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cobros cobro = db.cobros.Find(id);

            CobroVM c = new CobroVM()
            {
                id = cobro.id,
                nroFactura = cobro.nro_factura,
                fechaFactura = cobro.fecha_factura,
                monto = cobro.monto,
                cobroParcial = (double)cobro.cobro_parcial,
                Observaciones = cobro.observaciones,
                subtotalRecibo = (double)cobro.subtotal_recibo,
                total = cobro.total,
                clienteId = cobro.cliente_id,
                nroRecibo = cobro.nro_recibo,
            };

            c.clienteDescripcion = GetClienteDesc(c.clienteId);

            return View(c);
        }
        #endregion

        #region Eliminacion
        // GET: clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cobros cobro = db.cobros.Find(id);

            CobroVM c = new CobroVM()
            {
                id = cobro.id,
                nroRecibo = cobro.nro_recibo,
                nroFactura = cobro.nro_factura,
                fechaFactura = cobro.fecha_factura,
                monto = cobro.monto
            };

            return View(c);
        }

        // POST: clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                cobros cobro = db.cobros.Find(id);
                cobro.activo = 0;
                db.Entry(cobro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            return View();
        }

        #endregion

        #region Metodos Privados

        string GetClienteDesc(int clienteId)
        {
            var query = db.client.Where(c => c.activo == 1 && c.id == clienteId ).FirstOrDefault();
            if (query != null)
                return query.razonSocial;
            return "";
        }

        void GetComboClientes()
        {
            ViewBag.Clientes = db.client.Where(x => x.activo == 1).OrderBy(x => x.razonSocial);
        }
        private void GetComboYN()
        {
            enumYN e = new enumYN();
            ViewBag.YN = e.GetYN();
        }

        public long GetNroRecibo()
        {
            return db.cobros.DefaultIfEmpty().Max(c => c == null ? 0 : c.id) + 1;
        }
        #endregion

    }
}
