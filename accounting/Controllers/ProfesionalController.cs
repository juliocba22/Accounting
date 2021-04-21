using System;
using System.Collections.Generic;
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
    public class ProfesionalController : Controller
    {
        #region Variables
        private AccountingEntities db = new AccountingEntities();
        private IRepoCustom _repo = new RepoCustom();
        int _pageSize = 20;

        #endregion

        #region Listado
        // GET: profesionals
        public ActionResult Index(string nombre, int page=1)
        { 
            ProfesionalVMIndex model = new ProfesionalVMIndex { nombre = nombre, page = page };
            try
            {
                IEnumerable<ListProfesional> list = _repo.ProfesionalList(model.nombre);

                model.list = list.OrderBy(o => o.nombre).Skip((page - 1) * _pageSize).Take(_pageSize);
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
        // GET: profesionals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProfesionalVM prof = new ProfesionalVM();

            prof = _repo.GetDetalleProfesional(id);

            return View(prof);
        }
        #endregion

        #region Nuevo
        // GET: profesionals/Create
        public ActionResult Create()
        {
            GetComboServicios();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProfesionalVM prof)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    profesional p = new profesional()
                    {
                        product_service_id = prof.product_service_id,
                        nombre = prof.nombre,
                        domicilio = prof.domicilio,
                        cuit = prof.cuit,
                        matricula = prof.matricula,
                        localidad = prof.localidad,
                        provincia = prof.provincia,
                        telefono = prof.telefono,
                        email = prof.email,
                        update_date = DateTime.Now,
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                        activo = 1
                    };

                    db.profesional.Add(p);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            GetComboServicios();
            return View();
        }
        #endregion

        #region Edicion
        // GET: profesionals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            profesional prof = db.profesional.Find(id);
            GetComboServicios();

            ProfesionalVM p = new ProfesionalVM()
            {
                id = prof.id,
                product_service_id = prof.product_service_id,
                nombre = prof.nombre,
                domicilio = prof.domicilio,
                cuit = prof.cuit,
                matricula = prof.matricula,
                localidad = prof.localidad,
                provincia = prof.provincia,
                telefono = prof.telefono,
                email = prof.email
            };
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProfesionalVM prof)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    profesional p = new profesional()
                    {
                        id = prof.id,
                        product_service_id = prof.product_service_id,
                        nombre = prof.nombre,
                        domicilio = prof.domicilio,
                        cuit = prof.cuit,
                        matricula = prof.matricula,
                        localidad = prof.localidad,
                        provincia = prof.provincia,
                        telefono = prof.telefono,
                        email = prof.email,
                        update_date = DateTime.Now,
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                        activo = 1
                    };

                    db.Entry(p).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            GetComboServicios();
            return View();
        }
        #endregion

        #region Eliminacion
        // GET: profesionals/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ProfesionalVM prof = new ProfesionalVM();

                prof = _repo.GetDetalleProfesional(id);


                return View(prof);
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            return View();
        }

        // POST: profesionals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                profesional prof = db.profesional.Find(id);
                prof.activo = 0;
                db.Entry(prof).State = EntityState.Modified;
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
        private void GetComboServicios()
        {
            ViewBag.Servicio = db.product_service.Where(x=> x.tipo == 1).OrderBy(x => x.nombre); //solo Servicio.
        }
        public FileContentResult Export([Bind(Include = "nombre")] string nombre)
        {
            StringBuilder csv = new StringBuilder();
            IEnumerable<ReportProfesional> listado = _repo.ProfesionalReport(nombre);

            csv.AppendLine("Nombre y Apellido;Servicio;Matrícula;CUIT;Domicilio;Localidad;Provincia;Teléfono;Email");
            foreach (var item in listado)
                csv.AppendLine(item.ToString());

            string archivo = "ReporteListadoProfesionales_" + DateTime.Now.ToString("yyyyMMdd");
            return File(Encoding.Default.GetBytes(csv.ToString()), "text/csv", archivo + ".csv");
        }

        #endregion
    }
}
