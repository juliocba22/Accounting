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
    
    public partial class proveedor
    {
        public long id { get; set; }
        public string dni { get; set; }
        public string cuit { get; set; }
        public string razon_social { get; set; }
        public string nombre_fantasia { get; set; }
        public string personeria { get; set; }
        public string provincia { get; set; }
        public string mail { get; set; }
        public string mail_facturacion { get; set; }
        public string direccion { get; set; }
        public string piso_dpto { get; set; }
        public string codigo_postal { get; set; }
        public string telefono { get; set; }
        public string localidad { get; set; }
        public System.DateTime register_date { get; set; }
        public Nullable<long> update_user_id { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
        public Nullable<int> create_user_id { get; set; }
        public byte activo { get; set; }
        public string cbu { get; set; }
        public string banco { get; set; }
        public string nro_cuenta { get; set; }
        public string alias { get; set; }
    }
}
