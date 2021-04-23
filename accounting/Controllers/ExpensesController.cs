using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using accounting.Data;
using accounting.Helpers;
using accounting.Infra;
using accounting.Models;
using accounting.Repositories;
using accounting.ViewModels;

namespace accounting.Controllers
{

    /// <summary>
    /// GASTOS(EXPENSES)
    /// </summary>

    //[Authorize]
    //[CustomAuthorizeAttribute]
    //[SessionExpireFilter]
    public class ExpensesController : Controller
    {
        private accountingContext db = new accountingContext();

        private IRepoCustom _repo;
        int _pageSize = 5;

        #region --[CONSTRUCTOR]--

        public ExpensesController()
        {
            _repo = new RepoCustom();
        }

        public ExpensesController(IRepoCustom repoCustom)
        {
            _repo = repoCustom;
        }

        #endregion --[CONSTRUCTOR]--

        #region --[INDEX]--

        // GET: Expenses
        public ActionResult Index(string expense_type, int page = 1)
        {
            ExpenseIndexVM model = new ExpenseIndexVM { expense_type = expense_type, page = page };
            try
            {
                IEnumerable<ListExpense> list = _repo.ExpenseList(model.expense_type);

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
            ViewBagCreate(0);
            return View(new ExpenseCreateVM { register_date = DateTime.Now, date_expense = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExpenseCreateVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    byte[] img_load = null;
                    if (model.file != null)
                    {
                        WebImage img = new WebImage(model.file.InputStream);
                        img_load = img.GetBytes();

                    }



                    expense exp = new expense()
                    {
                        name = model.name,
                        description = model.description,
                        expense_id = model.expense_id,
                        date_expense = model.date_expense,
                        register_date = DateTime.Now,
                        create_user_id = int.Parse(Session["UserID"].ToString()),
                        amount = decimal.Parse(model.amount.ToString()),
                        image = img_load,//img.GetBytes(),

                    };

                    _repo.ExpenseAdd(exp);

                    //log.Info($"Usuario:{Session["UserID"]} - {User.Identity.Name} carga el archivo {model.file.FileName} - Grupo {model.group_name} con {cant} número de teléfonos.");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
                //log.Error($"Create - {ex.Message}", ex);
            }

            ViewBagCreate(model.expense_id);

            return View(model);
        }

        #endregion --[CREATE]--

        #region --[DETAILS]--

        // GET: Users/Details/5
        public ActionResult Details(long id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            expense exp = _repo.ExpenseFind(id);

            ExpenseCreateVM model = new ExpenseCreateVM()
            {
                id = exp.id,
                name = exp.name,
                description = exp.description,
                expense_id = exp.expense_id,
                date_expense = (DateTime)exp.date_expense,
                amount = (decimal)exp.amount,
            };


            if (exp == null)
            {
                return HttpNotFound();
            }

            ViewBagDetail(model.expense_id);

            return View(model);
        }

        #endregion --[DETAILS]--

        #region --[EDIT]--

        // GET: Users/Edit/5
        public ActionResult Edit(long id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            expense exp = _repo.ExpenseGet(id);

            ExpenseCreateVM model = new ExpenseCreateVM()
            {
                id = exp.id,
                name = exp.name,
                description = exp.description,
                expense_id = exp.expense_id,
                date_expense = (DateTime)exp.date_expense,
                amount = (decimal)exp.amount,
            };

            ViewBagCreate(model.expense_id);

            if (exp == null)
            {
                return HttpNotFound();
            }

            ViewBagCreate(model.expense_id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExpenseCreateVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    expense exp = new expense()
                    {
                        id = model.id,
                        name = model.name,
                        description = model.description,
                        expense_id = model.expense_id,
                        date_expense = model.date_expense,
                        register_date = DateTime.Now,//no se deberia modificar
                        update_date = DateTime.Now,
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                        amount = decimal.Parse(model.amount.ToString()),
                    };

                    _repo.ExpenseUpdate(exp);

                    //log.Info($"Usuario:{Session["UserID"]} - {User.Identity.Name} carga el archivo {model.file.FileName} - Grupo {model.group_name} con {cant} número de teléfonos.");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
                //log.Error($"Create - {ex.Message}", ex);
            }

            ViewBagCreate(model.expense_id);
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

            expense exp = _repo.ExpenseFind(id);

            ExpenseCreateVM model = new ExpenseCreateVM()
            {
                id = exp.id,
                name = exp.name,
                description = exp.description,
                expense_id = exp.expense_id,
                date_expense = (DateTime)exp.date_expense,
            };

            if (exp == null)
            {
                return HttpNotFound();
            }

            ViewBagDetail(model.expense_id);

            return View(model);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            expense exp = _repo.ExpenseFind(id);

            _repo.ExpenseDelete(exp);

            return RedirectToAction("Index");
        }

        #endregion --[DELETE]--

        #region --[EXTRA]--
        void ViewBagCreate(int expense_id)
        {
            ViewBag.expense_id = new SelectList(_repo.ExpenseTypeAll().Select(x => new { id = x.id, description = x.description }).ToList().OrderBy(o => o.description), "id", "description", expense_id);
        }

        void ViewBagDetail(int expense_id)
        {
            IEnumerable<ListExpenseType> list = _repo.ExpenseTypeGetById(expense_id);
            ViewBag.expense_name = list.Select(s => s.description).FirstOrDefault();
        }



        #endregion --[EXTRA]--

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult getImage(int id)
        {
            // expense exp = db.expenses.Find(id);
            expense exp = _repo.ExpenseFind(id);
            byte[] byteImage = exp.image;
            MemoryStream memoryStream = new MemoryStream(byteImage);
            Image image = Image.FromStream(memoryStream);

            memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Jpeg);
            memoryStream.Position = 0;

            return File(memoryStream, "image/jpg");
            //return File(memoryStream,"");


            //exp.image = memoryStream.ToArray();

            //ExpenseCreateVM model = new ExpenseCreateVM()
            //{
            //    id = exp.id,
            //    name = exp.name,
            //    description = exp.description,
            //    expense_id = exp.expense_id,
            //    date_expense = (DateTime)exp.date_expense,
            //    amount = (decimal)exp.amount,
            //    image=exp.image,
            //};

            //return File(exp.image, "image/jpg");


        }


        //[ActionName("getImage")]
        //public byte [] getImage(int id)
        //{
        //    expense exp = _repo.ExpenseFind(id);
        //    byte[] byteImage = exp.image;
        //    MemoryStream memoryStream = new MemoryStream(byteImage);
        //    Image image = Image.FromStream(memoryStream);

        //    memoryStream = new MemoryStream();
        //    image.Save(memoryStream, ImageFormat.Jpeg);
        //    memoryStream.Position = 0;

        //    //return File(memoryStream, "image/jpg");
        //    return memoryStream.ToArray();
        //}

        //public FileContentResult getImage(int id)
        //{
        //    //var owin = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        //    //var result = owin.FindByEmail<ApplicationUser, string>(User.Identity.Name);
        //    expense exp = _repo.ExpenseFind(id);

        //    //byte[] byteImage = exp.image;

        //    byte[] biteIMG = exp.image;

        //    MemoryStream m = new MemoryStream(biteIMG);


        //    Image image = Image.FromStream(m);

        //    m = new MemoryStream();

        //    image.Save(m, ImageFormat.Png);

        //    m.Position = 0;


        //    return new FileContentResult(biteIMG, "image/png");
        //}
    }
}
