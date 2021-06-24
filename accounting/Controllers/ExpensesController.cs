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
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using accounting.Data;
using accounting.Helpers;
using accounting.Infra;
using accounting.Models;
using accounting.Repositories;
using accounting.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using static accounting.Helpers.Enumerables;

namespace accounting.Controllers
{

    /// <summary>
    /// GASTOS(EXPENSES)
    /// </summary>

    [Authorize]
    [HandleError(View = "Error")]
    public class ExpensesController : Controller
    {
        private AccountingEntities1 db = new AccountingEntities1();

        private IRepoCustom _repo;
        int _pageSize = 20;
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

                model.list = list.OrderByDescending(o => o.date_expense).Skip((page - 1) * _pageSize).Take(_pageSize);
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
            GetComboCC();
            GetComboProv();
            GetComboPeriodo();
            return View(new ExpenseCreateVM { register_date = DateTime.Now, date_expense = DateTime.Now, pay_state = "pendiente", pay_date = DateTime.Now });
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
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                        amount_money = 0,
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
                        proveedor_id = model.proveedor_id,
                        periodo = model.periodo,
                        pay_state = model.pay_state,
                        pay_date = model.pay_date
                    };

                    _repo.ExpenseAdd(exp);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }

            ViewBagCreate(model.expense_id);
            ViewBagCreateTipoComprobante(model.tipo_comprobante_id);
            GetComboCC();
            GetComboProv();
            GetComboPeriodo();

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

            ExpenseCreateVM model = new ExpenseCreateVM();
            model = _repo.GetExpenseDetail(id);

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
                proveedor_id = exp.proveedor_id,
                periodo = exp.periodo,
                pay_state = exp.pay_state,
                pay_date = exp.pay_date
            };

            if (exp == null)
            {
                return HttpNotFound();
            }

            ViewBagCreate(model.expense_id);
            ViewBagCreateTipoComprobante(model.tipo_comprobante_id);
            GetComboCC();
            GetComboProv();
            GetComboPeriodo();
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
                        amount_money = 0,
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
                        proveedor_id = model.proveedor_id,
                        periodo = model.periodo,
                        pay_state = model.pay_state,
                        pay_date = model.pay_date
                    };

                    _repo.ExpenseUpdate(exp);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }

            ViewBagCreate(model.expense_id);
            ViewBagCreateTipoComprobante(model.tipo_comprobante_id);
            GetComboCC();
            GetComboProv();
            GetComboPeriodo();
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
        private void GetComboCC()
        {
            enumCC e = new enumCC();
            ViewBag.CC = e.GetCCD(); 
        }
        void ViewBagCreate(int expense_id)
        {
            ViewBag.expense_id = new SelectList(_repo.ExpenseTypeAll().Select(x => new { id = x.id, description = x.description }).ToList().OrderBy(o => o.description), "id", "description", expense_id);

        }

        void ViewBagCreateTipoComprobante(int tipo_comprobante_id)
        {
            ViewBag.tipo_comprobante_id = new SelectList(db.tipo_comprobante, "id", "descripcion", tipo_comprobante_id);
        }

        void GetComboProv()
        {
            ViewBag.Prov = db.proveedor.Where(x => x.activo == 1).OrderBy(x => x.razon_social);
        }

        void ViewBagDetail(int expense_id)
        {
            IEnumerable<ListExpenseType> list = _repo.ExpenseTypeGetById(expense_id);
            ViewBag.expense_name = list.Select(s => s.description).FirstOrDefault();
        }

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
            System.Drawing.Image image = System.Drawing.Image.FromStream(memoryStream);

            memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Jpeg);
            memoryStream.Position = 0;

            return File(memoryStream, "image/jpg");
        }

        public FileContentResult Export([Bind(Include = "expense_type")] string expense_type)
        {
            StringBuilder csv = new StringBuilder();
            IEnumerable<ReportExpense> listado = _repo.ExpenseReport(expense_type);

            csv.AppendLine("Nombre del Voluntario;Tipo Gasto;Fecha Gasto;Tipo comprobante;Proveedor;Punto de venta;Nro. comprobante;CUIT/CUIL;Nro. CUIT/CUIL;Emisor;Descripcion;Imp. Neto Gravado;Imp. Neto No Gravado;Imp. Op. Exentas;IVA;Importe Total");
            foreach (var item in listado)
                csv.AppendLine(item.ToString());

            string archivo = "ReporteGastos_" + DateTime.Now.ToString("yyyyMMdd");
            return File(Encoding.Default.GetBytes(csv.ToString()), "text/csv", archivo + ".csv");
        }

        private void GetComboPeriodo()
        {
            Enumerables e = new Enumerables();
            ViewBag.Periodo = e.GetPeriodo();
        }


        private DataTable CreateDetalleDataTable()
        {
            DataTable tabla = new DataTable("DetalleCobros");
            DataColumn column;

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Nombre del Voluntario";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Tipo Gasto";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.DateTime");
            column.ColumnName = "Fecha Gasto";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Tipo comprobante";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Proveedor";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Punto de venta";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Nro comprobante";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "CUIT/CUIL";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Nro CUIT/CUIL";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Emisor";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Descripcion";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Decimal");
            column.ColumnName = "Imp Neto Gravado";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Decimal");
            column.ColumnName = "Imp Neto No Gravado";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Decimal");
            column.ColumnName = "Imp Op Exentas";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Decimal");
            column.ColumnName = "IVA";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Decimal");
            column.ColumnName = "Importe Total";
            tabla.Columns.Add(column);


            return tabla;
        }

        #endregion --[EXTRA]--

        #region Descarga PDF
        public void ExportPDF([Bind(Include = "id")] long id)
        {
            List<ExpensesExportPDFVM> list = _repo.GetExpenses(id);
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            document.Open();
            iTextSharp.text.Font fontTitle = FontFactory.GetFont(FontFactory.COURIER_BOLD, 25);
            iTextSharp.text.Font font9 = FontFactory.GetFont(FontFactory.TIMES, 9);
            iTextSharp.text.Font tituloTabla = FontFactory.GetFont(FontFactory.TIMES_BOLD, 9);

            //Cabecera
            Paragraph title = new Paragraph(20, "Gastos", fontTitle);
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);
            document.Add(new Paragraph(23, "Nombre del Voluntario: ", font9));
            document.Add(new Paragraph(24, "Tipo Gasto: ", font9));
            document.Add(new Paragraph(25, "Fecha Gasto: ", font9));
            document.Add(new Paragraph(26, "Tipo comprobante: ", font9));
            document.Add(new Paragraph(27, "Proveedor: ", font9));
            document.Add(new Paragraph(28, "Punto de venta: ", font9));
            document.Add(new Paragraph(29, "Nro comprobante: ", font9));
            document.Add(new Paragraph(30, "CUIT/CUIL: ", font9));
            document.Add(new Paragraph(31, "Nro CUIT/CUIL: ", font9));
            document.Add(new Paragraph(32, "Emisor: ", font9));
            document.Add(new Paragraph(33, "Descripcion: ", font9));
            document.Add(new Paragraph(34, "Imp Neto Gravado: ", font9));
            document.Add(new Paragraph(35, "Imp Neto No Gravado: ", font9));
            document.Add(new Paragraph(36, "Imp Op Exentas: ", font9));
            document.Add(new Paragraph(37, "IVA: ", font9));
            document.Add(new Paragraph(38, "Importe Total: ", font9));
            document.Add(new Chunk("\n"));

            //Detalle
            DataTable dt = CreateDetalleDataTable();
            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row["Nombre del Voluntario"] = item.name;
                row["Tipo Gasto"] = item.expense_name;
                row["Fecha Gasto"] = item.date_expense;
                row["Tipo comprobante"] = item.tipo_comprobante;
                row["Proveedor"] = item.proveedor;
                row["Punto de venta"] = item.selling_point;
                row["Nro comprobante"] = item.nro_comprobante;
                row["CUIT/CUIL"] = item.cuit_cuil;
                row["Nro CUIT/CUIL"] = item.nro_cuit_cuil;
                row["Emisor"] = item.denominacion_emisor;
                row["Descripcion"] = item.description;
                row["Imp Neto Gravado"] = item.imp_neto_gravado;
                row["Imp Neto No Gravado"] = item.imp_neto_no_gravado;
                row["Imp Op Exentas"] = item.imp_op_exentas;
                row["IVA"] = item.iva;
                row["Importe Total"] = item.importe_total;
                dt.Rows.Add(row);
            }
            PdfPTable table = new PdfPTable(dt.Columns.Count);

            float[] widths = new float[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
                widths[i] = 4f;

            table.SetWidths(widths);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = Element.ALIGN_CENTER;

            foreach (DataColumn c in dt.Columns)
            {
                PdfPCell cell = new PdfPCell();
                Paragraph p = new Paragraph(c.ColumnName, tituloTabla);
                p.Alignment = Element.ALIGN_CENTER;
                cell.AddElement(p);
                table.AddCell(cell);
            }
            foreach (DataRow r in dt.Rows)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int h = 0; h < dt.Columns.Count; h++)
                    {
                        PdfPCell cell = new PdfPCell();
                        Paragraph p = new Paragraph(r[h].ToString(), font9);
                        p.Alignment = Element.ALIGN_CENTER;
                        cell.AddElement(p);
                        table.AddCell(cell);
                    }
                }
            }
            document.Add(table);
            document.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Gastos" + ".pdf");
            HttpContext.Response.Write(document);
            Response.Flush();
            Response.End();
        }
        #endregion

    }
}
