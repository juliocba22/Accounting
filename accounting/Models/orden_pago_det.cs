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
    
    public partial class orden_pago_det
    {
        public long id { get; set; }
        public long orden_pago_cab_id { get; set; }
        public long factura_proveedores_id { get; set; }
        public bool paga_total { get; set; }
        public double importe { get; set; }
        public string forma_pago { get; set; }
        public string nro_cheque { get; set; }
        public string nro_cuenta_corriente { get; set; }
        public string banco { get; set; }
        public string observaciones { get; set; }
    }
}
