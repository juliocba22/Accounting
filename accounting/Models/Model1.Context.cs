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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AccountingEntities1 : DbContext
    {
        public AccountingEntities1()
            : base("name=AccountingEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<expense_type> expense_type { get; set; }
        public virtual DbSet<product_service> product_service { get; set; }
        public virtual DbSet<profesional> profesional { get; set; }
        public virtual DbSet<social_work> social_work { get; set; }
        public virtual DbSet<users> users { get; set; }
        public virtual DbSet<work_order_status> work_order_status { get; set; }
        public virtual DbSet<tipo_comprobante> tipo_comprobante { get; set; }
        public virtual DbSet<compra> compra { get; set; }
        public virtual DbSet<client> client { get; set; }
        public virtual DbSet<pagina> pagina { get; set; }
        public virtual DbSet<rolpagina> rolpagina { get; set; }
        public virtual DbSet<proveedor> proveedor { get; set; }
        public virtual DbSet<work_order> work_order { get; set; }
        public virtual DbSet<factura_proveedores> factura_proveedores { get; set; }
        public virtual DbSet<orden_pago_cab> orden_pago_cab { get; set; }
        public virtual DbSet<orden_pago_det> orden_pago_det { get; set; }
        public virtual DbSet<expense> expense { get; set; }
        public virtual DbSet<cobros> cobros { get; set; }
        public virtual DbSet<rol> rol { get; set; }
    }
}
