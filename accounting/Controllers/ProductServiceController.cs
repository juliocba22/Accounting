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
//using ceTe.DynamicPDF;
//using ceTe.DynamicPDF.PageElements;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace accounting.Controllers
{
    [Authorize]
    [HandleError(View = "Error")]
    public class ProductServiceController : Controller
    {
        #region Variables
        private AccountingEntities1 db = new AccountingEntities1();
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

        #region Detalle
        // GET: ProductService/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductServiceVM product_service = _repo.GetProductServiceDetails(id);

            ViewBag.TipoDetalle = product_service.tipo == 0 ? "Producto" : "Servicio";
            ViewBag.UMDetalle = product_service.unidadMedida == 0 ? "Valor unitario por sesión" : product_service.unidadMedida == 1 ? "Valor unitario por hora" : product_service.unidadMedida == 2 ? "Valor unitario por visita" : "Valor unitario por Km";

            return View(product_service);
        }
        #endregion

        #region Nuevo
        // GET: ProductService/Create
        public ActionResult Create()
        {
            GetComboTipo();
            GetComboUnidadMedida();
            GetComboClientes();
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
                        activo = 1,
                        unidad_medida = psVM.unidadMedida,
                        costo_profesional = psVM.costoProfesional,
                        client_id = psVM.ClienteId
                    };

                    db.product_service.Add(ps);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                GetComboTipo();
                GetComboUnidadMedida();
                GetComboClientes();
            }
            catch
            {
                GetComboTipo();
                GetComboUnidadMedida();
                GetComboClientes();
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
            product_service product_service = db.product_service.Find(id);
            GetComboTipo();
            GetComboUnidadMedida();
            GetComboClientes();

            ProductServiceVM psVm = new ProductServiceVM()
            {
                id = product_service.id,
                nombre = product_service.nombre,
                tipo = product_service.tipo,
                valorUnitario = product_service.valorUnitario,
                unidadMedida = product_service.unidad_medida,
                costoProfesional = product_service.costo_profesional,
                ClienteId = product_service.client_id
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
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                        unidad_medida = psVm.unidadMedida,
                        costo_profesional = psVm.costoProfesional,
                        client_id = psVm.ClienteId
                    };
                    db.Entry(ps).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }

            GetComboTipo();
            GetComboUnidadMedida();
            GetComboClientes();
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

            ViewBag.Type = e.GetComboTipo();
        }
        private void GetComboUnidadMedida()
        {
            Enumerables e = new Enumerables();

            ViewBag.UM = e.GetUM();
        }
        private void GetComboClientes()
        {
            ViewBag.Client = db.client.Where(x => x.activo == 1).OrderBy(x => x.razonSocial);
        }
        public FileContentResult Export([Bind(Include = "nombre")] string nombre)
        {
            StringBuilder csv = new StringBuilder();
            IEnumerable<ReportProductService> listado = _repo.ProductServiceReport(nombre);

            csv.AppendLine("Código;Nombre;Tipo;Unidad Medida;Valor Unitario; Costo Porfesional;Cliente");
            foreach (var item in listado)
                csv.AppendLine(item.ToString());

            string archivo = "ReporteListadoProductosyServicios_" + DateTime.Now.ToString("yyyyMMdd");
            return File(Encoding.Default.GetBytes(csv.ToString()), "text/csv", archivo + ".csv");
        }
        #endregion

    }
}
