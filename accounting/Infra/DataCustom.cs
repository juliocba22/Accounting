using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace accounting.Infra
{
    #region --[Users]--

    /// <summary>
    /// Listado utilizado en SubscriberDirectController.Index
    /// </summary>
    public class ListUsers
    {
        public long id { get; set; }
        public string name { get; set; }
        public byte rol_id { get; set; }
        public string rol_description { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public bool active { get; set; }
        public long? update_user_id { get; set; }
        public DateTime? update_date { get; set; }


    }

    #endregion --[Users]--

    #region rol
    public class ListRol
    {
        public int id { get; set; }
        public string description { get; set; }
    }
    #endregion

    #region --[Expense]--

    public class ListExpense
    {
        public long id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public byte expense_id { get; set; }//para el combo
        public string expense_description { get; set; }
        public DateTime date_expense { get; set; }
        public decimal amount { get; set; }
        public byte[] image { get; set; }
    }

    #endregion --[Expense]--

    #region expense_type
    public class ListExpenseType
    {
        public int id { get; set; }
        public string description { get; set; }
    }
    #endregion

    #region --[SocialWork]--

    public class ListSocialWork
    {
        public long id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string phone { get; set; }
        public string mail { get; set; }
    }

    #endregion

    #region Client
    public class ListClient
    {
        public long id { get; set; }
        public string codigo { get; set; }
        public string razonSocial { get; set; } 
        public string localidad { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }

    }

    public class ReportClient
    {
        public string codigo { get; set; }
        public string razonSocial { get; set; }
        public string localidad { get; set; }
        public string provincia { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string emailFacturacion { get; set; }

        public override string ToString()
        {
            return $"{codigo};{razonSocial};{localidad};{provincia};{telefono};{email};{emailFacturacion}";
        }
    }

    #endregion

    #region ProductService
    public class ListProductService
    {  
        public int id { get; set; }
        public string tipo { get; set; }
        public string nombre { get; set; }
        public double valorUnitario { get; set; }
    }
    public class ReportProductService
    {
        public int id { get; set; }
        public string tipo { get; set; }
        public string nombre { get; set; }
        public double valorUnitario { get; set; }
        public override string ToString()
        {
            return $"{id};{nombre};{tipo};{valorUnitario}";
        }
    }
    #endregion

    #region Profesional
    public class ListProfesional
    {
        public int id { get; set; }
        public string servicio { get; set; }
        public string nombre { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
    }

    public class ReportProfesional
    {
        public string nombre { get; set; }
        public string servicio { get; set; }
        public string matricula { get; set; }
        public string cuit { get; set; }
        public string domicilio { get; set; }
        public string localidad { get; set; }
        public string provincia { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }

        public override string ToString()
        {
            return $"{nombre};{servicio};{matricula};{cuit};{domicilio};{localidad};{provincia};{telefono};{email}";
        }

    }
    #endregion
    #region Proveedor
    public class ListProveedor
    {
        public long id { get; set; }
        public string codigo { get; set; }
        public string razonSocial { get; set; }
        public string localidad { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }

    }

    public class ReportProveedor
    {
        public string codigo { get; set; }
        public string dni { get; set; }
        public string cuit { get; set; }
        public string razonSocial { get; set; }
        public string localidad { get; set; }
        public string provincia { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string emailFacturacion { get; set; }

        public override string ToString()
        {
            return $"{codigo};{dni};{cuit};{razonSocial};{localidad};{provincia};{telefono};{email};{emailFacturacion}";
        }
    }

    #endregion

    #region categoria impositiva
    public class ListCategoriaImpositiva
    {
        public int id { get; set; }
        public string descripcion { get; set; }
    }
    #endregion

    #region WorkOrder
    public class ComboProductService
    {
        public int id { get; set; }
        public string nombre { get; set; }
    }

    public class ListWorkOrder
    {
        public long id { get; set; }
        public string NroOrden { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }
        public string ProductServiceDesc { get; set; }
        public string StatusDesc { get; set; }
    }

    public class ReportWorkOrder
    {
        public string NroOrden { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public string ProductServiceDesc { get; set; }
        public string SocialWorkDesc { get; set; }
        public string ProfesionalDesc { get; set; }
        public string StatusDesc { get; set; }
        public double? Cantidad { get; set; }
        public string Paciente { get; set; }
        public string MotivoEliminacion { get; set; }

        public override string ToString()
        {
            return $"{NroOrden};{Fecha};{Descripcion};{ProductServiceDesc};{Cantidad};{Paciente};{SocialWorkDesc};{ProfesionalDesc};{StatusDesc};{MotivoEliminacion}";
        }
    }
    #endregion
}