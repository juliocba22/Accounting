using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using accounting.Models;
using accounting.Helpers;
using accounting.Repositories;
using accounting.ViewModels;
using accounting.Infra;
using static accounting.Helpers.Enumerables;
using System.Text;
using ceTe.DynamicPDF;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.tool.xml;

namespace accounting.Controllers
{
    [Authorize]
    [HandleError(View = "Error")]
    public class CobroController : Controller
    {
       
        #region Variables
        private AccountingEntities1 db = new AccountingEntities1();
        private IRepoCustom _repo = new RepoCustom();
        int _pageSize = 20;

        #endregion

        #region Listado
        // GET: clients
        public ActionResult Index(string nroFactura, int page = 1)
        {
            CobroVMIndex model = new CobroVMIndex { nroFactura = nroFactura, page = page };
            try
            {
                IEnumerable<ListCobros> list = _repo.CobrosList(model.nroFactura);

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


        #region Nuevo
        // GET: clients/Create
        public ActionResult Create()
        {
            CobroVM model = new CobroVM{ nroRecibo = GetNroRecibo(), fechaFactura = DateTime.Now }; 
            GetComboYN();
            GetComboClientes();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CobroVM cobro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    long NroRecibo = cobro.nroRecibo;
                    cobros c = new cobros()
                    {
                        nro_factura = cobro.nroFactura,
                        fecha_factura = cobro.fechaFactura,
                        monto = cobro.monto,
                        cobro_parcial = cobro.cobroParcial,
                        observaciones = cobro.Observaciones,
                        subtotal_recibo = cobro.subtotalRecibo,
                        total = cobro.total,
                        update_date = DateTime.Now,
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                        activo = 1,
                        cliente_id = cobro.clienteId,
                        nro_recibo = NroRecibo,// cobro.nroRecibo,
                    };

                    db.cobros.Add(c);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            GetComboYN();
            GetComboClientes();
            return View();
        }
        #endregion

        #region editar
        // GET: Compra/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cobros co = db.cobros.Find(id);

          
            CobroVM c = new CobroVM()
            {
                id = co.id,
                nroFactura = co.nro_factura,
                fechaFactura = co.fecha_factura,
                monto = co.monto,
                cobroParcial = (double)co.cobro_parcial,
                Observaciones = co.observaciones,
                subtotalRecibo = (double)co.subtotal_recibo,
                total = co.total,
                clienteId = co.cliente_id,
                nroRecibo = co.nro_recibo,
            };
            GetComboYN();
            GetComboClientes();

            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CobroVM cobroVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    cobros co = new cobros()
                    {
                        id = cobroVM.id,
                        nro_factura = cobroVM.nroFactura,
                        fecha_factura = cobroVM.fechaFactura,
                        monto = cobroVM.monto,
                        cobro_parcial = cobroVM.cobroParcial,
                        observaciones = cobroVM.Observaciones,
                        subtotal_recibo = cobroVM.subtotalRecibo,
                        total = cobroVM.total,
                        cliente_id = cobroVM.clienteId,
                        nro_recibo = cobroVM.nroRecibo,
                        activo = 1,
                        update_date = DateTime.Now,
                        update_user_id = int.Parse(Session["UserID"].ToString())
                    };

                    db.Entry(co).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }

            GetComboYN();
            GetComboClientes();
            return View();
        }
        #endregion

        #region Detalle
        // GET: clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cobros cobro = db.cobros.Find(id);

            CobroVM c = new CobroVM()
            {
                id = cobro.id,
                nroFactura = cobro.nro_factura,
                fechaFactura = cobro.fecha_factura,
                monto = cobro.monto,
                cobroParcial = (double)cobro.cobro_parcial,
                Observaciones = cobro.observaciones,
                subtotalRecibo = (double)cobro.subtotal_recibo,
                total = cobro.total,
                clienteId = cobro.cliente_id,
                nroRecibo = cobro.nro_recibo,
            };

            c.clienteDescripcion = GetClienteDesc(c.clienteId);

            return View(c);
        }
        #endregion

        #region Eliminacion
        // GET: clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cobros cobro = db.cobros.Find(id);

            CobroVM c = new CobroVM()
            {
                id = cobro.id,
                nroRecibo = cobro.nro_recibo,
                nroFactura = cobro.nro_factura,
                fechaFactura = cobro.fecha_factura,
                monto = cobro.monto
            };

            return View(c);
        }

        // POST: clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                cobros cobro = db.cobros.Find(id);
                cobro.activo = 0;
                db.Entry(cobro).State = EntityState.Modified;
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

        string GetClienteDesc(int clienteId)
        {
            var query = db.client.Where(c => c.activo == 1 && c.id == clienteId ).FirstOrDefault();
            if (query != null)
                return query.razonSocial;
            return "";
        }

        void GetComboClientes()
        {
            ViewBag.Clientes = db.client.Where(x => x.activo == 1).OrderBy(x => x.razonSocial);
        }
        private void GetComboYN()
        {
            enumYN e = new enumYN();
            ViewBag.YN = e.GetYN();
        }

        public long GetNroRecibo()
        {
            return db.cobros.DefaultIfEmpty().Max(c => c == null ? 0 : c.id) + 1;
        }

        public FileContentResult Export([Bind(Include = "nroFactura")] string nroFactura)
        {
            StringBuilder csv = new StringBuilder();
            IEnumerable<ReportCobros> listado = _repo.CobrosReport(nroFactura);

            csv.AppendLine("Nro Recibo;Cliente;Nro Factura;Fecha Factura;Monto;Cobro Parcial;SubTotal Recibo;Total");
            foreach (var item in listado)
                csv.AppendLine(item.ToString());

            string archivo = "ReporteCobros_" + DateTime.Now.ToString("yyyyMMdd");
            return File(Encoding.Default.GetBytes(csv.ToString()), "text/csv", archivo + ".csv");
        }

        private DataTable CreateDetalleDataTable()
        {
            DataTable tabla = new DataTable("DetalleCobros");
            DataColumn column;

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int64");
            column.ColumnName = "NroRecibo";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Cliente";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "NroFactura";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.DateTime");
            column.ColumnName = "FechaFactura";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Decimal");
            column.ColumnName = "Monto";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Decimal");
            column.ColumnName = "CobroParcial";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Decimal");
            column.ColumnName = "SubtotalRecibo";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Decimal");
            column.ColumnName = "Total";
            tabla.Columns.Add(column);

            return tabla;
        }
        #endregion

        #region Descarga PDF
        //public void ExportPDF([Bind(Include = "id")] long id)
        //{
        //    List<CobrosExportPDFVM> list = _repo.GetCobros(id);
        //    iTextSharp.text.Document document = new iTextSharp.text.Document();
        //    document.Open();
        //    iTextSharp.text.Font fontTitle = FontFactory.GetFont(FontFactory.COURIER_BOLD, 25);
        //    iTextSharp.text.Font font9 = FontFactory.GetFont(FontFactory.TIMES, 9);
        //    iTextSharp.text.Font tituloTabla = FontFactory.GetFont(FontFactory.TIMES_BOLD, 9);

        //    //Cabecera
        //    Paragraph title = new Paragraph(20, "Cobros", fontTitle);
        //    title.Alignment = Element.ALIGN_CENTER;
        //    document.Add(title);
        //    document.Add(new Paragraph(23, "Nro Recibo: "));
        //    document.Add(new Paragraph(24, "Cliente: ", font9));
        //    document.Add(new Paragraph(25, "Nro Factura: ", font9));
        //    document.Add(new Paragraph(26, "Fecha Factura: ", font9));
        //    document.Add(new Paragraph(27, "Monto: ", font9));
        //    document.Add(new Paragraph(28, "Cobro Parcial: ", font9));
        //    document.Add(new Paragraph(29, "Subtotal Recibo: ", font9));
        //    document.Add(new Paragraph(30, "Total: ", font9));
        //    document.Add(new Chunk("\n"));

        //    //Detalle
        //    DataTable dt = CreateDetalleDataTable();
        //    foreach (var item in list)
        //    {
        //        DataRow row = dt.NewRow();
        //        row["NroRecibo"] = item.nroRecibo;
        //        row["Cliente"] = item.cliente;
        //        row["NroFactura"] = item.nroFactura;
        //        row["FechaFactura"] = item.fechaFactura;
        //        row["Monto"] = item.monto;
        //        row["CobroParcial"] = item.cobroParcial;
        //        row["SubtotalRecibo"] = item.subtotalRecibo;
        //        row["Total"] = item.total;
        //        dt.Rows.Add(row);
        //    }
        //    PdfPTable table = new PdfPTable(dt.Columns.Count);

        //    float[] widths = new float[dt.Columns.Count];
        //    for (int i = 0; i < dt.Columns.Count; i++)
        //        widths[i] = 4f;

        //    table.SetWidths(widths);
        //    table.WidthPercentage = 100;
        //    table.HorizontalAlignment = Element.ALIGN_CENTER;

        //    foreach (DataColumn c in dt.Columns)
        //    {
        //        PdfPCell cell = new PdfPCell();
        //        Paragraph p = new Paragraph(c.ColumnName, tituloTabla);
        //        p.Alignment = Element.ALIGN_CENTER;
        //        cell.AddElement(p);
        //        table.AddCell(cell);
        //    }
        //    foreach (DataRow r in dt.Rows)
        //    {
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int h = 0; h < dt.Columns.Count; h++)
        //            {
        //                PdfPCell cell = new PdfPCell();
        //                Paragraph p = new Paragraph(r[h].ToString(), font9);
        //                p.Alignment = Element.ALIGN_CENTER;
        //                cell.AddElement(p);
        //                table.AddCell(cell);
        //            }
        //        }
        //    }
        //    document.Add(table);
        //    document.Close();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "attachment;filename=Cobros.pdf");
        //    HttpContext.Response.Write(document);
        //    Response.Flush();
        //    Response.End();
        //}

        public ActionResult ExportPDF([Bind(Include = "nroFactura")] string nroFactura)
        {
            CobroVMIndex model = new CobroVMIndex { nroFactura = nroFactura };
            IEnumerable<ListCobros> list = _repo.CobrosList(nroFactura);
            model.list = list;
            var HTMLViewStr = RenderViewToString(ControllerContext,
            "~/Views/Cobro/ExportPDF.cshtml",
            model);

            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(HTMLViewStr);
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "Cobros.pdf");
            }
        }

        static string RenderViewToString(ControllerContext context, string viewPath, object model = null, bool partial = false)
        {
            ViewEngineResult viewEngineResult = null;
            if (partial)
                viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
            else
                viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException("View cannot be found.");

            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,
                                            context.Controller.ViewData,
                                            context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }

        #endregion

    }
}
