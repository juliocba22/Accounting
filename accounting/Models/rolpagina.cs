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
    
    public partial class rolpagina
    {
        public byte id { get; set; }
        public byte rol_id { get; set; }
        public byte pagina_id { get; set; }
        public bool asignada { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
        public Nullable<int> update_user_id { get; set; }
    }
}
