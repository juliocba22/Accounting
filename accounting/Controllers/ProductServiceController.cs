using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using accounting.Helpers;
using accounting.Infra;
using accounting.Models;
using accounting.Repositories;
using accounting.ViewModels;

namespace accounting.Controllers
{
    public class ProductServiceController : Controller
    {
        #region Variables
        private AccountingEntities db = new AccountingEntities();
        private IRepoCustom _repo = new RepoCustom();
        int _pageSize = 20;
        #endregion

        #region Listado
        // GET: ProductService
        public ActionResult Index(string nombre, int page=1)
        {
            ProductServiceVMIndex model = new ProductServiceVMIndex { nombre = nombre, page = page };
            try
            {
                IEnumerable<ListProductService> list = _repo.ProductServiceList(model.nombre);

                model.list = list.OrderBy(o => o.id).Skip((page - 1) * _pageSize).Take(_pageSize);
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

        #region Detalle
        // GET: ProductService/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product_service product_service = db.product_service.Find(id);

            ProductServiceVM psVm = new ProductServiceVM()
            {
                id = product_service.id,
                nombre = product_service.nombre,
                tipo = product_service.tipo,
                valorUnitario = product_service.valorUnitario
            };

            ViewBag.TipoDetalle = product_service.tipo == 0 ? "Producto" : "Servicio";


            return View(psVm);
        }
        #endregion

        #region Nuevo
        // GET: ProductService/Create
        public ActionResult Create()
        {
            GetComboTipo();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductServiceVM psVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    product_service ps = new product_service()
                    {
                        tipo = psVM.tipo,
                        nombre = psVM.nombre,
                        valorUnitario = psVM.valorUnitario,
                        update_date = DateTime.Now,
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                        activo = 1
                    };

                    db.product_service.Add(ps);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                GetComboTipo();
            }
            catch
            {
                GetComboTipo();
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }

            return View();
        }
        #endregion

        #region Editar
        // GET: ProductService/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GetComboTipo();
            product_service product_service = db.product_service.Find(id);
           
            ProductServiceVM psVm = new ProductServiceVM()
            {
                id = product_service.id,
                nombre = product_service.nombre,
                tipo = product_service.tipo,
                valorUnitario = product_service.valorUnitario
            };

            return View(psVm);
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductServiceVM psVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    product_service ps = new product_service()
                    {
                        id = psVm.id,
                        tipo = psVm.tipo,
                        nombre = psVm.nombre,
                        valorUnitario = psVm.valorUnitario,
                        activo = 1,
                        update_date = DateTime.Now,
                        update_user_id = int.Parse(Session["UserID"].ToString())
                    };
                    db.Entry(ps).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                GetComboTipo();
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            return View();
        }
        #endregion

        #region Eliminacion
        // GET: ProductService/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product_service product_service = db.product_service.Find(id);

            ProductServiceVM psVm = new ProductServiceVM()
            {
                id = product_service.id,
                nombre = product_service.nombre,
                tipo = product_service.tipo,
                valorUnitario = product_service.valorUnitario
            };

            ViewBag.TipoDetalle = product_service.tipo == 0 ? "Producto" : "Servicio";
            return View(psVm);
        }

        // POST: ProductService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                product_service product_service = db.product_service.Find(id);
                product_service.activo = 0;
                db.Entry(product_service).State = EntityState.Modified;
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

        #region Metodos privados
        private void GetComboTipo()
        {
            Enumerables e = new Enumerables();

            ViewBag.Tipo = e.GetComboTipo;
        }
        public FileContentResult Export([Bind(Include = "nombre")] string nombre)
        {
            StringBuilder csv = new StringBuilder();
            IEnumerable<ReportProductService> listado = _repo.ProductServiceReport(nombre);

            csv.AppendLine("Código;Nombre;Tipo;Valor Unitario");
            foreach (var item in listado)
                csv.AppendLine(item.ToString());

            string archivo = "ReporteListadoProductosyServicios_" + DateTime.Now.ToString("yyyyMMdd");
            return File(Encoding.Default.GetBytes(csv.ToString()), "text/csv", archivo + ".csv");
        }
        #endregion

    }
}
