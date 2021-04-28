using accounting.Helpers;
using accounting.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace accounting.ViewModels
{
    public class WorkOrderVM
    {
        public long id { get; set; }

        [Display(Name = "* Nro de Orden")]
        [Required(ErrorMessage = "* requerido")]
        public string NroOrden { get; set; }

        [Display(Name = "* Fecha")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "* requerido")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [Display(Name = "* Producto/Servicio")]
        [Required(ErrorMessage = "* requerido")]
        public int ProductServiceId { get; set; }

        [Display(Name = "Cantidad")]
        public double? Cantidad { get; set; }

        [Display(Name = "Paciente")]
        public string Paciente { get; set; }

        [Display(Name = "Obra Social")]
        public int? SocialWorkId { get; set; }

        [Display(Name = "Profesional")]
        public int? ProfesionalId { get; set; }

        [Display(Name = "* Estado")]
        [Required(ErrorMessage = "* requerido")]
        public int StatusId { get; set; }

        [Display(Name = "Motivo Eliminación")]
        public string MotivoEliminacion { get; set; }

        public string ProductServiceDesc { get; set; }

        public string SocialWorkDesc { get; set; }

        public string ProfesionalDesc { get; set; }

        public string StatusDesc { get; set; }

    }

    public class WorkOrderDeleteVM
    {
        public long id { get; set; }
        public string NroOrden { get; set; }

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