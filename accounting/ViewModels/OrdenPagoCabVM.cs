using accounting.Helpers;
using accounting.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace accounting.ViewModels
{
    public class OrdenPagoCabVM
    {
        [Display(Name = "Nro Orden")]
        public long id { get; set; }

        [Display(Name = "* Profesional")]
        [Required(ErrorMessage = "* requerido")]
        public long Profesional_Id { get; set; }

        [Display(Name = "* Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "* requerido")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Importe")]
        public double Importe { get; set; }

        public int? create_user_id { get; set; }

        public int? update_user_id { get; set; }

        public string Profesional { get; set; }

        public bool? detalle { get; set; }
    }

    public class OrdenPagoVMIndex
    {
        public IEnumerable<ListOrdenPagoCab> list { get; set; }
        public long? Profesional_Id { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Fecha { get; set; }

        public int page { get; set; }

        public PagingInfo pagingInfo { get; set; }
    }

    public class OrdenesPagoCabDetVM
    {
        public long id { get; set; }
        public string Profesional { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }
        public double Importe { get; set; }
        public IEnumerable<ListOrdenPagoDet> list { get; set; }
        public PagingInfo pagingInfo { get; set; }
        public int page { get; set; }
    }

    public class OrdenesPagoDetVM
    {
        public long idCab { get; set; }
        public string Profesional { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }
        public double ImporteTotal { get; set; }

        public long? ProfesionalId { get; set; }

        public long? idDet { get; set; }

        [Display(Name = "* Factura")]
        [Required(ErrorMessage = "* requerido")]
        public long? FacturaProveedorId { get; set; }

        [Display(Name = "* Importe")]
        public double? Importe { get; set; }

        [Display(Name = "Paga Total")]
        public bool PagaTotal { get; set; }

        [Display(Name = "Forma de Pago")]
        public string FormaPago { get; set; }

        [Display(Name = "Banco")]   
        public string Banco { get; set; }

        [Display(Name = "Nro de Cheque")]
        public string NroCheque { get; set; }
        
        [Display(Name = "Nro de Cuenta Corriente")]
        public string NroCtaCte { get; set; }

        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }
    }

    public class OrdenesPagoDetailsDetalle
    {
        public long idCab { get; set; }
        public long idDet { get; set; }
        public long FacturaProveedorId { get; set; }
        public double Importe { get; set; }
        public string FormaPago { get; set; }
        public string Banco { get; set; }
        public string NroCheque { get; set; }
        public string NroCtaCte { get; set; }
        public string Observaciones { get; set; }
    }
}