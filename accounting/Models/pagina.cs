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
    
    public partial class pagina
    {
        public byte id { get; set; }
        public string link_text { get; set; }
        public string action_name { get; set; }
        public string controller_name { get; set; }
        public string route { get; set; }
        public Nullable<byte> activo { get; set; }
        public Nullable<byte> orden_menu { get; set; }
        public string class_menu { get; set; }
        public string link_menu { get; set; }
    }
}
