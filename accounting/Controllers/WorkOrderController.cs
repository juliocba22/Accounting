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
using Newtonsoft.Json;

namespace accounting.Controllers
{
    [Authorize]
    [CustomAuthorizeAttribute]
    [SessionExpireFilter]
    public class WorkOrderController : Controller
    {
        #region Variables
        private AccountingEntities1 db = new AccountingEntities1();
        private IRepoCustom _repo = new RepoCustom();
        int _pageSize = 20;

        #endregion

        #region Listado
        // GET: WorkOrder
        public ActionResult Index(int? status, int page = 0)
        {
            //Filtros por fecha y estado.
            GetComboStatusAll();

            WorkOrderVMIndex model = new WorkOrderVMIndex { status = status, page = page };
            try
            {
                IEnumerable<ListWorkOrder> list = _repo.WorkOrderList(model.status);

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
        // GET: WorkOrder/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WorkOrderVM wo = new WorkOrderVM();

            wo = _repo.GetDetalleWorkOrder(id);

            return View(wo);
        }
        #endregion

        #region Nuevo
        // GET: WorkOrder/Create
        public ActionResult Create()
        {
            GetComboServicios();
            GetComboProfesional();
            GetComboStatus();
            return View();
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkOrderVM woVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    work_order wo = new work_order()
                    {
                       fecha = woVM.Fecha,
                       descripcion = woVM.Descripcion,
                       product_service_id = woVM.ProductServiceId,
                       cantidad = woVM.Cantidad,
                       nombre_paciente = woVM.Paciente,
                       social_work_id = woVM.SocialWorkId,
                       profesional_id = woVM.ProfesionalId,
                       status_id = woVM.StatusId,
                       update_date = DateTime.Now,
                       update_user_id = int.Parse(Session["UserID"].ToString()),
                       importe = woVM.Importe
                    };

                    db.work_order.Add(wo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            GetComboServicios();
            GetComboProfesional();
            GetComboStatus();
            return View();
        }
        #endregion

        #region Edicion
        // GET: WorkOrder/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            work_order wo = db.work_order.Find(id);

            GetComboServicios();
            GetComboProfesional();
            GetComboStatus();

            WorkOrderVM p = new WorkOrderVM()
            {
                id = wo.id,
                Fecha = wo.fecha,
                Descripcion = wo.descripcion,
                ProductServiceId = wo.product_service_id,
                Cantidad = wo.cantidad,
                Paciente = wo.nombre_paciente,
                SocialWorkId = wo.social_work_id,
                ProfesionalId = wo.profesional_id,
                StatusId = wo.status_id,
                Importe = wo.importe
            };
            return View(p);
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkOrderVM woVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    work_order wo = new work_order()
                    {
                        id = woVM.id,
                        fecha = woVM.Fecha,
                        descripcion = woVM.Descripcion,
                        product_service_id = woVM.ProductServiceId,
                        cantidad = woVM.Cantidad,
                        nombre_paciente = woVM.Paciente,
                        social_work_id = woVM.SocialWorkId,
                        profesional_id = woVM.ProfesionalId,
                        status_id = woVM.StatusId,
                        update_date = DateTime.Now,
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                        importe = woVM.Importe
                    };

                    db.Entry(wo).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            GetComboServicios();
            GetComboProfesional();
            GetComboStatus();
            return View();
        }
        #endregion

        #region Eliminacion
        // GET: WorkOrder/Delete/5
        public ActionResult Delete(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                WorkOrderDeleteVM woVM = new WorkOrderDeleteVM();
                woVM = _repo.GetDelete(id);

                return View(woVM);
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            return View();
        }

        // POST: WorkOrder/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(WorkOrderDeleteVM woVM)
        {
            try
            {
                WorkOrderVM woBD = _repo.GetDeleteWorkOrder(woVM.id);

                if (ModelState.IsValid)
                {
                    work_order wo = new work_order()
                    {
                       id = woVM.id,
                       fecha = woBD.Fecha,
                       product_service_id = woBD.ProductServiceId,
                       descripcion =  woBD.Descripcion,
                       cantidad = woBD.Cantidad,
                       nombre_paciente = woBD.Paciente,
                       profesional_id = woBD.ProfesionalId,
                       social_work_id = woBD.SocialWorkId,
                       status_id = 4,
                       motivo_eliminacion = woVM.MotivoEliminacion,
                       update_date = DateTime.Now,
                       update_user_id = int.Parse(Session["UserID"].ToString())
                    };

                    db.Entry(wo).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            GetComboServicios();
            GetComboStatus();
            return View();
        }
        #endregion

        #region metodos privados
        [ActionName("CamposGet")]
        public string CamposGet(int? id)
        {
            IEnumerable <ComboProductService> _combo = GetComboTipoById(id);
            var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
            string json = JsonConvert.SerializeObject(_combo, Formatting.Indented, serializerSettings);

            return json;
        }

        private IEnumerable<ComboProductService> GetComboTipoById(int? id)
        {
            dynamic tipo;
           
            tipo = (from ps in db.product_service
                            where ps.id == id
                            select new ComboProductService
                            {
                                id = ps.tipo,
                                nombre = ps.nombre,
                                valUnitario = ps.valorUnitario
                            }).ToList();

            return tipo;
        }

        private void GetComboServicios()
        {
            ViewBag.Servicio = db.product_service.Where(x => x.activo == 1).OrderBy(x => x.nombre);
        }
        private void GetComboProfesional()
        {
            ViewBag.Profesional = db.profesional.Where(x => x.activo == 1).OrderBy(x => x.nombre); 
        }
        private void GetComboStatus()
        {
            ViewBag.Status = db.work_order_status.Where(x=> x.id != 4).OrderBy(x => x.id); 
        }
        private void GetComboStatusAll()
        {
            ViewBag.Status = db.work_order_status.OrderBy(x => x.id);
        }

        public FileContentResult Export([Bind(Include = "status")] int? status)
        {
            StringBuilder csv = new StringBuilder();
            IEnumerable<ReportWorkOrder> listado = _repo.WorkOrderReport(status);

            csv.AppendLine("Nro de Orden;Fecha;Descripción;Producto/Servicio;Cantidad;Paciente;Profesional;Importe;Estado;Motivo Eliminacion");
            foreach (var item in listado)
                csv.AppendLine(item.ToString());

            string archivo = "ReporteListadoOrdenesTrabajo_" + DateTime.Now.ToString("yyyyMMdd");
            return File(Encoding.Default.GetBytes(csv.ToString()), "text/csv", archivo + ".csv");
        }

        #endregion

    }
}
