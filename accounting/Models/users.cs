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
    
    public partial class users
    {
        public long id { get; set; }
        public string name { get; set; }
        public byte rol_id { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public bool active { get; set; }
        public System.DateTime register_date { get; set; }
        public byte state_id { get; set; }
        public int client_id { get; set; }
        public Nullable<long> update_user_id { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
        public Nullable<int> create_user_id { get; set; }
    
        public virtual rol rol { get; set; }
    }
}
