using accounting.Helpers;
using accounting.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace accounting.ViewModels
{
    public class ProveedorVM
    {
        public long id { get; set; }

        [Display(Name = "* Dni")]
        [Required(ErrorMessage = "* requerido")]
        public string dni { get; set; }

        [Display(Name = "* Cuit")]
        [Required(ErrorMessage = "* requerido")]
        public string cuit { get; set; }

        [Display(Name = "* Código")]
        [Required(ErrorMessage = "* requerido")]
        public string codigo { get; set; }

        [Display(Name = "* Razón Social")]
        [Required(ErrorMessage = "* requerido")]
        public string razonSocial { get; set; }

        [Display(Name = "Nombre de Fantasía")]
        public string nombreFantasia { get; set; }

        [Display(Name = "* Localidad")]
        [Required(ErrorMessage = "* requerido")]
        public string localidad { get; set; }

        [Display(Name = "* Provincia")]
        [Required(ErrorMessage = "* requerido")]
        public string provincia { get; set; }

        [Display(Name = "Personería ")]
        public string personeria { get; set; }

        [Display(Name = "* Telefono")]
        [Required(ErrorMessage = "* requerido")]
        [Range(0, 9999999999, ErrorMessage = "Debe ingresar un valor numérico de 10 dígitos")]
        public string telefono { get; set; }

        [Display(Name = "* Email")]
        [EmailAddress()]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "* requerido")]
        public string mail { get; set; }

        [Display(Name = "Email de Facturación")]
        [EmailAddress()]
        [DataType(DataType.EmailAddress)]
        public string mailFacturacion { get; set; }

        [Display(Name = "* Direccion")]
        [Required(ErrorMessage = "* requerido")]
        public string direccion { get; set; }

        [Display(Name = "Codigo Postal")]
        public string codigoPostal { get; set; }

        [Display(Name = "Piso/Dpto")]
        public string pisoDpto { get; set; }

        [Display(Name = "Categoria Impositiva")]
        [Required(ErrorMessage = "* requerido")]
        public byte categoria_impositiva_id { get; set; }

        [Display(Name = "categoria impositiva")]
        public string categoria_impositiva_name { get; set; }

        public Nullable<System.DateTime> update_date { get; set; }
        public Nullable<int> update_user_id { get; set; }

        public DateTime register_date { get; set; }
    }

    public class ProveedorVMIndex
    {
        public IEnumerable<ListProveedor> list { get; set; }

        [Display(Name = "Razón Social")]
        public string razonSocial { get; set; }
        public int page { get; set; }
        public PagingInfo pagingInfo { get; set; }


    }

    public class ProveedorReporteVM
    { 
    }
}