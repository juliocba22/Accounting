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
    [CustomAuthorizeAttribute]
    [SessionExpireFilter]
    public class SocialWorkController : Controller
    {
        /// <summary>
        /// OBRASOCIAL(SOCIALWORK)
        /// </summary>

        private IRepoCustom _repo;
        int _pageSize = 5;


        #region --[CONSTRUCTOR]--

        public SocialWorkController()
        {
            _repo = new RepoCustom();
        }

        public SocialWorkController(IRepoCustom repoCustom)
        {
            _repo = repoCustom;
        }

        #endregion --[CONSTRUCTOR]--

        #region --[INDEX]--

        // GET: Expenses
        public ActionResult Index(string name, int page = 1)
        {
            SocialWorkIndexVM model = new SocialWorkIndexVM { name = name, page = page };
            try
            {
                IEnumerable<ListSocialWork> list = _repo.SocialWorkList(model.name);

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

            return View(model);
        }

        #endregion --[INDEX]--

        #region --[CREATE]--

        public ActionResult Create()
        {
            //ViewBagCreate(0);
            return View(new SocialWorkCreateVM { register_date = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SocialWorkCreateVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    social_work sw = new social_work()
                    {
                        name = model.name,
                        description = model.description,
                        phone = model.phone,
                        mail = model.mail,
                        register_date = DateTime.Now,
                        create_user_id = int.Parse(Session["UserID"].ToString()),

                    };

                    _repo.SocialWorkAdd(sw);

                    //log.Info($"Usuario:{Session["UserID"]} - {User.Identity.Name} carga el archivo {model.file.FileName} - Grupo {model.group_name} con {cant} número de teléfonos.");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
                //log.Error($"Create - {ex.Message}", ex);
            }

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

            social_work sw = _repo.SocialWorkGet(id);

            SocialWorkCreateVM model = new SocialWorkCreateVM()
            {
                id = sw.id,
                name = sw.name,
                description = sw.description,
                phone = sw.phone,
                mail = sw.mail,
                register_date = sw.register_date,
            };

            if (sw == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SocialWorkCreateVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    social_work sw = new social_work()
                    {
                        id = model.id,
                        name = model.name,
                        description = model.description,
                        phone = model.phone,
                        mail = model.mail,
                        register_date = model.register_date,
                        update_date = DateTime.Now,
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                    };

                    _repo.SocialWorkUpdate(sw);

                    //log.Info($"Usuario:{Session["UserID"]} - {User.Identity.Name} carga el archivo {model.file.FileName} - Grupo {model.group_name} con {cant} número de teléfonos.");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
                //log.Error($"Create - {ex.Message}", ex);
            }

            return View(model);
        }
        #endregion --[EDIT]--

        #region --[DETAILS]--

        // GET: Users/Details/5
        public ActionResult Details(long id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            social_work sw = _repo.SocialWorkFind(id);

            SocialWorkCreateVM model = new SocialWorkCreateVM()
            {
                id = sw.id,
                name = sw.name,
                description = sw.description,
                phone = sw.phone,
                mail = sw.mail,
            };


            if (sw == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        #endregion --[DETAILS]--

        #region --[DELETE]--
        // GET: Users/Delete/5
        public ActionResult Delete(long id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            social_work sw = _repo.SocialWorkFind(id);

            SocialWorkCreateVM model = new SocialWorkCreateVM()
            {
                id = sw.id,
                name = sw.name,
                description = sw.description,
                phone = sw.phone,
                mail = sw.mail,
            };

            if (sw == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {

            social_work sw = _repo.SocialWorkFind(id);
            sw.activo = 0;
            _repo.SocialWorkUpdate(sw);

            return RedirectToAction("Index");

        }

        #endregion --[DELETE]--


    }
}
