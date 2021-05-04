using accounting.Helpers;
using accounting.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace accounting.ViewModels
{
    public class ClientVM
    {
        [Display(Name = "Código")]
        public int id { get; set; }

        [Display(Name = "CUIT/CUIL/DNI")]
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
        [Range(1111111111, 9999999999, ErrorMessage = "Debe ingresar un valor numérico de 10 dígitos")]
        public string telefono { get; set; }

        [Display(Name = "* Email")]
        [EmailAddress()]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "* requerido")]
        public string email { get; set; }

        [Display(Name = "Email de Facturación")]
        [EmailAddress()]
        [DataType(DataType.EmailAddress)]
        public string emailFacturacon { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
        public Nullable<int> update_user_id { get; set; }
    }

    public class ClientVMIndex
    {
        public IEnumerable<ListClient> list { get; set; }

        [Display(Name = "Razón Social")]
        public string razonSocial { get; set; }
        public int page { get; set; }
        public PagingInfo pagingInfo { get; set; }


    }

}
