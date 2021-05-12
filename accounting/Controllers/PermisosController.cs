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
    [CustomAuthorizeAttribute]
    [SessionExpireFilter]

    public class PermisosController : Controller
    {
        private AccountingEntities1 db = new AccountingEntities1();

        private IRepoCustom _repo;
        int _pageSize = 30;
        string path_file;

        #region --[CONSTRUCTOR]--

        public PermisosController()
        {
            _repo = new RepoCustom();
        }

        public PermisosController(IRepoCustom repoCustom)
        {
            _repo = repoCustom;
        }

        #endregion --[CONSTRUCTOR]--

        #region index

        // GET: Permisos
        public ActionResult Index(byte rol_id=0, int page = 1)
        {
            PermisoIndexVM model = new PermisoIndexVM {rol_id = rol_id, page = page };
            try
            {
                if (model.rol_id != 0)
                {
                    IEnumerable<ListPermisos> list = _repo.PermisosList(model.rol_id);

                    model.list = list.OrderBy(o => o.FechaAsignacion).Skip((page - 1) * _pageSize).Take(_pageSize);
                    model.pagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = _pageSize,
                        TotalItems = list.Count()
                    };
                }
                else
                {
                    model.list = Enumerable.Empty<ListPermisos>();
                }        
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }

            ViewBagCreate(rol_id);
            return View(model);
        }

        #endregion index

        #region extra
        void ViewBagCreate(int rol_id)
        {
            ViewBag.rol_id = new SelectList(db.rol, "id", "description", rol_id);
        }

        void ViewBagCreatePage(byte rol_id)
        {
           if (rol_id!=0)
                ViewBag.pagina_id = new SelectList(_repo.PaginasGet(rol_id).Select(x => new { id = x.id, pagina = x.pagina }).ToList().OrderBy(o => o.pagina), "id", "pagina");
           else
                ViewBag.pagina_id = new[] { new SelectListItem { } };
        }

        [ActionName("GetPages")]
        public void GetPages(byte rol_id)
        {
            PermisoCreateVM model = new PermisoCreateVM {};
            int page = 1;

            if (rol_id != 0)
            {
                IEnumerable<ListPaginas> list = _repo.PaginasList(rol_id);

                model.list = list.OrderBy(o => o.pagina).Skip((page - 1) * _pageSize).Take(_pageSize);
            }
            else
            {
                model.list = Enumerable.Empty<ListPaginas>();
            }

            //ViewBagCreate(rol_id);
            //return View(model);
        }

        #endregion

        #region create
        public ActionResult Create()
        {
            ViewBagCreate(0);

            ViewBagCreatePage(0);

            PermisoCreateVM model = new PermisoCreateVM {};

            //model.list = Enumerable.Empty<ListPaginas>();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PermisoCreateVM model)
        {
            try
            {
                if (model.Accion)
                {
                    if (ModelState.IsValid)
                    {
                        if (model.rol_id != 0)
                        {
                            rolpagina rp = new rolpagina()
                            {
                                rol_id = model.rol_id,
                                pagina_id = model.pagina_id,
                                asignada = true,                      
                                update_date = DateTime.Now,
                                update_user_id = int.Parse(Session["UserID"].ToString()),
                            };

                            db.rolpagina.Add(rp);
                            db.SaveChanges();

                        }
                        else
                        {
                            //model.list = Enumerable.Empty<ListPaginas>();
                            ViewBagCreatePage(0);
                        }

                        return RedirectToAction("Index");    
                    }
                }
                else
                {
                    if (model.rol_id != 0)
                    {
                        //IEnumerable<ListPaginas> list = _repo.PaginasList(model.rol_id);
                        //model.list = list.OrderBy(o => o.pagina).Skip((page - 1) * _pageSize).Take(_pageSize);
                        ViewBagCreatePage(model.rol_id);
                    }
                    else
                    {
                        // model.list = Enumerable.Empty<ListPaginas>();
                        ViewBagCreatePage(0);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }

            ViewBagCreate(model.rol_id);
            return View(model);
        }


        #endregion


        #region delete
        // GET: Users/Delete/5
        public ActionResult Delete(long id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rolpagina rp = db.rolpagina.Find(id);

            PermisoCreateVM model = new PermisoCreateVM()
            {
              pagina_id= rp.pagina_id,
              rol_id=rp.rol_id,
            };

            ViewBagDetail(model.pagina_id,model.rol_id);        
            return View(model);      

        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            try
            {
                rolpagina rp = db.rolpagina.Find(id);
                rp.asignada = false;
                rp.update_date = DateTime.Now;
                rp.update_user_id = int.Parse(Session["UserID"].ToString());
                db.Entry(rp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            return View();
        }


        void ViewBagDetail(byte pagina_id, byte rol_id)
        {
            IEnumerable<ListPaginas> list = _repo.PageName(pagina_id);
            ViewBag.page_name = list.Select(s => s.pagina).FirstOrDefault();

            IEnumerable<ListRol> listRoles = _repo.RolName(rol_id);
            ViewBag.rol_name = listRoles.Select(s => s.description).FirstOrDefault();
        }
        #endregion delete



    }
}