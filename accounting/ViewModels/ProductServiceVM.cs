using accounting.Helpers;
using accounting.Infra;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace accounting.ViewModels
{
    public class ProductServiceVM
    {
        [Display(Name = "Código")]
        public int id { get; set; }

        [Display(Name = "* Tipo")]
        [Required(ErrorMessage = "* requerido")]
        public int tipo { get; set; }

        [Display(Name = "* Nombre")]
        [Required(ErrorMessage = "* requerido")]
        public string nombre { get; set; }

        [Display(Name = "* Valor Unitario")]
        [Required(ErrorMessage = "* requerido")]
        public double valorUnitario { get; set; }
    }

    public class ProductServiceVMIndex
    {
        public IEnumerable<ListProductService> list { get; set; }

        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        public int page { get; set; }
        public PagingInfo pagingInfo { get; set; }
    }
}