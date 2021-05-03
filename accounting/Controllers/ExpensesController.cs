using System;
using System.Collections.Generic;
using System.Configuration;
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

    [Authorize]
    [CustomAuthorizeAttribute]
    [SessionExpireFilter]
    public class ExpensesController : Controller
    {
        //private accountingContext db = new accountingContext();
        private AccountingEntities1 db = new AccountingEntities1();

        private IRepoCustom _repo;
        int _pageSize = 5;
        string path_file;

        #region --[CONSTRUCTOR]--

        public ExpensesController()
        {
            _repo = new RepoCustom();
            path_file = ConfigurationManager.AppSettings["path_files"].ToString();
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
            ViewBagCreateTipoComprobante(0);
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
                    string FileName = "";
                    if (model.file != null)
                    {
                        FileName = model.file.FileName;
                        string url_path = Path.Combine(Server.MapPath("~/path"), Path.GetFileName(FileName));
                        model.file.SaveAs(url_path);

                    }

                    expense exp = new expense()
                    {
                        name = model.name,
                        description = model.description,
                        expense_id = model.expense_id,
                        date_expense = model.date_expense,
                        register_date = DateTime.Now,
                        create_user_id = int.Parse(Session["UserID"].ToString()),
                        amount_money = model.amount_money,
                        name_file = FileName,
                        activo = 1,
                        selling_point = model.selling_point,
                        tipo_comprobante_id=model.tipo_comprobante_id,
                        nro_comprobante=model.nro_comprobante,
                        cuit_cuil=model.cuit_cuil,
                        nro_cuit_cuil=model.nro_cuit_cuil,
                        denominacion_emisor=model.denominacion_emisor,
                        imp_neto_gravado=model.imp_neto_gravado,
                        imp_neto_no_gravado=model.imp_neto_no_gravado,
                        imp_op_exentas = model.imp_op_exentas,
                        iva=model.iva,    
                        importe_total=model.importe_total,
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
            ViewBagCreateTipoComprobante(model.tipo_comprobante_id);

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
                amount_money=exp.amount_money,
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
                amount_money = exp.amount_money,
               // name_file = exp.name_file,
                selling_point = exp.selling_point,
                tipo_comprobante_id = exp.tipo_comprobante_id,
                nro_comprobante = exp.nro_comprobante,
                cuit_cuil = exp.cuit_cuil,
                nro_cuit_cuil = exp.nro_cuit_cuil,
                denominacion_emisor = exp.denominacion_emisor,
                imp_neto_gravado = exp.imp_neto_gravado,
                imp_neto_no_gravado = exp.imp_neto_no_gravado,
                imp_op_exentas = exp.imp_op_exentas,
                iva = exp.iva,
                importe_total = exp.importe_total,
            };

           // ViewBagCreate(model.expense_id);

            if (exp == null)
            {
                return HttpNotFound();
            }

            ViewBagCreate(model.expense_id);
            ViewBagCreateTipoComprobante(model.tipo_comprobante_id);

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
                        amount_money = model.amount_money,
                        activo = 1,
                        selling_point = model.selling_point,
                        tipo_comprobante_id = model.tipo_comprobante_id,
                        nro_comprobante = model.nro_comprobante,
                        cuit_cuil = model.cuit_cuil,
                        nro_cuit_cuil = model.nro_cuit_cuil,
                        denominacion_emisor = model.denominacion_emisor,
                        imp_neto_gravado = model.imp_neto_gravado,
                        imp_neto_no_gravado = model.imp_neto_no_gravado,
                        imp_op_exentas = model.imp_op_exentas,
                        iva = model.iva,
                        importe_total =model.importe_total,
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
            ViewBagCreateTipoComprobante(model.tipo_comprobante_id);
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
            exp.activo = 0;
            //_repo.ExpenseDelete(exp);
            _repo.ExpenseUpdate(exp);

            return RedirectToAction("Index");
        }

        #endregion --[DELETE]--

        #region --[EXTRA]--
        void ViewBagCreate(int expense_id)
        {
            ViewBag.expense_id = new SelectList(_repo.ExpenseTypeAll().Select(x => new { id = x.id, description = x.description }).ToList().OrderBy(o => o.description), "id", "description", expense_id);

        }

        void ViewBagCreateTipoComprobante(int tipo_comprobante_id)
        {
            ViewBag.tipo_comprobante_id = new SelectList(db.tipo_comprobante, "id", "descripcion", tipo_comprobante_id);
        }

        void ViewBagDetail(int expense_id)
        {
            IEnumerable<ListExpenseType> list = _repo.ExpenseTypeGetById(expense_id);
            ViewBag.expense_name = list.Select(s => s.description).FirstOrDefault();
        }



        #endregion --[EXTRA]--

        public FileResult Download(string name_file)
        {
            //return File("~/Download/EjemploAltaDirecta.csv", "text/csv", "AltaDirecta.csv");


            //if (!string.IsNullOrEmpty(name_file))
            //{
            //    string url_path = Path.Combine(Server.MapPath("~/path"), Path.GetFileName(name_file));
            //    //return File(url_path, "img/jpg", name_file);
            //    return File(url_path, "application/force- download", name_file);
            //}


            //return null;

            //-----------

            if (!string.IsNullOrEmpty(name_file))
            {
                //string arc = "ftp://wi381664.ferozo.com/path/" + name_file;
                string arc = "ftp://wi381664.ferozo.com/fundacion/path/" + name_file;
                string folder = ConfigurationManager.AppSettings["folder"];
                string archivo = folder + name_file;
                 //string archivo = "C:/Users/mcejas.NEWLINK/Desktop/Files/" + name_file;

                using (WebClient request = new WebClient())
                {
                    request.Credentials = new NetworkCredential("developer@institutosanignacio.com.ar", "Jmolina22");
                    byte[] fileData = request.DownloadData(arc);

                    using (FileStream file = System.IO.File.Create(@archivo))
                    {
                        file.Write(fileData, 0, fileData.Length);
                        file.Close();
                    }

                }

                return File(archivo, "img/jpg", name_file);
            }

            return null;

            //----------


        }



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
        }
        
    }
}
