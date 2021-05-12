using accounting.Helpers;
using accounting.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace accounting.ViewModels
{
    public class WorkOrderVM
    {
        [Display(Name = "Nro de Prestación")]
        public long id { get; set; }

        [Display(Name = "* Fecha")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "* requerido")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [Display(Name = "* Producto/Servicio")]
        [Required(ErrorMessage = "* requerido")]
        public int ProductServiceId { get; set; }

        [Display(Name = "Unidad de Medida")]
        public string UnidadMedida { get; set; }
        
        [Display(Name = "Valor Unitario")]
        public string ValorUnitario { get; set; }

        [Display(Name = "Costo Unitario Profesional")]
        public string CostoUniProf { get; set; }

        [Display(Name = "* Cantidad")]
        [Required(ErrorMessage = "* requerido")]
        public double? Cantidad { get; set; }

        [Display(Name = "Paciente")]
        public string Paciente { get; set; }

        [Display(Name = "Obra Social")]
        public string ObraSocial { get; set; }

        [Display(Name = "Profesional")]
        public int? ProfesionalId { get; set; }

        [Display(Name = "Cliente")]
        public int? ClientId { get; set; }

        [Display(Name = "* Estado")]
        [Required(ErrorMessage = "* requerido")]
        public int StatusId { get; set; }

        [Display(Name = "Motivo Eliminación")]
        public string MotivoEliminacion { get; set; }

        public string ProductServiceDesc { get; set; }

        public string SocialWorkDesc { get; set; }

        public string ProfesionalDesc { get; set; }

        public string StatusDesc { get; set; }

        public string Cliente { get; set; }

        [Display(Name = "* Total a Facturar")]
        [Required(ErrorMessage = "* requerido")]
        public double? Importe { get; set; }

        [Display(Name = "Costo Total Profesional")]
        public double? CostoProfesional { get; set; }

    }

    public class WorkOrderDeleteVM
    {
        public long id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }
        public string ProductService { get; set; }
        public string Status { get; set; }

        public string MotivoEliminacion { get; set; }
    }

    public class WorkOrderVMIndex
    {
        public IEnumerable<ListWorkOrder> list { get; set; }
        public int? status { get; set; }
        public int page { get; set; }
        public PagingInfo pagingInfo { get; set; }
    }
}