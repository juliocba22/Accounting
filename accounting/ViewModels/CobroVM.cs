using accounting.Helpers;
using accounting.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace accounting.ViewModels
{
    public class CobroVM
    {
       
        public long id { get; set; }

        [Display(Name = "Nro. Recibo")]
        public long nroRecibo { get; set; }

        [Display(Name = "* Cliente")]
        [Required(ErrorMessage = "* requerido")]
        public int clienteId { get; set; }
        public string clienteDescripcion { get; set; }

        [Display(Name = "* Nro de Factura")]
        [Required(ErrorMessage = "* requerido")]
        public string nroFactura { get; set; }

        [Display(Name = "* Fecha de Factura")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "* requerido")]
        public DateTime fechaFactura { get; set; }

        [Display(Name = "* Monto")]
        [Required(ErrorMessage = "* requerido")]
        public double monto { get; set; }

        [Display(Name = "Recibo por el Total?")]
        [Required(ErrorMessage = "* requerido")]
        public string si_no { get; set; }

        [Display(Name = "Cobro Parcial")]
        public double cobroParcial { get; set; }

        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        [Display(Name = "Subtotal Recibo")]
        public double subtotalRecibo { get; set; }

        [Display(Name = "* Total")]
        [Required(ErrorMessage = "* requerido")]
        public double total { get; set; }
    }

    public class CobroVMIndex
    {
        public IEnumerable<ListCobros> list { get; set; }
        
        [Display(Name = "Nro de Factura")]
        public string nroFactura { get; set; }

        public int page { get; set; }

        public PagingInfo pagingInfo { get; set; }
    }

    public class CobrosExportPDFVM
    {
        public long id { get; set; }
        public long nroRecibo { get; set; }
        public string cliente { get; set; }
        public string nroFactura { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime fechaFactura { get; set; }
        public double monto { get; set; }
        public double cobroParcial { get; set; }
        public double subtotalRecibo { get; set; }
        public double total { get; set; }
    }
}