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
    //[Authorize]
    [CustomAuthorizeAttribute]
    [SessionExpireFilter]
    public class UsersController : Controller
    {
        //private accountingContext db = new accountingContext();

        //public string draw = ""; //Te sirve para que datatable se redibuje
        //public string start = ""; // Pagian a comenzar
        //public string length = "";
        //public string sortColumn = "";
        //public string sortColumnDir = "";
        //public string searchValue = "";

        //public int pageSize, skip, recordsTotal;


        private IRepoCustom _repo;
        int _pageSize = 5;

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

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}


        //implementacion tabla dinamica jquery

        //    [HttpPost]
        //    public ActionResult Json()
        //    {
        //        try
        //        {
        //            //logistica Datatable
        //            var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //            var start = Request.Form.GetValues("start").FirstOrDefault();
        //            var length = Request.Form.GetValues("length").FirstOrDefault();
        //            var sortColumn = 1;//Request.Form.GetValues("columns["+Request.Form.GetValues("order[0].[column]").FirstOrDefault() + "][apellido]").FirstOrDefault();
        //            var sortColumnDir = 1;// Request.Form.GetValues("order[0][dir]").FirstOrDefault();
        //            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

        //            pageSize = length != null ? Convert.ToInt32(length) : 0;
        //            skip = length != null ? Convert.ToInt32(start) : 0;
        //            recordsTotal = 0;

        //            IEnumerable<ListUsers> list = new List<ListUsers>();

        //            list = _repo.UsersList(searchValue, recordsTotal, skip, pageSize);

        //            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = list });
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json();
        //        }
        //    }

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
