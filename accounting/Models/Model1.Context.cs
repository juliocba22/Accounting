﻿//------------------------------------------------------------------------------
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
    
    public partial class AccountingEntities : DbContext
    {
        public AccountingEntities()
            : base("name=AccountingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<rol> rol { get; set; }
        public virtual DbSet<users> users { get; set; }
        public virtual DbSet<expense> expense { get; set; }
        public virtual DbSet<expense_type> expense_type { get; set; }
        public virtual DbSet<social_work> social_work { get; set; }
        public virtual DbSet<categoria_impositiva> categoria_impositiva { get; set; }
        public virtual DbSet<client> client { get; set; }
        public virtual DbSet<proveedor> proveedor { get; set; }
    }
}
