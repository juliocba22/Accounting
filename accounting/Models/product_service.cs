//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace accounting.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class product_service
    {
        public int id { get; set; }
        public int tipo { get; set; }
        public string nombre { get; set; }
        public double valorUnitario { get; set; }
        public System.DateTime update_date { get; set; }
        public int update_user_id { get; set; }
        public byte activo { get; set; }
        public Nullable<int> unidad_medida { get; set; }
        public Nullable<double> costo_profesional { get; set; }
        public Nullable<int> client_id { get; set; }
    }
}
