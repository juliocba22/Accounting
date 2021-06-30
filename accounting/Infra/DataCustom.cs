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
        public double amount { get; set; }
        public double amount_money { get; set; }
        public byte[] image { get; set; }
        public string path_file { get; set; }
        public string name_file { get; set; }
        public string user { get; set; }
        public string periodo { get; set; }
        public string proveedor { get; set; }
        public string desc_comprobante { get; set; }
        public string nro_comprobante { get; set; }
        public double importe_total { get; set; }
    }

    public class ReportExpense
    {
        public string name { get; set; }
        public string description { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? date_expense { get; set; }
        public string expense_name { get; set; }//para details
        public double amount_money { get; set; }
        public string selling_point { get; set; }
        public string nro_comprobante { get; set; }
        public string cuit_cuil { get; set; }
        public string nro_cuit_cuil { get; set; }
        public string denominacion_emisor { get; set; }
        public double? imp_neto_gravado { get; set; }
        public double? imp_neto_no_gravado { get; set; }
        public double? imp_op_exentas { get; set; }
        public double? iva { get; set; }
        public double? importe_total { get; set; }
        public string tipo_comprobante { get; set; }
        public string proveedor { get; set; }
        public override string ToString()
        {
            return $"{name};{expense_name};{date_expense};{tipo_comprobante};{proveedor};{selling_point};{nro_comprobante};{cuit_cuil};{nro_cuit_cuil};{denominacion_emisor};{description};{imp_neto_gravado};{imp_neto_no_gravado};{imp_op_exentas};{iva};{importe_total}";
        }
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
        public string razonSocial { get; set; } 
        public string localidad { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }

    }

    public class ReportClient
    {
        public long id { get; set; }
        public string razonSocial { get; set; }
        public string localidad { get; set; }
        public string provincia { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string emailFacturacion { get; set; }
        public string codigo { get; set; }
        public string nroCodigo { get; set; }
        public string nombreContacto { get; set; }

        public override string ToString()
        {
            return $"{id};{razonSocial};{localidad};{provincia};{nombreContacto};{telefono};{email};{emailFacturacion};{codigo};{nroCodigo}";
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

        public string cliente { get; set; }
    }
    public class ReportProductService
    {
        public int id { get; set; }
        public string tipo { get; set; }
        public string nombre { get; set; }
        public double valorUnitario { get; set; }
        public double? costoProfesional { get; set; }

        public string unidadMedida { get; set; }

        public string cliente { get; set; }
        public override string ToString()
        {
            return $"{id};{nombre};{tipo};{unidadMedida};{valorUnitario};{costoProfesional};{cliente}";
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
        public string cuitNro { get; set; }
        public string tipoFacturacion { get; set; }
        public string cbu { get; set; }
        public string banco { get; set; }
        public string nroCuenta { get; set; }
        public string alias { get; set; }

        public override string ToString()
        {
            return $"{nombre};{servicio};{matricula};{cuit};{cuitNro};{domicilio};{localidad};{provincia};{telefono};{email};{tipoFacturacion};{cbu};{banco};{nroCuenta};{alias}";
        }

    }
    #endregion

    #region Proveedor
    public class ListProveedor
    {
        public long id { get; set; }
        public string razonSocial { get; set; }
        public string localidad { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }

    }

    public class ReportProveedor
    {
        public long codigo { get; set; }
        public string cuitNro { get; set; }
        public string cuit { get; set; }
        public string razonSocial { get; set; }
        public string nombreFantasia { get; set; }
        public string localidad { get; set; }
        public string contacto { get; set; }
        public string provincia { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public string cp { get; set; }
        public string piso { get; set; }
        public string email { get; set; }
        public string emailFacturacion { get; set; }
        public string cbu { get; set; }
        public string banco { get; set; }
        public string nroCuenta { get; set; }
        public string alias { get; set; }

        public override string ToString()
        {
            return $"{codigo};{nombreFantasia};{razonSocial};{cuit};{cuitNro};{contacto};{email};{emailFacturacion};{telefono};{provincia};{localidad};{direccion};{piso};{cp};{cbu};{banco};{nroCuenta};{alias}";
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

    public class ComboProductServiceValues
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public double valUnitario { get; set; }
        public string unidadMedida { get; set; }
        public double? CostoProf { get; set; }
        public string Cliente { get; set; }
    }

    public class ListWorkOrder
    {
        public long id { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }
        public string ProductServiceDesc { get; set; }
        public string StatusDesc { get; set; }

        public double? Importe { get; set; }
    }

    public class ReportWorkOrder
    {
        public long id { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public string ProductServiceDesc { get; set; } 
        public string ProfesionalDesc { get; set; }
        public string StatusDesc { get; set; }
        public double? Cantidad { get; set; }
        public string Paciente { get; set; }
        public string Cliente { get; set; }
        public string UnidadMedida { get; set; }
        public string ObraSocial { get; set; }
        public double? Importe { get; set; }
        public double ValorUnitario { get; set; }
        public double? CostoUniProf { get; set; }
        public double? CostoTotalProf { get; set; }
        public override string ToString()
        {
            return $"{id};{Fecha};{Paciente};{ObraSocial};{Cliente};{ProfesionalDesc};{ProductServiceDesc};{UnidadMedida};{Cantidad};{Descripcion};{ValorUnitario};{CostoUniProf};{Importe};{CostoTotalProf};{StatusDesc};";
        }
    }
    #endregion

    #region Compra
    public class ListCompra
    {
        public long id { get; set; }

        public string Proveedor { get; set; }

        public string TipoComprobante { get; set; }

        public string NroFactura { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaEmision { get; set; }

        public double Importe { get; set; }

        public string EstadoDesc { get; set; }
    }

    public class ReportCompra
    {
        public string Proveedor { get; set; }

        public string TipoComprobante { get; set; }

        public string NroFactura { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaEmision { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaContable { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? PrimerVencimiento { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? SdoVencimiento { get; set; }

        public double Importe { get; set; }

        public double? DescuentoGlobal { get; set; }

        public override string ToString()
        {
            return $"{Proveedor};{TipoComprobante};{NroFactura};{FechaEmision};{FechaContable};{PrimerVencimiento};{SdoVencimiento};{Importe};{DescuentoGlobal}";
        }
    }
    #endregion

    #region ListPermisos
    public class ListPermisos
    {
        public Byte id { get; set; } 
        public string pagina { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaAsignacion { get; set; }
        public string AsignadoPor { get; set; }

     }
    #endregion

    #region ListPaginas
    public class ListPaginas
    { 
      public byte id { get; set; }
      public string pagina { get; set; }
    }

    public class ListMenu
    {
        public byte id { get; set; }
        public byte pagina_id { get; set; }
        public string link_text { get; set; }
        public string action_name { get; set; }
        public string controller_name { get; set; }
        public string link_menu { get; set; }
        public string class_menu { get; set; }
        public byte orden_menu { get; set; }
    }
    #endregion

    #region Factura Proveedores
    public class ListFacturaProveedores
    {
        public long id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FechaFactura { get; set; }
        public string Proveedor { get; set; }
        public string User { get; set; }
        public string FileName { get; set; }
        public string EstadoDesc { get; set; }
    }
    public class ReportFacturaProveedores
    {
        public long id { get; set; }
        public string Periodo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FechaPago { get; set; }
        public string TipoFactura { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FechaFactura { get; set; }
        public string TipoComprobante { get; set; }
        public string Proveedor { get; set; }
        public string PuntoVenta { get; set; }
        public string NroComprobante { get; set; }
        public string CuitCuil { get; set; }
        public string NroCuitCuil { get; set; }
        public string Description { get; set; }
        public double? ImpNetoGravado { get; set; }
        public double? ImpNetoNoGravado { get; set; }
        public double? ImpOpExentas { get; set; }
        public double? Iva { get; set; }
        public double ImporteTotal { get; set; }
        public string EstadoDesc { get; set; }
        public string UserCreate { get; set; }

        public override string ToString()
        {
            return $"{id};{Periodo};{FechaPago};{TipoFactura};{FechaFactura};{TipoComprobante};{Proveedor};{PuntoVenta};{NroComprobante};{CuitCuil};{NroCuitCuil};{Description};{ImpNetoGravado};{ImpNetoNoGravado};{ImpOpExentas};{Iva};{ImporteTotal};{EstadoDesc};{UserCreate};";
        }
    }
    #endregion

    #region Orden de Pago
    public class ListOrdenPagoCab
    {
        public long id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FechaOrden { get; set; }
        public string Profesional { get; set; }
        public double Importe { get; set; }
    }
    public class ListOrdenPagoDet
    {
        public long id { get; set; }
        public double Importe { get; set; }
        public string PagaTotal { get; set; }
        public string FormaPago { get; set; }
        public long factura_proveedor_id { get; set; }
        public string NroCheque{ get; set; }
        public string NroCtaCte { get; set; }
        public string Banco { get; set; }
        public string Observaciones { get; set; }
    }
    public class OrdenPagoDetImporte
    {
        public long id { get; set; }
        public double saldo { get; set; }
    }
    public class ReportOrdenPago
    {
        public long id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }
        public string Profesional { get; set; }
        public double Importe { get; set; }

        public override string ToString()
        {
            return $"{id};{Fecha};{Profesional};{Importe}";
        }
    }
    #endregion

    #region cobro

    public class ListCobros
    {
        public long id { get; set; }
        public long nroRecibo { get; set; }
        public string nroFactura { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime fechaFactura { get; set; }
        public double monto { get; set; }
        public double cobroParcial { get; set; }
        public double subtotalRecibo { get; set; }
        public double total { get; set; }
      
    }

    public class ReportCobros
    {
        public long nroRecibo { get; set; }
        public string cliente { get; set; }
        public string nroFactura { get; set; }
        public DateTime fechaFactura { get; set; }
        public double monto { get; set; }
        public double cobroParcial { get; set; }
        public double subtotalRecibo { get; set; }
        public double total { get; set; }

        public override string ToString()
        {
            return $"{nroRecibo};{cliente};{nroFactura};{fechaFactura};{monto};{cobroParcial};{subtotalRecibo};{total}";
        }
    }
    #endregion
}