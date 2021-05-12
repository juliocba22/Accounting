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
        [Display(Name = "Código")]
        public long id { get; set; }

        [Display(Name = "* Nro CUIT/CUIL/DNI")]
        [Required(ErrorMessage = "* requerido")]
        public string cuitNro { get; set; }

        [Display(Name = "* CUIT/CUIL/DNI")]
        [Required(ErrorMessage = "* requerido")]
        public string cuit { get; set; }

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

        [Display(Name = "Contacto")]
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

        public Nullable<System.DateTime> update_date { get; set; }
        public Nullable<int> update_user_id { get; set; }

        public DateTime register_date { get; set; }

        [Display(Name = "CBU")]
        public string cbu { get; set; }

        [Display(Name = "Banco")]
        public string banco { get; set; }

        [Display(Name = "Nro Cuenta")]
        public string nroCuenta { get; set; }

        [Display(Name = "Alias")]
        public string alias { get; set; }

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