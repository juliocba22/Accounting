using accounting.Helpers;
using accounting.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace accounting.ViewModels
{
    public class CompraVM
    {
        public long id { get; set; }

        [Display(Name = "* Proveedor")]
        [Required(ErrorMessage = "* requerido")]
        public long ProveedorId { get; set; }

        [Display(Name = "* Tipo de comprobante")]
        [Required(ErrorMessage = "* requerido")]
        public int TipoComprobanteId { get; set; }

        [Display(Name = "* Nro de Factura")]
        [Required(ErrorMessage = "* requerido")]
        public string NroFactura { get; set; }

        [Display(Name = "* Fecha emisión")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "* requerido")]
        public DateTime FechaEmision { get; set; }

        [Display(Name = "* Fecha contable")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "* requerido")]
        public DateTime FechaContable { get; set; }

        [Display(Name = "1er vencimiento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? PrimerVencimiento { get; set; }

        [Display(Name = "2do vencimiento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? SdoVencimiento { get; set; }

        [Display(Name = "* Importe")]
        [Required(ErrorMessage = "* requerido")]
        public double Importe { get; set; }

        [Display(Name = "Descuento global")]
        public double? DescuentoGlobal { get; set; }

        public string Proveedor { get; set; }

        public string TipoComprobante { get; set; }
    }

    public class CompraDeleteVM
    {
        public long id { get; set; }

        public string Proveedor { get; set; }

        public string TipoComprobante { get; set; }

        public string NroFactura { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaEmision { get; set; }

        public double Importe { get; set; }

    }

    public class CompraVMIndex
    {
        public IEnumerable<ListCompra> list { get; set; }
        public int? proveedor { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FechaEmisionDesde { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FechaEmisionHasta { get; set; }
        public int page { get; set; }

        public PagingInfo pagingInfo { get; set; }
    }
}