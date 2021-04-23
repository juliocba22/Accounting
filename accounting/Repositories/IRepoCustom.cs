using accounting.Infra;
using accounting.Models;
using accounting.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace accounting.Repositories
{
    public interface IRepoCustom
    {
        #region --[USUARIO]--

        /// <summary>
        /// New User.
        /// </summary>
        /// <param name="carrier">Datos del nuevo user</param>
        /// <returns></returns>
        users UserAdd(users user);

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="user">Datos del usuario</param>
        void UserUpdate(users user);

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="user"></param>
        void UserDelete(users user);
       
        /// <summary>
        /// Find User
        /// </summary>
        /// <param name="id"></param>
        users UserFind(long id);
        /// <summary>
        /// Get user -> user pass 
        /// </summary>
        /// <param name="user">Nombre de usuario</param>
        /// <param name="pass">Contraseña</param>
        /// <param name="active">Estado del usuario</param>
        /// <returns></returns>
        users UserGet(string user, string pass);

        /// <summary>
        /// validate user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string UserValidate(string user);

        /// <summary>
        /// Get user ->id
        /// </summary>
        /// <param name="id">PK del usuario</param>
        /// <returns></returns>
        users UserGet(long id);



        IEnumerable<ListUsers> UserList(string name);// (string searchValue, int recordsTotal, int skip, int pageSize);

        #endregion --[USUARIO]--

        #region --[ROL]--

        /// <summary>
        /// Obtiene un rol por su PK.
        /// </summary>
        /// <param name="rol_id">Identificador</param>
        /// <returns></returns>
        rol RolGet(int rol_id);


        ///<summary>
        ///Obtiene listado de rol por id 
        ///</summary>
        ///<param name="rol_id"></param>
        IEnumerable<ListRol> RolGetById(int rol_id);

        /// <summary>
        /// Obtiene todos los roles.
        /// </summary>
        /// <returns></returns>
        IEnumerable<rol> RolAll();

        /// <summary>
        /// Crea un nuevo rol.
        /// </summary>
        /// <param name="rol">Datos del nuevo rol</param>
        /// <returns></returns>
        rol RolAdd(rol rol);

        /// <summary>
        /// Actualiza y guarda un rol.
        /// </summary>
        /// <param name="rol">Datos del rol</param>
        void RolUpdate(rol rol);
        #endregion --[ROL]--

        #region --[GASTOS]-- 
        /// <summary>
        /// New expense.
        /// </summary>
        /// <param name="expense">Datos del nuevo gasto</param>
        /// <returns></returns>
        expense ExpenseAdd(expense exp);


        /// <summary>
        /// Update expense
        /// </summary>
        /// <param name="expense"></param>
        void ExpenseUpdate(expense exp);

        /// <summary>
        /// Delete expense
        /// </summary>
        /// <param name="expense"></param>
        void ExpenseDelete(expense exp);

        expense ExpenseFind(long id);

        /// <summary>
        /// Get expense ->id
        /// </summary>
        /// <param name="id">PK del user</param>
        /// <returns></returns>
        expense ExpenseGet(long id);

        IEnumerable<ListExpense> ExpenseList(string expense_type);
        #endregion --[GASTOS]--

        #region --[TIPO GASTO]--

        /// <summary>
        /// Obtiene un tipo gasto por su PK.
        /// </summary>
        /// <param name="expense_id">Identificador</param>
        /// <returns></returns>
        expense_type ExpenseTypeGet(int expense_id);

        ///<summary>
        ///Obtiene listado de tipo de gasto por id 
        ///</summary>
        ///<param name="expense_id"></param>
        IEnumerable<ListExpenseType> ExpenseTypeGetById(int expense_id);


        /// <summary>
        /// Obtiene todos los tipos de gastos
        /// </summary>
        /// <returns></returns>
        IEnumerable<expense_type> ExpenseTypeAll();

        /// <summary>
        /// Crea un nuevo tipo de gasto
        /// </summary>
        /// <param name="expense">Datos del nuevo tipo de gasto</param>
        /// <returns></returns>
        expense_type ExpenseTypeAdd(expense_type exp_type);

        /// <summary>
        /// Actualiza y guarda un tipo de dato.
        /// </summary>
        /// <param name="rol">Datos del rol</param>
        void ExpenseTypeUpdate(expense_type exp_type);


        #endregion --[TIPO GASTO]--

        #region --[OBRA SOCIAL]--

        /// <summary>
        /// New social work.
        /// </summary>
        /// <param name="expense">Datos del nueva obra social</param>
        /// <returns></returns>
        social_work SocialWorkAdd(social_work sw);


        /// <summary>
        /// Update social work
        /// </summary>
        /// <param name="sw"></param>
        void SocialWorkUpdate(social_work sw);

        void SocialWorkDelete(social_work sw);

        /// <summary>
        /// Find social work
        /// </summary>
        /// <param name="id"></param>
        social_work SocialWorkFind(long id);

        /// <summary>
        /// Get social work ->id
        /// </summary>
        /// <param name="id">PK del socialwork</param>
        /// <returns></returns>
        social_work SocialWorkGet(long id);


        IEnumerable<ListSocialWork> SocialWorkList(string name);

        #endregion --[GASTOS]--

        #region --[CLIENTE]--
        IEnumerable<ListClient> ClientList(string razonSocial);
        List<ReportClient> ClientReport(string razonSocial);
       
        #endregion

        #region [--PRODUCTSERVICE--]
        IEnumerable<ListProductService> ProductServiceList(string nombre);
        IEnumerable<ReportProductService> ProductServiceReport(string nombre);
        #endregion

        #region --[PROFESIONAL]
        ProfesionalVM GetDetalleProfesional(int? id);
        
        IEnumerable<ListProfesional> ProfesionalList(string nombre);
        IEnumerable<ReportProfesional> ProfesionalReport(string nombre);
        #endregion

        #region --[WORKORDER] 
        WorkOrderVM GetDetalleWorkOrder(long? id);
        WorkOrderVM GetDeleteWorkOrder(long? id);
        IEnumerable<ListWorkOrder> WorkOrderList(int? status);
        IEnumerable<ReportWorkOrder> WorkOrderReport(int? status);
        WorkOrderDeleteVM GetDelete(long? id);
        #endregion
    }
}
