using accounting.Helpers;
using accounting.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace accounting.ViewModels
{
    public class FacturaProveedoresVM
    {
        [Display(Name = "Nro de Factura")]
        public long id { get; set; }

        [Display(Name = "* Período")]
        [Required(ErrorMessage = "* requerido")]
        public string periodo { get; set; }

        [Display(Name = "* Fecha Pago")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "* requerido")]
        public DateTime? fecha_pago { get; set; }

        [Display(Name = "* Tipo Factura")]
        [Required(ErrorMessage = "* requerido")]
        public string tipo_factura { get; set; }

        [Display(Name = "* Fecha Factura")]
        [Required(ErrorMessage = "* requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_factura { get; set; }

        [Display(Name = "Creada por")]
        public long? create_user_id { get; set; }

        [Display(Name = "Creada por")]
        public string user_create { get; set; }

        [Display(Name = "Actualizada por")]
        public int? update_user_id { get; set; }

        [Display(Name = "Actualizada por")]
        public string user_update { get; set; }

        [Display(Name = "* Tipo Comprobante")]
        [Required(ErrorMessage = "* requerido")]
        public int tipo_comprobante_id { get; set; }

        [Display(Name = "Punto de venta")]
        public string punto_venta { get; set; }

        [Display(Name = "* Nro. comprobante")]
        [Required(ErrorMessage = "* requerido")]
        public string nro_comprobante { get; set; }

        [Display(Name = "* CUIT/CUIL")]
        [Required(ErrorMessage = "* requerido")]
        public string cuit_cuil { get; set; }

        [Display(Name = "* Nro. cuit/cuil")]
        [Required(ErrorMessage = "* requerido")]
        public string nro_cuit_cuil { get; set; }

        [Display(Name = "* Descripcion")]
        [Required(ErrorMessage = "* requerido")]
        public string description { get; set; }

        [Display(Name = "Imp. Neto Gravado")]
        public double? imp_neto_gravado { get; set; }

        [Display(Name = "Imp. Neto No Gravado")]
        public double? imp_neto_no_gravado { get; set; }

        [Display(Name = "Imp. Op. Exentas")]
        public double? imp_op_exentas { get; set; }

        [Display(Name = "IVA")]
        public double? iva { get; set; }

        [Display(Name = "* Importe Total")]
        [Required(ErrorMessage = "* requerido")]
        public double importe_total { get; set; }

        [Display(Name = "Archivo de la factura (image,excel,word,notes..) ")]
        public HttpPostedFileBase file { get; set; }

        [Display(Name = "* Estado")]
        [Required(ErrorMessage = "* requerido")]
        public int? estado { get; set; }

        public string estadoDesc { get; set; }

        public string tipo_comprobante { get; set; }
        public string fileName { get; set; }
        public string profesional { get; set; }
        
        [Display(Name = "* Profesional")]
        [Required(ErrorMessage = "* requerido")]
        public long? profesional_id { get; set; }
    }

    public class FacturaProveedoresVMIndex
    {
        public IEnumerable<ListFacturaProveedores> list { get; set; }
        public int? estado { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FechaEmisionDesde { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FechaEmisionHasta { get; set; }
        public int page { get; set; }

        public PagingInfo pagingInfo { get; set; }
    }

    public class ComboProvProf
    {
        public long id { get; set; }
        public string desc { get; set; }
    }

}