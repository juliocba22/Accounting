using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using accounting.Data;
using accounting.Helpers;
using accounting.Infra;
using accounting.Models;
using accounting.Repositories;
using accounting.ViewModels;

namespace accounting.Controllers
{
    [Authorize]
    [HandleError(View = "Error")]
    public class UsersController : Controller
    {
        private IRepoCustom _repo;
        int _pageSize = 20;

        #region --[CONSTRUCTOR]--

        public UsersController()
        {
            _repo = new RepoCustom();
        }

        public UsersController(IRepoCustom repoCustom)
        {
            _repo = repoCustom;
        }

        #endregion --[CONSTRUCTOR]--

        #region --[INDEX]--
        // GET: Users
        public ActionResult Index(string name, int page=1)
        {
            //return View(db.users.ToList());

            UsersIndexVM model = new UsersIndexVM { name = name, page= page };
            try
            {
                IEnumerable<ListUsers> list = _repo.UserList(model.name);

                model.list = list.OrderBy(o => o.name).Skip((page - 1) * _pageSize).Take(_pageSize);
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

           // ViewBagIndex(model.service_id);
            return View(model);
        }

        #endregion --[INDEX]--

        #region --[DETAILS]--

        // GET: Users/Details/5
        public ActionResult Details(long id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            users user = _repo.UserFind(id);

            UsersCreateVM model = new UsersCreateVM()
            {
                id = user.id,
                name = user.name,
                rol_id = user.rol_id,
                user_name = user.user_name,
                password = user.password,
                active = user.active,
            };


            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBagDetail(model.rol_id);

            return View(model);
        }

        #endregion --[DETAILS]--
    
        #region --[CREATE]--

        public ActionResult Create()
        {
            ViewBagCreate(0);
            return View(new UsersCreateVM { register_date = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsersCreateVM model)
        {
            try
            {         
                if (ModelState.IsValid)
                {
                    users user = new users()
                    {
                        name = model.name,
                        user_name = model.user_name,
                        rol_id = model.rol_id,
                        password = model.password,
                        register_date = DateTime.Now,
                        active = model.active,
                        create_user_id = int.Parse(Session["UserID"].ToString()),

                    };
                    
                    _repo.UserAdd(user);

                    //log.Info($"Usuario:{Session["UserID"]} - {User.Identity.Name} carga el archivo {model.file.FileName} - Grupo {model.group_name} con {cant} número de teléfonos.");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
                //log.Error($"Create - {ex.Message}", ex);
            }

            ViewBagCreate(model.rol_id);

            return View(model);
        }

        #endregion --[CREATE]--

        #region --[EDIT]--

        // GET: Users/Edit/5
        public ActionResult Edit(long id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            users user = _repo.UserGet(id);

            UsersCreateVM model = new UsersCreateVM()
            {
              id = user.id,
              name=user.name,
              rol_id=user.rol_id,
              user_name=user.user_name,
              password=user.password,
              active=user.active,
            };

            ViewBagCreate(model.rol_id);

            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBagCreate(model.rol_id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsersCreateVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    users user = new users()
                    {
                        id=model.id,
                        name = model.name,
                        user_name = model.user_name,
                        rol_id = model.rol_id,
                        password = model.password,
                        register_date = DateTime.Now,//no se deberia modificar
                        active = model.active,
                        update_date = DateTime.Now,
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                    };

                    _repo.UserUpdate(user);

                    //log.Info($"Usuario:{Session["UserID"]} - {User.Identity.Name} carga el archivo {model.file.FileName} - Grupo {model.group_name} con {cant} número de teléfonos.");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
                //log.Error($"Create - {ex.Message}", ex);
            }

            ViewBagCreate(model.rol_id);
            return View(model);
        }
        #endregion --[EDIT]--

        #region --[DELETE]--
        // GET: Users/Delete/5
        public ActionResult Delete(long id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            users user = _repo.UserFind(id);

            UsersCreateVM model = new UsersCreateVM()
            {
                id = user.id,
                name = user.name,
                rol_id = user.rol_id,
                user_name = user.user_name,
                password = user.password,
                active = user.active,
            };

            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBagDetail(model.rol_id);

            return View(model);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            users user = _repo.UserFind(id);

            _repo.UserDelete(user);

            return RedirectToAction("Index");
        }

        #endregion --[DELETE]--

        #region --[EXTRA]--
        void ViewBagCreate(int rol_id)
        {

            ViewBag.rol_id = new SelectList(_repo.RolAll().Select(x => new { id = x.id, description = x.description }).ToList().OrderBy(o => o.description), "id", "description", rol_id);
            
            //cuando se edita vendra el rol_id
            //if (rol_id==0)
            //{
            //    ViewBag.rol_id = new SelectList(_repo.RolAll().Select(x => new { id = x.id, description = x.description }).ToList().OrderBy(o => o.description), "id", "description");
            //}
            //else
            //{
            //    ViewBag.rol_id = new SelectList(_repo.RolAll().Select(x => new { id = x.id, description = x.description }).ToList().OrderBy(o => o.description), "id", "description",rol_id);
            //}
        }

        void ViewBagDetail(int rol_id)
        {
            IEnumerable<ListRol> list= _repo.RolGetById(rol_id);
            ViewBag.rol_name = list.Select(s => s.description).FirstOrDefault();
        }

        #endregion --[EXTRA]--
    }

}
