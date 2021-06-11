using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using accounting.Helpers;
using accounting.Infra;
using accounting.Models;
using accounting.Repositories;
using accounting.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;

namespace accounting.Controllers
{
    [Authorize]
    [HandleError(View = "Error")]
    public class OrdenPagoController : Controller
    {
        private AccountingEntities1 db = new AccountingEntities1();
        private IRepoCustom _repo;
        int _pageSize = 20;
        DateTime FechaOrden;

        public OrdenPagoController()
        {
            _repo = new RepoCustom();
        }

        #region Listado de OP Cabecera
        public ActionResult Index(long? profesional_id, DateTime? Fecha, int page = 1)
        {
            GetProfesionales();
            if (Fecha != null)
            {
                string fechaD = Convert.ToDateTime(Fecha).Year.ToString() + "/" + Convert.ToDateTime(Fecha).Month.ToString() + "/" + Convert.ToDateTime(Fecha).Day.ToString();
                FechaOrden = Convert.ToDateTime(fechaD);
            }

            OrdenPagoVMIndex model = new OrdenPagoVMIndex { Profesional_Id = profesional_id, Fecha = Fecha == null ? Fecha : FechaOrden, page = page };
            try
            {
                IEnumerable<ListOrdenPagoCab> list = _repo.OrdenPagoCabList(model.Profesional_Id, model.Fecha);

                model.list = list.OrderBy(o => o.FechaOrden).Skip((page - 1) * _pageSize).Take(_pageSize);
                model.pagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = _pageSize,
                    TotalItems = list.Count()
                };
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            return View(model);
        }
        #endregion

        #region Nueva OP cabecera
        public ActionResult Create()
        {
            GetProfesionales();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrdenPagoCabVM OpVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    orden_pago_cab c = new orden_pago_cab
                    {
                        fecha = OpVM.Fecha,
                        profesional_id = OpVM.Profesional_Id,
                        importe_total = 0,
                        update_date = DateTime.Now,
                        create_user_id = int.Parse(Session["UserID"].ToString()),
                        activo = 1
                    };
                    db.orden_pago_cab.Add(c);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            GetProfesionales();
            return View(OpVM);
        }
        #endregion

        #region Editar OP cabecera
        public ActionResult Edit(long? id)
        {
            bool detalle = false;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            orden_pago_cab orden_pago_cab = db.orden_pago_cab.Find(id);

            orden_pago_det orden_pago_det = db.orden_pago_det.Where(x=> x.orden_pago_cab_id == id).FirstOrDefault();
            if (orden_pago_det != null)
                detalle = true;

            OrdenPagoCabVM opVm = new OrdenPagoCabVM
            {
                id = orden_pago_cab.id,
                Fecha = orden_pago_cab.fecha,
                Profesional_Id = orden_pago_cab.profesional_id,
                Importe = orden_pago_cab.importe_total,
                create_user_id = orden_pago_cab.create_user_id,
                detalle = detalle
            };

            GetProfesionales();

            return View(opVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrdenPagoCabVM opVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    orden_pago_cab op = new orden_pago_cab
                    {
                        id = opVm.id,
                        fecha = opVm.Fecha,
                        profesional_id = opVm.Profesional_Id,
                        importe_total = opVm.Importe,
                        update_date = DateTime.Now,
                        create_user_id = opVm.create_user_id,
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                        activo = 1,
                    };

                    db.Entry(op).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            GetProfesionales();
            return View(opVm);
        }
        #endregion

        #region eliminar OP cabecera
        public ActionResult Delete(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                OrdenPagoCabVM opVM = new OrdenPagoCabVM();
                opVM = _repo.GetDeleteOP(id);

                return View(opVM);
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //eliminacion logica de la cabecera
                    orden_pago_cab orden_pago_cab = db.orden_pago_cab.Find(id);
                    orden_pago_cab.activo = 0;
                    
                    //Busco si la cabecera tiene detalle y recorro.
                    List<orden_pago_det> OpDet = db.orden_pago_det.Where(x => x.orden_pago_cab_id == id).ToList();                  
                    foreach (var det in OpDet)
                    {
                        //actualizacion del estado de la factura del profesional asociado
                        factura_proveedores fp = db.factura_proveedores.Where(x => x.id == det.factura_proveedores_id).First();
                        if (fp != null)
                        {
                            if (fp.estado == 0 && det.importe == fp.importe_total) //pagado y los importes son iguales, queda impago
                            {
                                orden_pago_cab.importe_total = orden_pago_cab.importe_total - det.importe;
                                fp.estado = 2;
                            }
                            else if (fp.estado == 0 && det.importe < fp.importe_total) //pagado y el importe del detalle es menor a la factura, queda pago parcial.
                            {
                                orden_pago_cab.importe_total = orden_pago_cab.importe_total - det.importe;
                                fp.estado = 1;
                            }
                            else if (fp.estado == 1 && det.importe < fp.importe_total) //Pago parcial y el detalle es menor a la factura, queda pago parcial.
                            {
                                fp.estado = 1;
                                orden_pago_cab.importe_total = orden_pago_cab.importe_total - det.importe;
                            }

                            if (orden_pago_cab.importe_total == 0)
                                fp.estado = 2;

                            //actualizo cabecera
                            db.Entry(orden_pago_cab).State = EntityState.Modified;
                            db.SaveChanges();

                            //actualizo factura
                            db.Entry(fp).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        //eliminacion fisica del detalle
                        db.orden_pago_det.Remove(det);
                        db.SaveChanges();
                    }

                    if (OpDet.Count() == 0)//cabecera sin detalle. 
                    {
                        //actualizo cabecera
                        db.Entry(orden_pago_cab).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            return View();
        }
        #endregion

        #region Index con Cabecera y Detalle
        public ActionResult Details(long? id, int page = 1)
        {
            OrdenesPagoCabDetVM opvm = new OrdenesPagoCabDetVM();
            opvm = _repo.GetOrdenPagoCab(id);
            opvm.pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = _pageSize,
                TotalItems = opvm.list.Count()
            };
            return View(opvm);
        }

        #endregion

        #region Nueva OP Detalle
        public ActionResult CreateDetalle(long idCab)
        {
            OrdenesPagoDetVM opvm = _repo.GetOrdenPagoCabDet(idCab);
            GetFacturas(opvm.ProfesionalId);
            GetFormasPago();
            return View(opvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDetalle(OrdenesPagoDetVM OPvm, string submit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    factura_proveedores fp = db.factura_proveedores.Where(x => x.id == OPvm.FacturaProveedorId).First();
                    double importeDetalle = 0;

                    if (OPvm.PagaTotal == false) //ingresa los valores a mano, por eso controlo.
                    {
                        if (OPvm.Importe == null || OPvm.Importe == 0)
                        {
                            GetFacturas(OPvm.ProfesionalId);
                            GetFormasPago();
                            ModelState.AddModelError("", "Debe ingresar un importe mayor a 0!");
                            return View(OPvm);
                        }
                        //Controlo que el importe a pagar sea menor al ingresado.
                        if (OPvm.Importe > fp.importe_total)
                        {
                            GetFacturas(OPvm.ProfesionalId);
                            GetFormasPago();
                            ModelState.AddModelError("", "El importe ingresado en la Orden de Pago, no puede ser mayor al saldo por pagar de la Factura que es de: " + fp.importe_total);
                            return View(OPvm);
                        }
                        else if (fp.estado == 1) //tiene pago parcial. O sea, le queda algo por saldar pero no es todo el importe de la factura
                        {
                            //Ver en que detalle de orden de pago está para sacar el importe y hacer la diferencia.
                            double importeCargado = db.orden_pago_det.Where(x => x.factura_proveedores_id == OPvm.FacturaProveedorId).Select(x => x.importe).FirstOrDefault();
                            double diferencia = fp.importe_total - importeCargado;
                            if (OPvm.Importe > diferencia)
                            {
                                GetFacturas(OPvm.ProfesionalId);
                                GetFormasPago();
                                ModelState.AddModelError("", "El importe ingresado en la Orden de Pago, no puede ser mayor al saldo por pagar de la Factura que es de: " + diferencia);
                                return View(OPvm);
                            }
                        }
                        importeDetalle = (double)OPvm.Importe;
                    }
                    else
                    {
                        //tengo que controlar si ya hay algo pago de esa factura
                        if (fp.estado == 1) //pago parcial
                        {
                            double importesSum = 0;
                            List<double> importesCargados = db.orden_pago_det.Where(x => x.factura_proveedores_id == OPvm.FacturaProveedorId).Select(x => x.importe).ToList();
                            foreach (var l in importesCargados)
                            {
                                importesSum = importesSum + l;
                            }
                            double diferencia = fp.importe_total - importesSum;
                            importeDetalle = diferencia;
                        }
                        else //es 2, significa que no tiene nada pago
                            importeDetalle = fp.importe_total;
                    }
                    //Guardo el detalle
                    orden_pago_det opdet = new orden_pago_det
                    {
                       orden_pago_cab_id = OPvm.idCab,
                       factura_proveedores_id = (long)OPvm.FacturaProveedorId,
                       paga_total = OPvm.PagaTotal,
                       importe = importeDetalle,
                       forma_pago = OPvm.FormaPago,
                       nro_cheque = OPvm.NroCheque,
                       nro_cuenta_corriente = OPvm.NroCtaCte,
                       banco = OPvm.Banco,
                       observaciones = OPvm.Observaciones
                    };

                    db.orden_pago_det.Add(opdet);
                    db.SaveChanges();

                    //Actualizo el importe en la cabecera
                    orden_pago_cab orden_pago_cab = db.orden_pago_cab.Find(OPvm.idCab);
                    orden_pago_cab.importe_total = orden_pago_cab.importe_total + importeDetalle;
                    db.Entry(orden_pago_cab).State = EntityState.Modified;
                    db.SaveChanges();

                    //actualizo el estado de la factur
                    fp.estado = OPvm.PagaTotal == true ? 0 : 1;
                    db.Entry(fp).State = EntityState.Modified;
                    db.SaveChanges();

                    //segun el boton veo adonde lo llevo.
                    if (submit == "Guardar y Volver")
                        return RedirectToAction("Details", new { id = OPvm.idCab, page = 1 });
                    else
                        return RedirectToAction("CreateDetalle", new { idCab = OPvm.idCab});
                }
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            GetFacturas(OPvm.ProfesionalId);
            GetFormasPago();
            return View(OPvm);
        }
        #endregion

        #region Detalle de un detalle de OP
        public ActionResult DetailsDetalle(long id)
        {
            OrdenesPagoDetailsDetalle opvm = _repo.GetOrdenPagoDetails(id);
            return View(opvm);
        }
        #endregion

        #region Eliminacion de un detalle de OP
        public ActionResult DeleteDetalle(long id)
        {
            OrdenesPagoDetailsDetalle opvm = _repo.GetOrdenPagoDetails(id);
            return View(opvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDetalle(OrdenesPagoDetailsDetalle opvm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //cacebera
                    orden_pago_cab orden_pago_cab = db.orden_pago_cab.Find(opvm.idCab);

                    //factura 
                    factura_proveedores fp = db.factura_proveedores.Find(opvm.FacturaProveedorId);

                    //detalle
                    orden_pago_det orden_pago_det = db.orden_pago_det.Find(opvm.idDet);

                    //actualizacion del importe en la cabecera
                    orden_pago_cab.importe_total = orden_pago_cab.importe_total - orden_pago_det.importe;
                    db.Entry(orden_pago_cab).State = EntityState.Modified;
                    db.SaveChanges();

                    //actualizo estado de la factura
                    if (fp.estado == 0) //pagado
                    {
                        if (fp.importe_total == orden_pago_det.importe)
                            fp.estado = 2;
                        else if (fp.importe_total > orden_pago_det.importe)
                            fp.estado = 1;
                    }
                    else if (fp.estado == 1) //pago parcial.
                    {
                        //tengo que buscar si existe la factura en algun detalle que no sea el actual para diferenciar si queda pago parcial o no.
                        int cont = db.orden_pago_det.Where(x => x.factura_proveedores_id == opvm.FacturaProveedorId && x.id != opvm.idDet).Count();
                        if (cont == 0)
                            fp.estado = 2;
                    }

                    db.Entry(fp).State = EntityState.Modified;
                    db.SaveChanges();

                    //eliminacion fisica del detalle
                    db.orden_pago_det.Remove(orden_pago_det);
                    db.SaveChanges();

                    return RedirectToAction("Details", new { id = opvm.idCab, page = 1 });
                }
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
              return View(opvm);
        }
        #endregion

        #region Descarga PDF
        public void ExportPDF([Bind(Include = "id")] long id)
        {
            OrdenesPagoCabDetVM OP = _repo.GetOrdenPagoCab(id);            
            Document document = new Document();
            document.Open();
            Font fontTitle = FontFactory.GetFont(FontFactory.COURIER_BOLD, 25);
            Font font9 = FontFactory.GetFont(FontFactory.TIMES, 9);
            Font tituloTabla = FontFactory.GetFont(FontFactory.TIMES_BOLD, 9);

            //Cabecera
            Paragraph title = new Paragraph(20, "Orden de Pago", fontTitle);
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);
            document.Add(new Paragraph(23, "Nro de Orden: " + OP.id, font9));
            document.Add(new Paragraph(24, "Fecha: " + OP.Fecha.ToShortDateString(), font9));
            document.Add(new Paragraph(25, "Profesional: " + OP.Profesional, font9));
            document.Add(new Paragraph(26, "Importe Total: " + OP.Importe, font9));
            document.Add(new Chunk("\n"));

            //Detalle
            DataTable dt = CreateDetalleDataTable();
            foreach (var item in OP.list)
            {
                DataRow row = dt.NewRow();
                row["Factura"] = item.factura_proveedor_id;
                row["Paga Total"] = item.PagaTotal;
                row["Forma Pago"] = item.FormaPago;
                row["Banco"] = item.Banco;
                row["Nro Cheque"] = item.NroCheque;
                row["Nro Cta Cte"] = item.NroCtaCte;
                row["Observaciones"] = item.Observaciones;
                row["Importe"] = item.Importe;
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
            Response.AddHeader("content-disposition", "attachment;filename=Orden de Pago - Nro " + OP.id + ".pdf");
            HttpContext.Response.Write(document);
            Response.Flush();
            Response.End();
        }
        #endregion

        #region Métodos privados
        private DataTable CreateDetalleDataTable()
        {
            DataTable tabla = new DataTable("DetalleOP");
            DataColumn column;

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int64");
            column.ColumnName = "Factura";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Paga Total";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Forma Pago";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Banco";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Nro Cheque";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Nro Cta Cte";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Observaciones";
            tabla.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Decimal");
            column.ColumnName = "Importe";
            tabla.Columns.Add(column);

            return tabla;
        }
        private void GetProfesionales()
        {
            ViewBag.Prof = db.profesional.Where(x => x.activo == 1).OrderBy(x => x.nombre).ToList();
        }
        private void GetFacturas(long? profesionalId)
        {
            ViewBag.Factura= db.factura_proveedores.Where(x => x.profesional_id == profesionalId && x.estado != 0 && x.activo == 1).OrderBy(x => x.id).ToList();
        }
        private void GetFormasPago()
        {
            Enumerables e = new Enumerables();
            ViewBag.FP = e.GetFormasPago();
        }
        [ActionName("CamposGet")]
        public string CamposGet(long id)
        {
            IEnumerable<OrdenPagoDetImporte> opdi = GetImporteFactura(id);
            var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };
            string json = JsonConvert.SerializeObject(opdi, Formatting.Indented, serializerSettings);

            return json;
        }
        private IEnumerable<OrdenPagoDetImporte> GetImporteFactura(long id)
        {
            IEnumerable<OrdenPagoDetImporte>  d;
            d = (from c in db.factura_proveedores
                 where c.id == id
                 && c.activo == 1
                 select new OrdenPagoDetImporte
                 {
                     id = c.id,
                     saldo = c.importe_total
                 }).ToList();
            
            return d;
        }

        public FileContentResult Export([Bind(Include = "profesional_id, Fecha")] long? profesional_id, DateTime? Fecha)
        {
            StringBuilder csv = new StringBuilder();
            IEnumerable<ReportOrdenPago> listado = _repo.OrdenPagoReport(profesional_id, Fecha);

            csv.AppendLine("Nro de OP;Fecha;Profesional;Importe Total");
            foreach (var item in listado)
                csv.AppendLine(item.ToString());

            string archivo = "ReporteListadoOrdenesDePago_" + DateTime.Now.ToString("yyyyMMdd");
            return File(Encoding.Default.GetBytes(csv.ToString()), "text/csv", archivo + ".csv");
        }
        #endregion
    }
}
