using accounting.Models;
using accounting.Repositories;
using accounting.ViewModels;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace accounting.Controllers
{
    public class UserController : Controller
    {
        #region --[GLOBAL]--
        private IRepoCustom _repo;

        #endregion --[GLOBAL]--

        #region --[CONSTRUCTOR]--

        public UserController()
        {
            _repo = new RepoCustom();
        }

        public UserController(IRepoCustom repoCustom)
        {
            _repo = repoCustom;
        }

        #endregion --[CONSTRUCTOR]--

        #region --[LOGIN]--

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginVM model, string returnUrl)
        {
            try 
            {  
                if (!ModelState.IsValid)
                return View(model);

                if (_repo.UserValidate(model.user_name) != "")
                {
                    users user = _repo.UserGet(model.user_name, model.password);
                    if (user != null)
                    {
                        if (user.active == true)
                        {
                            switch (user.state_id)
                            {
                                case 0:
                                    // Usuario OK
                                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                        1,
                                        user.name,
                                        DateTime.Now,
                                        DateTime.Now.AddYears(1),
                                        model.rememberme,
                                        user.rol.description);

                                    string hashCookies = FormsAuthentication.Encrypt(ticket);
                                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
                                    Response.Cookies.Add(cookie);

                                    Session["UserID"] = user.id;

                                    return RedirectToLocal(returnUrl);
                                case 1:
                                    // Usuario Bloquedo.
                                    return View("Bloqueado");
                                case 2:
                                    // Usuario que debe cambiar la contraseña.
                                    Session["UserID"] = user.id;
                                    return RedirectToAction("ChangePassword");
                                default:
                                    // Si no encuentra un estado.
                                    return View("Error");
                            }
                        }
                        else
                            ViewBag.ValidationUser = "*Usuario Inactivo";
                    }
                    else
                        ViewBag.ValidationUser = "*Contraseña incorrecta";
                }
                else            
                    ViewBag.ValidationUser = "*Usuario incorrecto"; 
            }
            catch
            {
                return View(model);
            }
            
            return View(model);
        }

        #endregion --[LOGIN]--

        #region --[LOGOFF]--

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        #endregion --[LOGOFF]--

        #region --[CHANGE PASSWORD]--

        [AllowAnonymous]
        //public ActionResult ChangePassword()
        //{
        //    UserChangePasswordVM model = new UserChangePasswordVM();
        //    try
        //    {
        //        if (Session["UserID"] == null)
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        //        manager_user user = _repo.UserGet((int)Session["UserID"]);
        //        if (user == null)
        //            return HttpNotFound();

        //        model.id = user.id;
        //    }
        //    catch (Exception)
        //    {
        //        ModelState.AddModelError("", "Se produjo un error. Ponerse en contacto con el Administrador.");
        //    }

        //    return View(model);
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ChangePassword(UserChangePasswordVM model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        manager_user user = _repo.UserGet(model.id);
        //        if (user == null)
        //            return HttpNotFound();

        //        if (user.id != (int)Session["UserID"])
        //        {
        //            ModelState.AddModelError("", "Solo puede actualizar su contraseña.");
        //            return View(model);
        //        }

        //        if (user.password != model.password)
        //        {
        //            ModelState.AddModelError("Contrasena", "La contraseña actual no coincide.");
        //            return View(model);
        //        }

        //        user.password = model.new_password;
        //        user.state_id = (byte)Enums.StateUser.OK;

        //        _repo.UserUpdate(user);

        //        FormsAuthentication.SignOut();
        //        return RedirectToAction("Login", "Usuario", null);
        //    }

        //    return View(model);
        //}

        #endregion --[CHANGE PASSWORD]--

        #region --[EXTRA]--

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        #endregion --[EXTRA]--
    }
}