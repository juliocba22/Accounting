using accounting.Helpers;
using accounting.Infra;
using accounting.Models;
using accounting.Repositories;
using accounting.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace accounting.Controllers
{
    [Authorize]
    [HandleError(View = "Error")]
    public class RolController : Controller
    {
        #region Variables
        private AccountingEntities1 db = new AccountingEntities1();
        private IRepoCustom _repo = new RepoCustom();
        int _pageSize = 20;

        #endregion

        #region listado
        // GET: Rol
        public ActionResult Index(string descripcion, int page = 1)
        {
            RolVMIndex model = new RolVMIndex { descripcion = descripcion, page = page };
            try
            {
                IEnumerable<ListRol> list = _repo.RolList(model.descripcion);

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

        #region detalle
        // GET: Rol/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rol r = db.rol.Find(id);

            RolVM c = new RolVM()
            {
                id = r.id,
                descripcion = r.description
            };

            return View(c);
        }
        #endregion

        #region nuevo
        // GET: Rol/Create
        public ActionResult Create()
        {
            RolVM model = new RolVM { };
            return View(model);
        }

        // POST: Rol/Create
        [HttpPost]
        public ActionResult Create(RolVM rol)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    rol r = new rol()
                    {
                        description = rol.descripcion,
                    };

                    db.rol.Add(r);
                    db.SaveChanges();
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

        #region editar
        // GET: Rol/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rol r = db.rol.Find(id);


            RolVM c = new RolVM()
            {
                id = r.id,
                descripcion = r.description
            };

            return View(c);
        }

        // POST: Rol/Edit/5
        [HttpPost]
        public ActionResult Edit(RolVM rol)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    rol r = new rol()
                    {
                        id = rol.id,
                        description = rol.descripcion
                    };

                    db.Entry(r).State = EntityState.Modified;
                    db.SaveChanges();
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

        #region eliminacion
        // GET: Rol/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rol r = db.rol.Find(id);

            RolVM rol= new RolVM()
            {
                id = r.id,
                descripcion = r.description
            };

            return View(rol);
        }

        // POST: Rol/Delete/5
        [HttpPost]
        public ActionResult Delete(byte id)
        {
            try
            {
                rol r = db.rol.Find(id);
                r.activo = 0;
                db.Entry(r).State = EntityState.Modified;
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
    }
}
