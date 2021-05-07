using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using accounting.Models;
using accounting.Repositories;
using accounting.Helpers;
using accounting.ViewModels;
using accounting.Infra;
using System.Text;

namespace accounting.Controllers
{
    [Authorize]
    [HandleError(View = "Error")]
    public class CompraController : Controller
    {
        #region Variables
        private AccountingEntities1 db = new AccountingEntities1();
        private IRepoCustom _repo = new RepoCustom();
        int _pageSize = 20;
        DateTime FechaEmisionD;
        DateTime FechaEmisionH;

        #endregion

        #region Listado
        // GET: Compra
        public ActionResult Index(int? proveedor, DateTime? FechaEmisionDesde, DateTime? FechaEmisionHasta, int page = 1)
        {
            //filtro por fecha y proveedor.
            GetComboProveedores();

            if (FechaEmisionDesde != null)
            {
                string fechaD = Convert.ToDateTime(FechaEmisionDesde).Year.ToString() + "/" + Convert.ToDateTime(FechaEmisionDesde).Month.ToString() + "/" + Convert.ToDateTime(FechaEmisionDesde).Day.ToString();
                FechaEmisionD = Convert.ToDateTime(fechaD);
            }

            if (FechaEmisionHasta != null)
            {
                string fechaH = Convert.ToDateTime(FechaEmisionHasta).Year.ToString() + "/" + Convert.ToDateTime(FechaEmisionHasta).Month.ToString() + "/" + Convert.ToDateTime(FechaEmisionHasta).Day.ToString();
                FechaEmisionH = Convert.ToDateTime(fechaH);
            }
            CompraVMIndex model = new CompraVMIndex { proveedor = proveedor, FechaEmisionDesde = FechaEmisionDesde == null ? FechaEmisionDesde : FechaEmisionD, FechaEmisionHasta = FechaEmisionHasta == null ? FechaEmisionHasta : FechaEmisionH, page = page };
            try
            {
                IEnumerable<ListCompra> list = _repo.CompraList(model.proveedor, model.FechaEmisionDesde, model.FechaEmisionHasta);

                model.list = list.OrderBy(o => o.FechaEmision).Skip((page - 1) * _pageSize).Take(_pageSize);
                model.pagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = _pageSize,
                    TotalItems = list.Count()
                };
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            return View(model);
        }
        #endregion

        #region Detalle
        // GET: Compra/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CompraVM co = new CompraVM();

            co = _repo.GetDetalleCompra(id);

            co.EstadoDesc = GetEstadoDesc(co.Estado);

            return View(co);
        }

       
        #endregion

        #region nuevo
        // GET: Compra/Create
        public ActionResult Create()
        {
            GetComboProveedores();
            GetComboTipoComprobante();
            return View();
        }

        // POST: Compra/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompraVM compraVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    compra co = new compra()
                    {
                        proveedor_id = compraVM.ProveedorId,
                        tipo_comprobante_id = compraVM.TipoComprobanteId,
                        nro_factura = compraVM.NroFactura,
                        fecha_emision = compraVM.FechaEmision,
                        fecha_contable = compraVM.FechaContable,
                        vencimiento_1 = compraVM.PrimerVencimiento,
                        vencimiento_2 = compraVM.SdoVencimiento,
                        importe = compraVM.Importe,
                        descuento_global = compraVM.DescuentoGlobal,
                        activo = 1,
                        update_date = DateTime.Now,
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                        estado = 0
                    };

                    db.compra.Add(co);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }

            GetComboProveedores();
            GetComboTipoComprobante();
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
            compra co = db.compra.Find(id);

            GetComboProveedores();
            GetComboTipoComprobante();

            CompraVM c = new CompraVM()
            {
                id = co.id,
                ProveedorId = co.proveedor_id,
                TipoComprobanteId = co.tipo_comprobante_id,
                NroFactura = co.nro_factura,
                FechaEmision = co.fecha_emision,
                FechaContable = co.fecha_contable,
                PrimerVencimiento = co.vencimiento_1,
                SdoVencimiento = co.vencimiento_2,
                Importe = co.importe,
                DescuentoGlobal = co.descuento_global,
                Estado = co.estado
            };
            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompraVM compraVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    compra co = new compra()
                    {
                        id = compraVM.id,
                        proveedor_id = compraVM.ProveedorId,
                        tipo_comprobante_id = compraVM.TipoComprobanteId,
                        nro_factura = compraVM.NroFactura,
                        fecha_emision = compraVM.FechaEmision,
                        fecha_contable = compraVM.FechaContable,
                        vencimiento_1 = compraVM.PrimerVencimiento,
                        vencimiento_2 = compraVM.SdoVencimiento,
                        importe = compraVM.Importe,
                        descuento_global = compraVM.DescuentoGlobal,
                        activo = 1,
                        update_date = DateTime.Now,
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                        estado = compraVM.Estado
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

            GetComboProveedores();
            GetComboTipoComprobante();
            return View();
        }
        #endregion

        #region Eliminar
        // GET: Compra/Delete/5
        public ActionResult Delete(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                CompraDeleteVM woVM = new CompraDeleteVM();
                woVM = _repo.GetDeleteCompra(id);

                return View(woVM);
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            return View();
        }

        // POST: Compra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            try
            {
                compra co = db.compra.Find(id);
                co.activo = 0;
                db.Entry(co).State = EntityState.Modified;
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

        #region metodos privados
        private void GetComboProveedores()
        {
            ViewBag.Provedores = db.proveedor.Where(x => x.activo == 1).OrderBy(x => x.razon_social);
        }
        private void GetComboTipoComprobante()
        {

            ViewBag.Tipo = db.tipo_comprobante.Where(x => x.activo == 1).OrderBy(x => x.descripcion);
        }
        private string GetEstadoDesc(int? estado)
        {
            string est= "";
            switch (estado)
            {
                case 0:
                    est = "Pendiente de Pago";
                    break;
                case 1:
                    est = "Pago incompleto";
                    break;
                case 2:
                    est = "Pagada";
                    break;
                default:
                    est = "Pendiente de Pago";
                    break;
            }
            return est;
        }
        public FileContentResult Export([Bind(Include = "proveedor, FechaD, FechaH")] int? proveedor, DateTime? FechaD, DateTime? FechaH)
        {
            StringBuilder csv = new StringBuilder();
            IEnumerable<ReportCompra> listado = _repo.CompraReport(proveedor, FechaD, FechaH);

            csv.AppendLine("Proveedor;Tipo de Comprobante;NroFactura;Fecha emision;Fecha Contable;Primer Vencimiento;Segundo Vencimiento;Importe;Descuento");
            foreach (var item in listado)
                csv.AppendLine(item.ToString());

            string archivo = "ReporteListadoCompras_" + DateTime.Now.ToString("yyyyMMdd");
            return File(Encoding.Default.GetBytes(csv.ToString()), "text/csv", archivo + ".csv");
        }
        #endregion
    }
}
