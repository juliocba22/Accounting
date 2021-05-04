using accounting.Helpers;
using accounting.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace accounting.ViewModels
{
    public class ProfesionalVM
    {
        public int id { get; set; }

        [Display(Name = "* Servicio")]
        [Required(ErrorMessage = "* requerido")]
        public int product_service_id { get; set; }

        [Display(Name = "* Nombre  y Apellido")]
        [Required(ErrorMessage = "* requerido")]
        public string nombre { get; set; }

        [Display(Name = "Nro de Matrícula")]
        public string matricula { get; set; }

        [Display(Name = "CUIT")]
        public string cuit { get; set; }

        [Display(Name = "Domicilio")]
        public string domicilio { get; set; }

        [Display(Name = "Localidad")]
        public string localidad { get; set; }

        [Display(Name = "Provincia ")]
        public string provincia { get; set; }

        [Display(Name = "* Telefono")]
        [Required(ErrorMessage = "* requerido")]
        [Range(1111111111, 9999999999, ErrorMessage = "Debe ingresar un valor numérico de 10 dígitos")]
        public string telefono { get; set; }

        [Display(Name = "* Email")]
        [EmailAddress()]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "* requerido")]

        public string email { get; set; }

        public string servicioDesc { get; set; }
    }

    public class ProfesionalVMIndex
    {
        public IEnumerable<ListProfesional> list { get; set; }

        [Display(Name = "Nombre y Apellido")]
        public string nombre { get; set; }
        public int page { get; set; }
        public PagingInfo pagingInfo { get; set; }
    }
}