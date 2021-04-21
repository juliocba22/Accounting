using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using accounting.Models;

namespace accounting.Controllers
{
    public class WorkOrderController : Controller
    {
        #region Variables
        private AccountingEntities db = new AccountingEntities();
        private IRepoCustom _repo = new RepoCustom();
        int _pageSize = 20;

        #endregion

        #region Listado
        // GET: WorkOrder
        public ActionResult Index()
        {
            //Filtros por fecha y estado.

            return View(db.work_order.ToList());
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
            work_order work_order = db.work_order.Find(id);
            if (work_order == null)
            {
                return HttpNotFound();
            }
            return View(work_order);
        }
        #endregion

        #region Nuevo
        // GET: WorkOrder/Create
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nro_orden,fecha,descripcion,product_service_id,cantidad,nombre_paciente,social_work_id,profesional_id,status,motivo_eliminacion")] work_order work_order)
        {
            if (ModelState.IsValid)
            {
                db.work_order.Add(work_order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(work_order);
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
            work_order work_order = db.work_order.Find(id);
            if (work_order == null)
            {
                return HttpNotFound();
            }
            return View(work_order);
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nro_orden,fecha,descripcion,product_service_id,cantidad,nombre_paciente,social_work_id,profesional_id,status,motivo_eliminacion")] work_order work_order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(work_order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(work_order);
        }
        #endregion

        #region Eliminacion
        // GET: WorkOrder/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            work_order work_order = db.work_order.Find(id);
            if (work_order == null)
            {
                return HttpNotFound();
            }
            return View(work_order);
        }

        // POST: WorkOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            work_order work_order = db.work_order.Find(id);
            db.work_order.Remove(work_order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
