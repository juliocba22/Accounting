using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using accounting.Helpers;
using accounting.Infra;
using accounting.Models;
using accounting.Repositories;
using accounting.ViewModels;

namespace accounting.Controllers
{
    [Authorize]
    [CustomAuthorizeAttribute]
    [SessionExpireFilter]

    public class ProveedoresController : Controller
    {
        #region Variables
        private AccountingEntities1 db = new AccountingEntities1();
        private IRepoCustom _repo = new RepoCustom();
        int _pageSize = 20;
        #endregion

        #region listado
        // GET: Proveedores
        public ActionResult Index(string razonsocial, int page = 1)
        {
            ProveedorVMIndex model = new ProveedorVMIndex { razonSocial = razonsocial, page = page };
            try
            {
                IEnumerable<ListProveedor> list = _repo.ProveedorList(model.razonSocial);

                model.list = list.OrderBy(o => o.razonSocial).Skip((page - 1) * _pageSize).Take(_pageSize);
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
                //log.Error($"Index - {ex.Message}", ex);
            }

            return View(model);
        }
        #endregion

        #region Nuevo
        // GET: clients/Create
        public ActionResult Create()
        {
            ViewBagCreate(0);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProveedorVM prov)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    proveedor p = new proveedor()
                    {
                        dni = prov.dni,
                        cuit = prov.cuit,
                        codigo = int.Parse(prov.codigo),
                        razon_social = prov.razonSocial,
                        nombre_fantasia = prov.nombreFantasia,
                        localidad = prov.localidad,
                        provincia = prov.provincia,
                        personeria = prov.personeria,
                        telefono = prov.telefono,
                        mail = prov.mail,
                        mail_facturacion = prov.mailFacturacion,
                        categoria_impositiva_id = prov.categoria_impositiva_id,
                        direccion = prov.direccion,
                        piso_dpto = prov.pisoDpto,
                        codigo_postal = prov.codigoPostal,
                        register_date = DateTime.Now,
                        create_user_id = int.Parse(Session["UserID"].ToString()),
                        activo = 1
                    };

                    db.proveedor.Add(p);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }

            ViewBagCreate(prov.categoria_impositiva_id);
            return View();
        }
        #endregion


        #region Edicion
        // GET: clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proveedor prov = db.proveedor.Find(id);

            ProveedorVM p = new ProveedorVM()
            {
                id = prov.id,
                dni = prov.dni,
                cuit = prov.cuit,
                codigo = prov.codigo.ToString(),
                razonSocial = prov.razon_social,
                nombreFantasia = prov.nombre_fantasia,
                localidad = prov.localidad,
                provincia = prov.provincia,
                personeria = prov.personeria,
                telefono = prov.telefono,
                mail = prov.mail,
                mailFacturacion = prov.mail_facturacion,
                categoria_impositiva_id = prov.categoria_impositiva_id,
                direccion = prov.direccion,
                pisoDpto = prov.piso_dpto,
                codigoPostal = prov.codigo_postal,
                register_date = prov.register_date,
            };

            ViewBagCreate(p.categoria_impositiva_id);
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProveedorVM prov)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    proveedor p = new proveedor()
                    {
                        id = prov.id,
                        dni = prov.dni,
                        cuit = prov.cuit,
                        codigo = int.Parse(prov.codigo),
                        razon_social = prov.razonSocial,
                        nombre_fantasia = prov.nombreFantasia,
                        localidad = prov.localidad,
                        provincia = prov.provincia,
                        personeria = prov.personeria,
                        telefono = prov.telefono,
                        mail = prov.mail,
                        mail_facturacion = prov.mailFacturacion,
                        categoria_impositiva_id = prov.categoria_impositiva_id,
                        direccion = prov.direccion,
                        piso_dpto = prov.pisoDpto,
                        codigo_postal = prov.codigoPostal,
                        update_date = DateTime.Now,
                        register_date = prov.register_date,
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                        activo = 1
                    };

                    db.Entry(p).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBagCreate(prov.categoria_impositiva_id);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
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

            proveedor prov = db.proveedor.Find(id);

            ProveedorVM c = new ProveedorVM()
            {
                id = prov.id,
                dni = prov.dni,
                cuit = prov.cuit,
                codigo = prov.codigo.ToString(),
                razonSocial = prov.razon_social,
                nombreFantasia = prov.nombre_fantasia,
                localidad = prov.localidad,
                provincia = prov.provincia,
                personeria = prov.personeria,
                telefono = prov.telefono,
                mail = prov.mail,
                mailFacturacion = prov.mail_facturacion,
                categoria_impositiva_id = prov.categoria_impositiva_id,
                direccion = prov.direccion,
                pisoDpto = prov.piso_dpto,
                codigoPostal = prov.codigo_postal,
            };

            ViewBagDetail(prov.categoria_impositiva_id);

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
            proveedor prov = db.proveedor.Find(id);

            ProveedorVM c = new ProveedorVM()
            {
                id = prov.id,
                codigo = prov.codigo.ToString(),
                razonSocial = prov.razon_social
            };

            return View(c);
        }

        // POST: clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            proveedor prov = db.proveedor.Find(id);
            prov.activo = 0;
            db.Entry(prov).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion


        #region --[Extra]--
        void ViewBagCreate(int categoria_impositiva_id)
        {
            ViewBag.categoria_impositiva_id = new SelectList(db.categoria_impositiva, "id", "descripcion", categoria_impositiva_id);
        }

        void ViewBagDetail(int categoria_impositiva_id)
        {
            IEnumerable<ListCategoriaImpositiva> list = _repo.CategoriaImpositivaGetById(categoria_impositiva_id);
            ViewBag.categoria_impositiva_name = list.Select(s => s.descripcion).FirstOrDefault();
        }

        public FileContentResult Export([Bind(Include = "razonSocial")] string razonSocial)
        {
            StringBuilder csv = new StringBuilder();
            IEnumerable<ReportProveedor> listado = _repo.ProveedorReport(razonSocial);

            csv.AppendLine("codigo;dni;cuil;razonSocial;localidad;provincia;telefono;email;emailFacturacion");
            foreach (var item in listado)
                csv.AppendLine(item.ToString());

            string archivo = "ReporteListadoProveedores_" + DateTime.Now.ToString("yyyyMMdd");
            return File(Encoding.Default.GetBytes(csv.ToString()), "text/csv", archivo + ".csv");
        }

        #endregion --[EXTRA]--
    }
}
