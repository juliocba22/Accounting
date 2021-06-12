using accounting.Helpers;
using accounting.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace accounting.ViewModels
{
    public class ExpenseIndexVM
    {

        #region --[INDEX]--
        public IEnumerable<ListExpense> list { get; set; }

        [Display(Name = "nombre")]
        public string name { get; set; }

        [Display(Name = "tipo gasto")]
        public string expense_type { get; set; }
        public int page { get; set; }
        public PagingInfo pagingInfo { get; set; }
       
        #endregion --[INDEX]--

    }

    public class ExpenseCreateVM
    {
        #region --[CREATE]--
        public long id { get; set; }

        [Display(Name = "* Nombre del Voluntario")]
        [Required(ErrorMessage = "* requiredo")]
        public string name { get; set; }

        [Display(Name = "* Periodo")]
        [Required(ErrorMessage = "* requerido")]
        public string periodo { get; set; }

        [Display(Name = "* Fecha de pago")]
        [Required(ErrorMessage = "* requerido")]
        [DataType(DataType.DateTime, ErrorMessage = "Formato inválido.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? pay_date { get; set; }

        [Display(Name = "* Estado")]
        public string pay_state { get; set; }

        [Display(Name = "* Descripcion")]
        [Required(ErrorMessage = "* requiredo")]
        public string description { get; set; }

        [Display(Name = "* Fecha gasto")]
        [Required(ErrorMessage = "* requiredo")]
        [DataType(DataType.DateTime, ErrorMessage = "Formato inválido.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? date_expense { get; set; }


        [Display(Name = "* Tipo gasto")]
        [Required(ErrorMessage = "* requiredo")]
        public byte expense_id { get; set; }

        [Display(Name = "Tipo Gasto")]
        public string expense_name { get; set; }//para details
        public DateTime register_date { get; set; }

        [Display(Name = "Monto gasto")]
        public double? amount_money { get; set; }

        [Display(Name = "Punto de venta")]
        public string selling_point { get; set; }

        [Display(Name = "* Tipo Comprobante")]
        [Required(ErrorMessage = "* requiredo")]
        public int tipo_comprobante_id { get; set; }

        [Display(Name = "Proveedor")]
        public long? proveedor_id { get; set; }

        [Display(Name = "* Nro. comprobante")]
        [Required(ErrorMessage = "* requiredo")]
        public string nro_comprobante { get; set; }

        [Display(Name = "* CUIT/CUIL")]
        [Required(ErrorMessage = "* requiredo")]
        public string cuit_cuil { get; set; }

        [Display(Name = "* Nro. cuit/cuil")]
        [Required(ErrorMessage = "* requiredo")]
        public string nro_cuit_cuil { get; set; }

        [Display(Name = "Denominacion Emisor")]
        public string denominacion_emisor { get; set; }

        [Display(Name = "Imp. Neto Gravado")]
        public double? imp_neto_gravado { get; set; }

        [Display(Name = "Imp. Neto No Gravado")]
        public double? imp_neto_no_gravado { get; set; }

        [Display(Name = "Imp. Op. Exentas")]
        public double? imp_op_exentas { get; set; }

        [Display(Name = "IVA")]
        public double? iva { get; set; }

        [Display(Name = "Importe Total")]
        public double? importe_total { get; set; }

        [Display(Name = "Archivo del gasto (image,excel,word,notes..) ")]
        public HttpPostedFileBase file { get; set; }

        public byte[] image { get; set; }

        public string tipo_comprobante { get; set; }

        public string proveedor { get; set; }

        public string user { get; set; }

        #endregion --[CREATE]--
    }
}