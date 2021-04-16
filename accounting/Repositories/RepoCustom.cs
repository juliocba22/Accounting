﻿
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.Entity;
using accounting.Models;
using accounting.Infra;

namespace accounting.Repositories
{
    public class RepoCustom:IRepoCustom
    {

        #region --[GLOBAL]--
        public string Cnn { get; set; }
        IRepoGeneric<users> _repoUser;
        IRepoGeneric<rol> _repoRol;
        IRepoGeneric<expense> _repoExpense;
        IRepoGeneric<expense_type> _repoExpenseType;
        IRepoGeneric<social_work> _repoSocialWork;

        #endregion --[GLOBAL]--

        #region --[CONSTRUCTOR]--

        public RepoCustom()
        {
           // Cnn = ConfigurationManager.ConnectionStrings["strCnn"].ConnectionString;
            DbContext context = new AccountingEntities();
            _repoUser = new RepoGeneric<users>(context);
            _repoRol = new RepoGeneric<rol>(context);
            _repoExpense = new RepoGeneric<expense>(context);
            _repoExpenseType = new RepoGeneric<expense_type>(context);
            _repoSocialWork = new RepoGeneric<social_work>(context);
        }

        #endregion --[CONSTRUCTOR]--

        #region --[USUARIO]--

        /// <summary>
        /// New User.
        /// </summary>
        /// <param name="carrier">Datos del nuevo user</param>
        /// <returns></returns>
        public users UserAdd(users user)
        {
            _repoUser.Add(user);
            _repoUser.SaveChanges();

            return user;
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="user"></param>
        public void UserUpdate(users user)
        {
            _repoUser.Update(user);
            _repoUser.SaveChanges();
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="user"></param>
        public void UserDelete(users user)
        {
            _repoUser.Remove(user);
            _repoUser.SaveChanges();
        }

        /// <summary>
        /// Find User
        /// </summary>
        /// <param name="id"></param>
        public users UserFind(long id)
        {

           return _repoUser.Find(x=> (x.id == id));
        }

        /// <summary>
        /// Get user -> user pass 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public users UserGet(string user,string pass)
        {
            return _repoUser.Find(x => (x.user_name == user &&  x.password == pass));
        }

        /// <summary>
        /// validate user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string UserValidate(string user)
        {
            var username = "";

            using (AccountingEntities ctx = new AccountingEntities())
            {
                var query = ctx.users.Where(c => c.user_name == user).FirstOrDefault();

                if (query!=null)
                    username = query.user_name;
            }

            return username;
        }

        /// <summary>
        /// Get user ->id
        /// </summary>
        /// <param name="id">PK del user</param>
        /// <returns></returns>
        public users UserGet(long id)
        {
            return _repoUser.Get(id);
        }

        //public users UserGet(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public void UserUpdate(users user)
        //{
        //    throw new NotImplementedException();
        //}

        public IEnumerable<ListUsers> UserList(string name)//(string searchValue,int recordsTotal,int skip,int pageSize)
        {
            List<ListUsers> list = new List<ListUsers>();

            //if (string.IsNullOrEmpty(name))
            //    return Enumerable.Empty<ListUsers>();

            using (AccountingEntities ctx = new AccountingEntities())
            {
                return (from u in ctx.users
                        join r in ctx.rol on u.rol_id equals r.id
                        where (string.IsNullOrEmpty(name) || u.name.Contains(name))
                        select new ListUsers
                        {
                            id= u.id,
                            name = u.name,
                            rol_id = u.rol_id,
                            rol_description = r.description,
                            user_name = u.user_name,
                            password = u.password,
                            active = u.active
                        }).ToList();

                //IQueryable<ListUsers> query = (from u in ctx.users
                //                               join r in ctx.rol on u.rol_id equals r.id
                //                               select new ListUsers
                //                               {
                //                                   name = u.name,
                //                                   rol_id = u.rol_id,
                //                                   rol_description = r.description,
                //                                   user_name = u.user_name,
                //                                   password = u.password,
                //                                   active = u.active
                //                               });

                //if (searchValue != "")
                //{
                //    query = query.Where(d => d.user_name.Contains(searchValue) || d.user_name.Contains(searchValue));
                //}
                //recordsTotal = query.Count();
                //if (!(string.IsNullOrEmpty("1") && string.IsNullOrEmpty("1")))
                //{
                //    query = query.OrderBy(d => d.user_name);
                //}

                //list = query.Skip(skip).Take(pageSize).ToList();
                
                //return list;
            }
        }


        //public IEnumerable<ListExpense> ExpenseList(string expense_type)
        //{
        //    List<ListExpense> list = new List<ListExpense>();

        //    using (AccountingEntities ctx = new AccountingEntities())
        //    {
        //        return (from e in ctx.expense
        //                join t in ctx.expense_type on e.expense_id equals t.id
        //                where (string.IsNullOrEmpty(expense_type) || t.description.Contains(expense_type))
        //                select new ListExpense
        //                {
        //                    id = e.id,
        //                    name = e.name,
        //                    description=e.description,
        //                    expense_id=e.expense_id,
        //                    expense_description=t.description
        //                }).ToList();
        //    }
        //}
        #endregion --[USUARIO]--


        #region --[ROL]--

        /// <summary>
        /// Obtiene un rol por su PK.
        /// </summary>
        /// <param name="rol_id">Identificador</param>
        /// <returns></returns>
        public rol RolGet(int rol_id)
        {
            return _repoRol.Get(rol_id);
        }

        ///<summary>
        ///Obtiene listado de rol por id 
        ///</summary>
        ///<param name="rol_id"></param>
        public IEnumerable<ListRol> RolGetById(int rol_id)
        {

            using (AccountingEntities ctx = new AccountingEntities())
            {
                return (from c in ctx.rol
                        where (c.id == rol_id)
                        select new ListRol
                        {
                            id = c.id,
                            description = c.description

                        }).Distinct().ToList();
            }
        }

        /// <summary>
        /// Obtiene todos los roles.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<rol> RolAll()
        {
            return _repoRol.GetAll();
        }

        /// <summary>
        /// Crea un nuevo rol.
        /// </summary>
        /// <param name="rol">Datos del nuevo rol</param>
        /// <returns></returns>
        public rol RolAdd(rol rol)
        {
            _repoRol.Add(rol);
            _repoRol.SaveChanges();

            return rol;
        }

        /// <summary>
        /// Actualiza y guarda un rol.
        /// </summary>
        /// <param name="rol">Datos del rol</param>
        public void RolUpdate(rol rol)
        {
            _repoRol.Update(rol);
            _repoRol.SaveChanges();
        }

        #endregion --[ROL]--

        #region --[GASTOS]--

        /// <summary>
        /// New expense.
        /// </summary>
        /// <param name="expense">Datos del nuevo gasto</param>
        /// <returns></returns>
        public expense ExpenseAdd(expense exp)
        {
            _repoExpense.Add(exp);
            _repoExpense.SaveChanges();

            return exp;
        }

        /// <summary>
        /// Update expense
        /// </summary>
        /// <param name="expense"></param>
        public void ExpenseUpdate(expense exp)
        {
            _repoExpense.Update(exp);
            _repoExpense.SaveChanges();
        }

        /// <summary>
        /// Delete expense
        /// </summary>
        /// <param name="expense"></param>
        public void ExpenseDelete(expense exp)
        {
            _repoExpense.Remove(exp);
            _repoExpense.SaveChanges();
        }

        /// <summary>
        /// Find expense
        /// </summary>
        /// <param name="id"></param>
        public expense ExpenseFind(long id)
        {

            return _repoExpense.Find(x => (x.id == id));
        }

        /// <summary>
        /// Get expense ->id
        /// </summary>
        /// <param name="id">PK del user</param>
        /// <returns></returns>
        public expense ExpenseGet(long id)
        {
            return _repoExpense.Get(id);
        }

        public IEnumerable<ListExpense> ExpenseList(string expense_type)
        {
            List<ListExpense> list = new List<ListExpense>();

            using (AccountingEntities ctx = new AccountingEntities())
            {
                return (from e in ctx.expense
                        join t in ctx.expense_type on e.expense_id equals t.id
                        where (string.IsNullOrEmpty(expense_type) || t.description.Contains(expense_type))
                        select new ListExpense
                        {
                            id = e.id,
                            name = e.name,
                            description = e.description,
                            expense_id = e.expense_id,
                            expense_description = t.description,
                            date_expense= (DateTime)e.date_expense,
                        }).ToList();
            }
        }
        #endregion --[GASTOS]--


        #region --[TIPO GASTO]--

        /// <summary>
        /// Obtiene un tipo gasto por su PK.
        /// </summary>
        /// <param name="expense_id">Identificador</param>
        /// <returns></returns>
        public expense_type ExpenseTypeGet(int expense_id)
        {
            return _repoExpenseType.Get(expense_id);
        }

        ///<summary>
        ///Obtiene listado de tipo de gasto por id 
        ///</summary>
        ///<param name="expense_id"></param>
        public IEnumerable<ListExpenseType> ExpenseTypeGetById(int expense_id)
        {

            using (AccountingEntities ctx = new AccountingEntities())
            {
                return (from c in ctx.expense_type
                        where (c.id == expense_id)
                        select new ListExpenseType
                        {
                            id = c.id,
                            description = c.description

                        }).Distinct().ToList();
            }
        }

        /// <summary>
        /// Obtiene todos los tipos de gastos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<expense_type> ExpenseTypeAll()
        {
            return _repoExpenseType.GetAll();
        }

        /// <summary>
        /// Crea un nuevo tipo de gasto
        /// </summary>
        /// <param name="expense">Datos del nuevo tipo de gasto</param>
        /// <returns></returns>
        public expense_type ExpenseTypeAdd(expense_type exp_type)
        {
            _repoExpenseType.Add(exp_type);
            _repoExpenseType.SaveChanges();

            return exp_type;
        }

        /// <summary>
        /// Actualiza y guarda un tipo de dato.
        /// </summary>
        /// <param name="rol">Datos del rol</param>
        public void ExpenseTypeUpdate(expense_type exp_type)
        {
            _repoExpenseType.Update(exp_type);
            _repoExpenseType.SaveChanges();
        }

        #endregion --[TIPO GASTO]--


        #region --[OBRA SOCIAL]--

        /// <summary>
        /// New social work.
        /// </summary>
        /// <param name="expense">Datos del nueva obra social</param>
        /// <returns></returns>
        public social_work SocialWorkAdd(social_work sw)
        {
            _repoSocialWork.Add(sw);
            _repoSocialWork.SaveChanges();

            return sw;
        }

        /// <summary>
        /// Update social work
        /// </summary>
        /// <param name="sw"></param>
        public void SocialWorkUpdate(social_work sw)
        {
            _repoSocialWork.Update(sw);
            _repoSocialWork.SaveChanges();
        }

        /// <summary>
        /// Delete social work
        /// </summary>
        /// <param name="sw"></param>
        public void SocialWorkDelete(social_work sw)
        {
            _repoSocialWork.Remove(sw);
            _repoSocialWork.SaveChanges();
        }

        /// <summary>
        /// Find social work
        /// </summary>
        /// <param name="id"></param>
        public social_work SocialWorkFind(long id)
        {

            return _repoSocialWork.Find(x => (x.id == id));
        }

        /// <summary>
        /// Get social work ->id
        /// </summary>
        /// <param name="id">PK del socialwork</param>
        /// <returns></returns>
        public social_work SocialWorkGet(long id)
        {
            return _repoSocialWork.Get(id);
        }

        public IEnumerable<ListSocialWork> SocialWorkList(string name)
        {
            using (AccountingEntities ctx = new AccountingEntities())
            {
                return (from s in ctx.social_work
                        //join t in ctx.expense_type on e.expense_id equals t.id
                        where (string.IsNullOrEmpty(name) || s.name.Contains(name))
                        select new ListSocialWork
                        {
                            id = s.id,
                            name = s.name,
                            description = s.description,
                            phone = s.phone,
                            mail = s.mail,
                        }).ToList();
            }
        }
        #endregion --[GASTOS]--


    }
}