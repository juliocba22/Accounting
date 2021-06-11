using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using accounting.Helpers;
using accounting.Infra;
using accounting.Models;
using accounting.Repositories;
using accounting.ViewModels;
using Newtonsoft.Json;
using static accounting.Helpers.Enumerables;

namespace accounting.Controllers
{
    [Authorize]
    [HandleError(View = "Error")]
    public class FacturaProveedoresController : Controller
    {
        private AccountingEntities1 db = new AccountingEntities1();

        private IRepoCustom _repo;
        int _pageSize = 20;
        DateTime FechaEmisionD;
        DateTime FechaEmisionH;

        public FacturaProveedoresController()
        {
            _repo = new RepoCustom();
        }

        #region Listado
        public ActionResult Index(int? estado, DateTime? FechaEmisionDesde, DateTime? FechaEmisionHasta, int page = 1)
        {
            GetEstado();

            if (FechaEmisionDesde != null)
            {
                string fechaD = Convert.ToDateTime(FechaEmisionDesde).Year.ToString() + "/" + Convert.ToDateTime(FechaEmisionDesde).Month.ToString() + "/" + Convert.ToDateTime(FechaEmisionDesde).Day.ToString();
                FechaEmisionD = Convert.ToDateTime(fechaD);
            }

            if (FechaEmisionHasta != null)
            {
                string fechaH = Convert.ToDateTime(FechaEmisionHasta).Year.ToString() + "/" + Convert.ToDateTime(FechaEmisionHasta).Month.ToString() + "/" + Convert.ToDateTime(FechaEmisionHasta).Day.ToString();
                FechaEmisionH = Convert.ToDateTime(fechaH);
            }
            FacturaProveedoresVMIndex model = new FacturaProveedoresVMIndex { estado = estado, FechaEmisionDesde = FechaEmisionDesde == null ? FechaEmisionDesde : FechaEmisionD, FechaEmisionHasta = FechaEmisionHasta == null ? FechaEmisionHasta : FechaEmisionH, page = page };
            try
            {
                IEnumerable<ListFacturaProveedores> list = _repo.FacturaProveedoresList(model.estado, model.FechaEmisionDesde, model.FechaEmisionHasta);

                model.list = list.OrderBy(o => o.FechaFactura).Skip((page - 1) * _pageSize).Take(_pageSize);
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

        #region Detalle
        public ActionResult Details(long? id)
        {
            FacturaProveedoresVM fp = new FacturaProveedoresVM();
            fp = _repo.FacturaProveedorGet(id);

            return View(fp);
        }
        #endregion

        #region Nuevo
        public ActionResult Create()
        {
            GetTipoFactura();
            GetTipoComprobante();
            GetProfesionales();
            GetEstado();
            GetComboCC();
            GetComboPeriodo();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FacturaProveedoresVM fpVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string FileName = "";
                    if (fpVM.file != null)
                    {
                        FileName = fpVM.file.FileName;
                        string url_path = Path.Combine(Server.MapPath("~/path"), Path.GetFileName(FileName));
                        fpVM.file.SaveAs(url_path);
                    }
                    factura_proveedores fp = new factura_proveedores()
                    {
                        periodo = fpVM.periodo,
                        fecha_factura = fpVM.fecha_factura,
                        fecha_pago = fpVM.fecha_pago,
                        tipo_factura = fpVM.tipo_factura,
                        create_user_id = int.Parse(Session["UserID"].ToString()),
                        update_date = DateTime.Now,
                        tipo_comprobante_id = fpVM.tipo_comprobante_id, 
                        profesional_id = fpVM.profesional_id,
                        punto_venta = fpVM.punto_venta,
                        nro_comprobante = fpVM.nro_comprobante,
                        cuit_cuil = fpVM.cuit_cuil,
                        nro_cuit_cuil = fpVM.nro_cuit_cuil,
                        description = fpVM.description,
                        imp_neto_gravado = fpVM.imp_neto_gravado,
                        imp_neto_no_gravado = fpVM.imp_neto_no_gravado,
                        imp_op_exentas = fpVM.imp_op_exentas,
                        iva = fpVM.iva,
                        importe_total = fpVM.importe_total,
                        name_file = FileName,
                        estado = fpVM.estado,
                        activo = 1
                    };

                    db.factura_proveedores.Add(fp);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            GetTipoFactura();
            GetTipoComprobante();
            GetProfesionales();
            GetEstado();
            GetComboCC();
            GetComboPeriodo();
            return View(fpVM);
        }
        #endregion

        #region Edicion      
        public ActionResult Edit(long? id)
        {
            factura_proveedores fp = db.factura_proveedores.Find(id);

            FacturaProveedoresVM fpVM = new FacturaProveedoresVM
            {
                id = fp.id,
                periodo = fp.periodo,
                fecha_factura = fp.fecha_factura,
                fecha_pago = fp.fecha_pago,
                tipo_factura = fp.tipo_factura,
                create_user_id = fp.create_user_id,
                tipo_comprobante_id = fp.tipo_comprobante_id,  
                punto_venta = fp.punto_venta,
                nro_comprobante = fp.nro_comprobante,
                cuit_cuil = fp.cuit_cuil,
                nro_cuit_cuil = fp.nro_cuit_cuil,
                description = fp.description,
                imp_neto_gravado = fp.imp_neto_gravado,
                imp_neto_no_gravado = fp.imp_neto_no_gravado,
                imp_op_exentas = fp.imp_op_exentas,
                iva = fp.iva,
                importe_total = fp.importe_total,
                estado = fp.estado,
                fileName = fp.name_file,
                profesional_id = fp.profesional_id
            };
            GetProfesionales();
            GetComboPeriodo();
            GetTipoFactura();
            GetTipoComprobante();        
            GetEstado();
            GetComboCC();
            GetUser();
            return View(fpVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FacturaProveedoresVM fpVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    factura_proveedores fp = new factura_proveedores
                    {
                        id = fpVM.id,
                        periodo = fpVM.periodo,
                        fecha_factura = fpVM.fecha_factura,
                        fecha_pago = fpVM.fecha_pago,
                        tipo_factura = fpVM.tipo_factura,
                        create_user_id = fpVM.create_user_id,
                        update_date = DateTime.Now,
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                        tipo_comprobante_id = fpVM.tipo_comprobante_id,    
                        punto_venta = fpVM.punto_venta,
                        nro_comprobante = fpVM.nro_comprobante,
                        cuit_cuil = fpVM.cuit_cuil,
                        nro_cuit_cuil = fpVM.nro_cuit_cuil,
                        description = fpVM.description,
                        imp_neto_gravado = fpVM.imp_neto_gravado,
                        imp_neto_no_gravado = fpVM.imp_neto_no_gravado,
                        imp_op_exentas = fpVM.imp_op_exentas,
                        iva = fpVM.iva,
                        importe_total = fpVM.importe_total,
                        estado = fpVM.estado,
                        name_file = fpVM.fileName,
                        activo = 1,
                        profesional_id = fpVM.profesional_id
                    };
                    db.Entry(fp).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            GetComboPeriodo();
            GetTipoFactura();
            GetTipoComprobante();
            GetProfesionales();
            GetEstado();
            GetComboCC();
            GetUser();
            return View(fpVM);
        }
        #endregion

        #region eliminar
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            factura_proveedores fp = db.factura_proveedores.Find(id);

            FacturaProveedoresVM fpVM = new FacturaProveedoresVM
            {
                id = fp.id,
                periodo = fp.periodo,
                fecha_factura = fp.fecha_factura,
                fecha_pago = fp.fecha_pago,
                nro_comprobante = fp.nro_comprobante,
                description = fp.description,
                importe_total = fp.importe_total,
                estadoDesc = fp.estado == 0 ? "PAGADO" : fp.estado == 1 ? "PAGO PARCIAL" : "IMPAGO"
            };
            return View(fpVM);

         }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            try
            {
                factura_proveedores factura_proveedores = db.factura_proveedores.Find(id);
                factura_proveedores.activo = 0;
                db.Entry(factura_proveedores).State = EntityState.Modified;
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

        #region Métodos Privados
        private void GetTipoFactura()
        {
            Enumerables e = new Enumerables();
            ViewBag.TipoFac = e.GetTipoFactura();
        }
        private void GetTipoComprobante()
        {
            ViewBag.TipoComp = db.tipo_comprobante.ToList();
        }
        private void GetProfesionales()
        {
            ViewBag.Prof = db.profesional.Where(x => x.activo == 1).OrderBy(x => x.nombre).ToList();
        }
        private void GetEstado()
        {
            Enumerables e = new Enumerables();
            ViewBag.Est = e.GetEstadoFacturaProveedores();
        }
        private void GetComboCC()
        {
            enumCC e = new enumCC();
            ViewBag.CC = e.GetCCD();
        }
        private void GetUser()
        {
            ViewBag.UserCreate = db.users.Where(x => x.active == true).OrderBy(x => x.user_name).ToList();
        }
        private void GetComboPeriodo()
        {
            Enumerables e = new Enumerables();           
            ViewBag.Per = e.GetPeriodo();
        }
        public FileContentResult Export([Bind(Include = "estado, FechaD, FechaH")] int? estado, DateTime? FechaD, DateTime? FechaH)
        {
            StringBuilder csv = new StringBuilder();
            IEnumerable<ReportFacturaProveedores> listado = _repo.FacturaProveedoresReport(estado, FechaD, FechaH);

            csv.AppendLine("Nro de Factura;Período;Fecha de Pago;Tipo de Factura;Fecha de Factura;Tipo de Comprobante;Profesional;Punto de Venta;Nro de Comprobante; CUIT/CUIL;Nro CUIT/CUIL; Descripción;Imp Neto Gravado; Imp Neto No Gravado;Imp. Op. Exentas;IVA;Importe Total;Estado;Cargada por;");
            foreach (var item in listado)
                csv.AppendLine(item.ToString());

            string archivo = "ReporteListadoFacturasProveedores_" + DateTime.Now.ToString("yyyyMMdd");
            return File(Encoding.Default.GetBytes(csv.ToString()), "text/csv", archivo + ".csv");
        }
        #endregion
    }
}
