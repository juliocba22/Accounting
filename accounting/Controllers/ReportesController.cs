using accounting.Models;
using accounting.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace accounting.Controllers
{
    [Authorize]
    [HandleError(View = "Error")]
    public class ReportesController : Controller
    {
        private AccountingEntities1 db = new AccountingEntities1();
        private IRepoCustom _repo = new RepoCustom();
        int _pageSize = 50;

        public ActionResult Index()
        {
            return View();
        }

        #region Cash Flow
        public ActionResult CashFlow()
        {
            return View();
        }
        #endregion

        #region Cuentas a Pagar
        public ActionResult CuentasAPagar()
        {
            return View();
        }
        public void CuentasAPagarPDF()
        {
            
        }
        #endregion

        #region Cuentas Por Cobrar
        public ActionResult CuentasPorCobrar()
        {
            return View();
        }
        #endregion

        #region Saldo de Clientes
        public ActionResult SaldoClientes()
        {
            return View();
        }
        #endregion

        #region Saldo Facturas Profesionales
        public ActionResult SaldoFacturasProfesionales()
        {
            return View();
        }
        #endregion
    }
}