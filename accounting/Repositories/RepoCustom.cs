using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using accounting.Models;
using accounting.Infra;
using accounting.ViewModels;
using System.Data;

namespace accounting.Repositories
{
    public class RepoCustom : IRepoCustom
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
            DbContext context = new AccountingEntities1();
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

            return _repoUser.Find(x => (x.id == id));
        }

        /// <summary>
        /// Get user -> user pass 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public users UserGet(string user, string pass)
        {
            return _repoUser.Find(x => (x.user_name == user && x.password == pass));
        }

        /// <summary>
        /// validate user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string UserValidate(string user)
        {
            var username = "";

            using (AccountingEntities1 ctx = new AccountingEntities1())
            {
                var query = ctx.users.Where(c => c.user_name == user).FirstOrDefault();

                if (query != null)
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

            using (AccountingEntities1 ctx = new AccountingEntities1())
            {
                return (from u in ctx.users
                        join r in ctx.rol on u.rol_id equals r.id
                        where (string.IsNullOrEmpty(name) || u.name.Contains(name))
                        select new ListUsers
                        {
                            id = u.id,
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

        public IEnumerable<ListRol> RolList(string descripcion)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from r in ctx.rol
                            where (string.IsNullOrEmpty(descripcion) || r.description.Contains(descripcion))
                             && r.activo == 1
                            select new ListRol
                            {
                                id = r.id,
                                description = r.description
                            }).ToList();
                }
            }
            catch
            { return null; }
        }

        ///<summary>
        ///Obtiene listado de rol por id 
        ///</summary>
        ///<param name="rol_id"></param>
        public IEnumerable<ListRol> RolGetById(int rol_id)
        {

            using (AccountingEntities1 ctx = new AccountingEntities1())
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

        public ExpenseCreateVM GetExpenseDetail(long id)
        {
            using (AccountingEntities1 ctx = new AccountingEntities1())
            {
                return (from e in ctx.expense
                        join t in ctx.expense_type on e.expense_id equals t.id
                        join tc in ctx.tipo_comprobante on e.tipo_comprobante_id equals tc.id
                        join u in ctx.users on e.update_user_id equals u.id
                        join p in ctx.proveedor on e.proveedor_id equals p.id into temp
                        from temp1 in temp.DefaultIfEmpty()
                        where e.id == id
                          && e.activo == 1
                        select new ExpenseCreateVM
                        {
                            id = e.id,
                            name = e.name,
                            tipo_comprobante = tc.descripcion,
                            selling_point = e.selling_point,
                            nro_comprobante = e.nro_comprobante,
                            cuit_cuil = e.cuit_cuil,
                            nro_cuit_cuil = e.nro_cuit_cuil,
                            denominacion_emisor = e.denominacion_emisor,
                            imp_neto_gravado = e.imp_neto_gravado,
                            imp_neto_no_gravado = e.imp_neto_no_gravado,
                            imp_op_exentas = e.imp_op_exentas,
                            iva = e.iva,
                            importe_total = e.importe_total,
                            description = e.description,
                            expense_name = t.description,
                            date_expense = e.date_expense,
                            amount_money = e.amount_money,
                            user = u.user_name,
                            proveedor = temp1.razon_social,
                            periodo = e.periodo,
                            pay_date = e.pay_date,
                            pay_state = e.pay_state
                        }).First();
            }
        }

        public IEnumerable<ReportExpense> ExpenseReport(string expense_type)
        {
            List<ReportExpense> list = new List<ReportExpense>();

            using (AccountingEntities1 ctx = new AccountingEntities1())
            {
                return (from e in ctx.expense
                        join t in ctx.expense_type on e.expense_id equals t.id
                        join tc in ctx.tipo_comprobante on e.tipo_comprobante_id equals tc.id
                        join p in ctx.proveedor on e.proveedor_id equals p.id into temp
                        from temp1 in temp.DefaultIfEmpty()
                        where (string.IsNullOrEmpty(expense_type) || t.description.Contains(expense_type))
                          && e.activo == 1
                        select new ReportExpense
                        {
                            name = e.name,
                            tipo_comprobante = tc.descripcion,
                            selling_point = e.selling_point,
                            nro_comprobante = e.nro_comprobante,
                            cuit_cuil = e.cuit_cuil,
                            nro_cuit_cuil = e.nro_cuit_cuil,
                            denominacion_emisor = e.denominacion_emisor,
                            imp_neto_gravado = e.imp_neto_gravado,
                            imp_neto_no_gravado = e.imp_neto_no_gravado,
                            imp_op_exentas = e.imp_op_exentas,
                            iva = e.iva,
                            importe_total = e.importe_total,
                            description = e.description,
                            expense_name = t.description,
                            date_expense = e.date_expense,
                            proveedor = temp1.razon_social
                        }).ToList();
            }
        }

        public List<ExpensesExportPDFVM> GetExpenses(long? id)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from e in ctx.expense
                            join t in ctx.expense_type on e.expense_id equals t.id
                            join tc in ctx.tipo_comprobante on e.tipo_comprobante_id equals tc.id
                            join p in ctx.proveedor on e.proveedor_id equals p.id into temp
                            from temp1 in temp.DefaultIfEmpty()
                            where e.id == id 
                              && e.activo == 1
                            select new ExpensesExportPDFVM
                            {
                                name = e.name,
                                tipo_comprobante = tc.descripcion,
                                selling_point = e.selling_point,
                                nro_comprobante = e.nro_comprobante,
                                cuit_cuil = e.cuit_cuil,
                                nro_cuit_cuil = e.nro_cuit_cuil,
                                denominacion_emisor = e.denominacion_emisor,
                                imp_neto_gravado = e.imp_neto_gravado,
                                imp_neto_no_gravado = e.imp_neto_no_gravado,
                                imp_op_exentas = e.imp_op_exentas,
                                iva = e.iva,
                                importe_total = e.importe_total,
                                description = e.description,
                                expense_name = t.description,
                                date_expense = e.date_expense,
                                proveedor = temp1.razon_social
                            }).ToList();
                }
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<ListExpense> ExpenseList(string expense_type)
        {
            List<ListExpense> list = new List<ListExpense>();

            using (AccountingEntities1 ctx = new AccountingEntities1())
            {
                return (from e in ctx.expense
                        join t in ctx.expense_type on e.expense_id equals t.id
                        join u in ctx.users on e.update_user_id equals u.id
                        where (string.IsNullOrEmpty(expense_type) || t.description.Contains(expense_type))
                          && e.activo == 1
                        select new ListExpense
                        {
                            id = e.id,
                            name = e.name,
                            description = e.description,
                            expense_id = e.expense_id,
                            expense_description = t.description,
                            date_expense = (DateTime)e.date_expense,
                            image = e.image,
                            path_file = e.path_url,
                            name_file = e.name_file,
                            amount_money = e.amount_money,
                            user = u.user_name
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

            using (AccountingEntities1 ctx = new AccountingEntities1())
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
            using (AccountingEntities1 ctx = new AccountingEntities1())
            {
                return (from s in ctx.social_work
                            //join t in ctx.expense_type on e.expense_id equals t.id
                        where (string.IsNullOrEmpty(name) || s.name.Contains(name))
                        && s.activo == 1
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

        #region --[CLIENTE]--
        public IEnumerable<ListClient> ClientList(string razonSocial)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from c in ctx.client
                            where (string.IsNullOrEmpty(razonSocial) || c.razonSocial.Contains(razonSocial))
                            && c.activo == 1
                            select new ListClient
                            {
                                id = c.id,
                                razonSocial = c.razonSocial,
                                localidad = c.localidad,
                                telefono = c.telefono,
                                email = c.email
                            }).ToList();
                }
            }
            catch
            { return null; }
        }

        public List<ReportClient> ClientReport(string razonSocial)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from c in ctx.client
                            where (string.IsNullOrEmpty(razonSocial) || c.razonSocial.Contains(razonSocial))
                            && c.activo == 1
                            select new ReportClient
                            {
                                id = c.id,
                                razonSocial = c.razonSocial,
                                localidad = c.localidad,
                                provincia = c.provincia,
                                nombreContacto = c.personeria,
                                telefono = c.telefono,
                                email = c.email,
                                emailFacturacion = c.emailFacturacon,
                                codigo = c.codigo,
                                nroCodigo = c.nro_codigo

                            }).ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region --[PRODUCTSERVICE]--
        public IEnumerable<ListProductService> ProductServiceList(string nombre)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from ps in ctx.product_service
                            join c in ctx.client on ps.client_id equals c.id
                            where (string.IsNullOrEmpty(nombre) || ps.nombre.Contains(nombre))
                            && ps.activo == 1
                            select new ListProductService
                            {
                                id = ps.id,
                                nombre = ps.nombre,
                                tipo = ps.tipo == 0 ? "Producto" : "Servicio",
                                valorUnitario = ps.valorUnitario,
                                cliente = c.razonSocial
                            }).ToList();
                }
            }
            catch
            { return null; }
        }
        public IEnumerable<ReportProductService> ProductServiceReport(string nombre)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from ps in ctx.product_service
                            join c in ctx.client on ps.client_id equals c.id
                            where (string.IsNullOrEmpty(nombre) || ps.nombre.Contains(nombre))
                            && ps.activo == 1
                            select new ReportProductService
                            {
                                id = ps.id,
                                nombre = ps.nombre,
                                tipo = ps.tipo == 0 ? "Producto" : "Servicio",
                                valorUnitario = ps.valorUnitario,
                                costoProfesional = ps.costo_profesional,
                                unidadMedida = ps.unidad_medida == 0 ? "Valor unitario por sesión" : ps.unidad_medida == 1 ? "Valor unitario por hora" : ps.unidad_medida == 2 ? "Valor unitario por visita" : "Valor unitario por Km",
                                cliente = c.razonSocial
                            }).ToList();
                }
            }
            catch
            { return null; }
        }
        public ProductServiceVM GetProductServiceDetails(int? id)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from ps in ctx.product_service
                            join c in ctx.client on ps.client_id equals c.id
                            where ps.id == id
                            && ps.activo == 1
                            select new ProductServiceVM
                            {
                                id = ps.id,
                                nombre = ps.nombre,
                                tipo = ps.tipo,
                                valorUnitario = ps.valorUnitario,
                                unidadMedida = ps.unidad_medida,
                                costoProfesional = ps.costo_profesional,
                                Cliente = c.razonSocial
                            }).First();
                }
            }
            catch
            { return null; }
        }
        #endregion

        #region --[PROFESIONAL]
        public ProfesionalVM GetDetalleProfesional(int? id)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from pr in ctx.profesional
                            join ps in ctx.product_service on pr.product_service_id equals ps.id
                            where pr.id == id
                            select new ProfesionalVM
                            {
                                id = pr.id,
                                product_service_id = pr.product_service_id,
                                nombre = pr.nombre,
                                domicilio = pr.domicilio,
                                cuit = pr.cuit,
                                cuitNro = pr.nro_cuit,
                                matricula = pr.matricula,
                                localidad = pr.localidad,
                                provincia = pr.provincia,
                                telefono = pr.telefono,
                                email = pr.email,
                                servicioDesc = ps.nombre,
                                tipoFacturacionDesc = pr.tipo_facturacion == 0 ? "Factura" : pr.tipo_facturacion == 1 ? "Recibo" : "Voluntariado",
                                cbu = pr.cbu,
                                banco = pr.banco,
                                nroCuenta = pr.nro_cuenta,
                                alias = pr.alias
                            }).First();
                }
            }
            catch
            { return null; }
        }
        public IEnumerable<ListProfesional> ProfesionalList(string nombre)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from pr in ctx.profesional
                            join ps in ctx.product_service on pr.product_service_id equals ps.id
                            where (string.IsNullOrEmpty(nombre) || pr.nombre.Contains(nombre))
                            && pr.activo == 1
                            select new ListProfesional
                            {
                                id = pr.id,
                                nombre = pr.nombre,
                                servicio = ps.nombre,
                                telefono = pr.telefono,
                                email = pr.email
                            }).ToList();
                }
            }
            catch
            { return null; }
        }
        public IEnumerable<ReportProfesional> ProfesionalReport(string nombre)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from pr in ctx.profesional
                            join ps in ctx.product_service on pr.product_service_id equals ps.id
                            where (string.IsNullOrEmpty(nombre) || pr.nombre.Contains(nombre))
                            && pr.activo == 1
                            select new ReportProfesional
                            {
                                nombre = pr.nombre,
                                servicio = ps.nombre,
                                domicilio = pr.domicilio,
                                cuit = pr.cuit,
                                matricula = pr.matricula,
                                localidad = pr.localidad,
                                provincia = pr.provincia,
                                telefono = pr.telefono,
                                email = pr.email,
                                cuitNro = pr.nro_cuit,
                                tipoFacturacion = pr.tipo_facturacion == 0 ? "Factura" : pr.tipo_facturacion == 1 ? "Recibo" : "Voluntariado",
                                cbu = pr.cbu,
                                banco = pr.banco,
                                nroCuenta = pr.nro_cuenta,
                                alias = pr.alias
                            }).ToList();
                }
            }
            catch
            { return null; }
        }
        #endregion

        #region --[PROVEEDOR]--

        public IEnumerable<ListProveedor> ProveedorList(string razonSocial)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from p in ctx.proveedor
                            where (string.IsNullOrEmpty(razonSocial) || p.razon_social.Contains(razonSocial))
                            && p.activo == 1
                            select new ListProveedor
                            {
                                id = p.id,
                                razonSocial = p.razon_social,
                                localidad = p.localidad,
                                telefono = p.telefono,
                                email = p.mail
                            }).ToList();
                }
            }
            catch
            { return null; }
        }

        public List<ReportProveedor> ProveedorReport(string razonSocial)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from p in ctx.proveedor
                            where (string.IsNullOrEmpty(razonSocial) || p.razon_social.Contains(razonSocial))
                            && p.activo == 1
                            select new ReportProveedor
                            {
                                codigo = p.id,
                                cuitNro = p.dni,
                                cuit = p.cuit,
                                nombreFantasia = p.nombre_fantasia,
                                razonSocial = p.razon_social,
                                localidad = p.localidad,
                                provincia = p.provincia,
                                telefono = p.telefono,
                                email = p.mail,
                                emailFacturacion = p.mail_facturacion,
                                contacto = p.personeria,
                                direccion = p.direccion,
                                piso = p.piso_dpto,
                                cp = p.codigo_postal,
                                cbu = p.cbu,
                                banco = p.banco,
                                nroCuenta = p.nro_cuenta,
                                alias = p.alias
                            }).ToList();
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region --[WORKORDER]--
        public WorkOrderVM GetDeleteWorkOrder(long? id)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from wo in ctx.work_order
                            join ps in ctx.product_service on wo.product_service_id equals ps.id
                            join st in ctx.work_order_status on wo.status_id equals st.id
                            where wo.id == id
                            select new WorkOrderVM
                            {
                                id = wo.id,
                                Fecha = wo.fecha,
                                ProductServiceId = ps.id,
                                StatusId = st.id,
                                Descripcion = wo.descripcion,
                                Cantidad = wo.cantidad,
                                ProfesionalId = wo.profesional_id,
                                Paciente = wo.nombre_paciente,

                            }).First();
                }
            }
            catch (Exception ex)
            { return null; }
        }

        public WorkOrderVM GetDetalleWorkOrder(long? id)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from wo in ctx.work_order
                            join ps in ctx.product_service on wo.product_service_id equals ps.id
                            join st in ctx.work_order_status on wo.status_id equals st.id
                            join c in ctx.client on ps.client_id equals c.id
                            join pr in ctx.profesional on wo.profesional_id equals pr.id into temp
                            from temp1 in temp.DefaultIfEmpty()
                            where wo.id == id
                            select new WorkOrderVM
                            {
                                id = wo.id,
                                Fecha = wo.fecha,
                                Descripcion = wo.descripcion,
                                ProductServiceDesc = ps.id.ToString() + "-" + ps.nombre,
                                Cantidad = wo.cantidad,
                                Paciente = wo.nombre_paciente,
                                ProfesionalDesc = temp1.nombre,
                                Importe = wo.importe,
                                StatusDesc = st.descripcion,
                                Cliente = c.razonSocial,
                                CostoProfesional = wo.costo_profesional,
                                ObraSocial = wo.obra_social,
                                UnidadMedida = ps.unidad_medida == 0 ? "Valor unitario por sesión" : ps.unidad_medida == 1 ? "Valor unitario por hora" : ps.unidad_medida == 2 ? "Valor unitario por visita" : ps.unidad_medida == 3 ? "Valor unitario por Km" : "",
                                ValorUnitario = ps.valorUnitario.ToString(),
                                CostoUniProf = ps.costo_profesional.ToString(),
                            }).First();
                }
            }
            catch
            { return null; }
        }
        public IEnumerable<ListWorkOrder> WorkOrderList(int? status)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from wo in ctx.work_order
                            join ps in ctx.product_service on wo.product_service_id equals ps.id
                            join st in ctx.work_order_status on wo.status_id equals st.id
                            where (st.id == status || status == null)
                            select new ListWorkOrder
                            {
                                id = wo.id,
                                Fecha = wo.fecha,
                                ProductServiceDesc = ps.id.ToString() + "-" + ps.nombre,
                                StatusDesc = st.descripcion,
                                Importe = wo.importe
                            }).ToList();
                }
            }
            catch (Exception ex)
            { return null; }
        }
        public IEnumerable<ReportWorkOrder> WorkOrderReport(int? status)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from wo in ctx.work_order
                            join ps in ctx.product_service on wo.product_service_id equals ps.id
                            join st in ctx.work_order_status on wo.status_id equals st.id
                            join c in ctx.client on ps.client_id equals c.id
                            join pr in ctx.profesional on wo.profesional_id equals pr.id into temp2
                            from temp3 in temp2.DefaultIfEmpty()
                            where (st.id == status || status == null)
                            select new ReportWorkOrder
                            {
                                id = wo.id,
                                Fecha = wo.fecha,
                                Descripcion = wo.descripcion,
                                ProductServiceDesc = ps.id.ToString() + "-" + ps.nombre,
                                Cantidad = wo.cantidad,
                                Paciente = wo.nombre_paciente,
                                ProfesionalDesc = temp3.nombre,
                                Importe = wo.importe,
                                StatusDesc = st.descripcion,
                                UnidadMedida = ps.unidad_medida == 0 ? "Valor unitario por sesión" : ps.unidad_medida == 1 ? "Valor unitario por hora" : ps.unidad_medida == 2 ? "Valor unitario por visita" : ps.unidad_medida == 3 ? "Valor unitario por Km" : "",
                                ValorUnitario = ps.valorUnitario,
                                CostoUniProf = ps.costo_profesional,
                                Cliente = c.razonSocial,
                                ObraSocial = wo.obra_social,
                                CostoTotalProf = wo.costo_profesional
                            }).ToList();
                }
            }
            catch (Exception ex)
            { return null; }
        }
        public WorkOrderDeleteVM GetDelete(long? id)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from wo in ctx.work_order
                            join ps in ctx.product_service on wo.product_service_id equals ps.id
                            join st in ctx.work_order_status on wo.status_id equals st.id
                            where wo.id == id
                            select new WorkOrderDeleteVM
                            {
                                id = wo.id,
                                Fecha = wo.fecha,
                                ProductService = ps.id.ToString() + "-" + ps.nombre,
                                Status = st.descripcion
                            }).First();
                }
            }
            catch
            { return null; }
        }
        #endregion

        #region --[COMPRA]--
        public CompraDeleteVM GetDeleteCompra(long? id)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from co in ctx.compra
                            join pr in ctx.proveedor on co.proveedor_id equals pr.id
                            join tp in ctx.tipo_comprobante on co.tipo_comprobante_id equals tp.id
                            where co.id == id
                            select new CompraDeleteVM
                            {
                                id = co.id,
                                Proveedor = pr.razon_social,
                                TipoComprobante = tp.descripcion,
                                NroFactura = co.nro_factura,
                                FechaEmision = co.fecha_emision,
                                Importe = co.importe
                            }).First();
                }
            }
            catch
            { return null; }
        }

        public CompraVM GetDetalleCompra(long? id)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from co in ctx.compra
                            join pr in ctx.proveedor on co.proveedor_id equals pr.id
                            join tp in ctx.tipo_comprobante on co.tipo_comprobante_id equals tp.id
                            where co.id == id
                            select new CompraVM
                            {
                                id = co.id,
                                Proveedor = pr.razon_social,
                                TipoComprobante = tp.descripcion,
                                NroFactura = co.nro_factura,
                                FechaEmision = co.fecha_emision,
                                FechaContable = co.fecha_contable,
                                PrimerVencimiento = co.vencimiento_1,
                                SdoVencimiento = co.vencimiento_2,
                                Importe = co.importe,
                                DescuentoGlobal = co.descuento_global,
                                Estado = co.estado
                            }).First();
                }
            }
            catch
            { return null; }
        }

        public IEnumerable<ListCompra> CompraList(int? proveedor, DateTime? fechaEmisionDesde, DateTime? fechaEmisionHasta)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from co in ctx.compra
                            join pr in ctx.proveedor on co.proveedor_id equals pr.id
                            join tp in ctx.tipo_comprobante on co.tipo_comprobante_id equals tp.id
                            where (co.proveedor_id == proveedor || proveedor == null)
                            && (co.fecha_emision >= fechaEmisionDesde || fechaEmisionDesde == null)
                            && (co.fecha_emision <= fechaEmisionHasta || fechaEmisionHasta == null)
                            && co.activo == 1
                            select new ListCompra
                            {
                                id = co.id,
                                Proveedor = pr.razon_social,
                                TipoComprobante = tp.descripcion,
                                NroFactura = co.nro_factura,
                                FechaEmision = co.fecha_emision,
                                Importe = co.importe,
                                EstadoDesc = co.estado == 0 ? "Pendiente de Pago" : co.estado == 1 ? "Pago incompleto" : "Pagada"
                            }).ToList();
                }
            }
            catch
            { return null; }
        }

        public IEnumerable<ReportCompra> CompraReport(int? proveedor, DateTime? fechaEmisionDesde, DateTime? fechaEmisionHasta)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from co in ctx.compra
                            join pr in ctx.proveedor on co.proveedor_id equals pr.id
                            join tp in ctx.tipo_comprobante on co.tipo_comprobante_id equals tp.id
                            where (co.proveedor_id == proveedor || proveedor == null)
                            && (co.fecha_emision >= fechaEmisionDesde || fechaEmisionDesde == null)
                            && (co.fecha_emision <= fechaEmisionHasta || fechaEmisionHasta == null)
                            && co.activo == 1
                            select new ReportCompra
                            {
                                Proveedor = pr.razon_social,
                                TipoComprobante = tp.descripcion,
                                NroFactura = co.nro_factura,
                                FechaEmision = co.fecha_emision,
                                FechaContable = co.fecha_contable,
                                PrimerVencimiento = co.vencimiento_1,
                                SdoVencimiento = co.vencimiento_2,
                                Importe = co.importe,
                                DescuentoGlobal = co.descuento_global
                            }).ToList();
                }
            }
            catch
            { return null; }
        }

        #endregion

        #region --[PERMISOS]--

        public IEnumerable<ListPermisos> PermisosList(byte rol_id)
        {
            using (AccountingEntities1 ctx = new AccountingEntities1())
            {
                return (from rp in ctx.rolpagina
                        join p in ctx.pagina on rp.pagina_id equals p.id
                        join u in ctx.users on (int)rp.update_user_id equals u.id
                        where rp.rol_id == rol_id && rp.asignada == true
                        select new ListPermisos
                        {
                            id = rp.id,
                            pagina = p.link_text,
                            FechaAsignacion = (DateTime)rp.update_date,
                            AsignadoPor = u.user_name
                        }).ToList();
            }
        }

        public IEnumerable<ListPaginas> PaginasList(byte rol_id)
        {
            using (AccountingEntities1 ctx = new AccountingEntities1())
            {
                return (from p in ctx.pagina
                        let pages = (from rp in ctx.rolpagina
                                     where rp.asignada == true && rp.rol_id == rol_id
                                     select rp.pagina_id)
                        where !pages.Contains(p.id) && p.activo == 1
                        orderby p.id
                        select new ListPaginas
                        {
                            id = p.id,
                            pagina = p.link_text

                        }).ToList();
            }
        }

        public IEnumerable<ListPaginas> PaginasGet(byte rol_id)
        {
            using (AccountingEntities1 ctx = new AccountingEntities1())
            {
                return (from p in ctx.pagina
                        let pages = (from rp in ctx.rolpagina
                                     where rp.asignada == true && rp.rol_id == rol_id
                                     select rp.pagina_id)
                        where !pages.Contains(p.id) && p.activo == 1
                        orderby p.id
                        select new ListPaginas
                        {
                            id = p.id,
                            pagina = p.link_text

                        }).ToList();
            }
        }

        public IEnumerable<ListPaginas> PageName(byte pagina_id)
        {
            using (AccountingEntities1 ctx = new AccountingEntities1())
            {
                return (from p in ctx.pagina
                        where (p.id == pagina_id)
                        select new ListPaginas
                        {
                            id = p.id,
                            pagina = p.link_text,

                        }).Distinct().ToList();
            }
        }

        public IEnumerable<ListRol> RolName(byte rol_id)
        {
            using (AccountingEntities1 ctx = new AccountingEntities1())
            {
                return (from r in ctx.rol
                        where (r.id == rol_id)
                        select new ListRol
                        {
                            id = r.id,
                            description = r.description,

                        }).Distinct().ToList();
            }
        }

        public IEnumerable<ListMenu> GetMenu(byte rol_id)
        {
            using (AccountingEntities1 ctx = new AccountingEntities1())
            {
                return (from rp in ctx.rolpagina
                        join p in ctx.pagina on rp.pagina_id equals p.id
                        where rp.rol_id == rol_id && rp.asignada == true && p.activo == 1
                        select new ListMenu
                        {
                            id = rp.id,
                            pagina_id = rp.pagina_id,
                            link_text = p.link_text,
                            action_name = p.action_name,
                            controller_name = p.controller_name,
                            link_menu = p.link_menu,
                            class_menu = p.class_menu,
                            orden_menu = (byte)p.orden_menu,
                        }).ToList().OrderBy(o => o.orden_menu);
            }
        }

        #endregion

        #region --[FACTURAS PROVEEDORES]
        public FacturaProveedoresVM FacturaProveedorGet(long? id)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from e in ctx.factura_proveedores
                            join tc in ctx.tipo_comprobante on e.tipo_comprobante_id equals tc.id
                            join u in ctx.users on e.create_user_id equals u.id
                            join pr in ctx.profesional on e.profesional_id equals pr.id 
                            where e.id == id
                              && e.activo == 1
                            select new FacturaProveedoresVM
                            {
                                id = e.id,
                                periodo = e.periodo,
                                fecha_factura = e.fecha_factura,
                                fecha_pago = e.fecha_pago,
                                tipo_factura = e.tipo_factura,
                                tipo_comprobante = tc.descripcion,
                                punto_venta = e.punto_venta,
                                nro_comprobante = e.nro_comprobante,
                                cuit_cuil = e.cuit_cuil,
                                nro_cuit_cuil = e.nro_cuit_cuil,
                                imp_neto_gravado = e.imp_neto_gravado,
                                imp_neto_no_gravado = e.imp_neto_no_gravado,
                                imp_op_exentas = e.imp_op_exentas,
                                iva = e.iva,
                                importe_total = e.importe_total,
                                description = e.description,
                                user_create = u.user_name,
                                profesional = pr.nombre,
                                fileName = e.name_file,
                                estadoDesc = e.estado == 0 ? "PAGADO" : e.estado == 1 ? "PAGO PARCIAL" : "IMPAGO"
                            }).First();
                }
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<ListFacturaProveedores> FacturaProveedoresList(int? estado, DateTime? fechaEmisionDesde, DateTime? fechaEmisionHasta)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                   
                    return (from e in ctx.factura_proveedores
                            join u in ctx.users on e.create_user_id equals u.id
                            join pr in ctx.profesional on e.profesional_id equals pr.id
                            where (e.estado == estado || estado == null)
                            && (e.fecha_factura >= fechaEmisionDesde || fechaEmisionDesde == null)
                            && (e.fecha_factura <= fechaEmisionHasta || fechaEmisionHasta == null)
                              && e.activo == 1
                            select new ListFacturaProveedores
                            {
                                id = e.id,
                                FechaFactura = e.fecha_factura,
                                Proveedor = pr.nombre,
                                FileName = e.name_file,
                                User = u.user_name,
                                EstadoDesc = e.estado == 0 ? "PAGADO" : e.estado == 1 ? "PAGO PARCIAL" : "IMPAGO"
                            }).ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<ReportFacturaProveedores> FacturaProveedoresReport(int? estado, DateTime? fechaD, DateTime? fechaH)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from e in ctx.factura_proveedores
                            join tc in ctx.tipo_comprobante on e.tipo_comprobante_id equals tc.id
                            join u in ctx.users on e.create_user_id equals u.id
                            join pr in ctx.profesional on e.profesional_id equals pr.id
                            where (e.estado == estado || estado == null)
                            && (e.fecha_factura >= fechaD || fechaD == null)
                            && (e.fecha_factura <= fechaH || fechaH == null)
                            && e.activo == 1
                            select new ReportFacturaProveedores
                            {
                                id = e.id,
                                Periodo = e.periodo,
                                FechaFactura = e.fecha_factura,
                                FechaPago = e.fecha_pago,
                                TipoFactura = e.tipo_factura,
                                TipoComprobante = tc.descripcion,
                                PuntoVenta = e.punto_venta,
                                NroComprobante = e.nro_comprobante,
                                CuitCuil = e.cuit_cuil,
                                NroCuitCuil = e.nro_cuit_cuil,
                                ImpNetoGravado = e.imp_neto_gravado,
                                ImpNetoNoGravado = e.imp_neto_no_gravado,
                                ImpOpExentas = e.imp_op_exentas,
                                Iva = e.iva,
                                ImporteTotal = e.importe_total,
                                Description = e.description,
                                UserCreate = u.user_name,
                                Proveedor = pr.nombre,
                                EstadoDesc = e.estado == 0 ? "PAGADO" : e.estado == 1 ? "PAGO PARCIAL" : "IMPAGO"
                            }).ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region --[ORDEN DE PAGO]--
        public IEnumerable<ListOrdenPagoCab> OrdenPagoCabList(long? profesional_Id, DateTime? fecha)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from e in ctx.orden_pago_cab 
                            join pr in ctx.profesional on e.profesional_id equals pr.id
                            where (e.profesional_id == profesional_Id || profesional_Id == null)
                            && (e.fecha == fecha || fecha == null)
                            && e.activo == 1
                            select new ListOrdenPagoCab
                            {
                               id = e.id,
                               FechaOrden = e.fecha,
                               Importe = e.importe_total,
                               Profesional = pr.nombre
                            }).ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        public OrdenesPagoCabDetVM GetOrdenPagoCab(long? id)
        {
            try
            {
                OrdenesPagoCabDetVM op = new OrdenesPagoCabDetVM();
                IEnumerable<ListOrdenPagoDet> opdet; ;

                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    op = (from e in ctx.orden_pago_cab
                            join pr in ctx.profesional on e.profesional_id equals pr.id
                            where e.id == id 
                            && e.activo == 1
                            select new OrdenesPagoCabDetVM
                            {
                                id = e.id,
                                Fecha = e.fecha,
                                Importe = e.importe_total,
                                Profesional = pr.nombre
                            }).First();

                    opdet = (from d in ctx.orden_pago_det
                             where d.orden_pago_cab_id == op.id
                             select new ListOrdenPagoDet
                             {
                                 id = d.id,
                                 PagaTotal = d.paga_total == true ? "si" : "no",
                                 Importe = d.importe,
                                 FormaPago = d.forma_pago,
                                 factura_proveedor_id = d.factura_proveedores_id,
                                 Banco = d.banco,
                                 NroCheque = d.nro_cheque,
                                 NroCtaCte = d.nro_cuenta_corriente,
                                 Observaciones = d.observaciones
                             }).ToList();  
                }
                op.list = opdet;
                return op;
            }
            catch
            {
                return null;
            }
        }
        public OrdenPagoCabVM GetDeleteOP(long? id)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from e in ctx.orden_pago_cab
                          join pr in ctx.profesional on e.profesional_id equals pr.id
                          where e.id == id 
                          && e.activo == 1
                          select new OrdenPagoCabVM
                          {
                              id = e.id,
                              Fecha = e.fecha,
                              Importe = e.importe_total,
                              Profesional = pr.nombre,
                              Profesional_Id = e.profesional_id
                          }).First();

                   
                }
            }
            catch
            {
                return null;
            }
        }
        public OrdenesPagoDetVM GetOrdenPagoCabDet(long idCab)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from e in ctx.orden_pago_cab
                          join pr in ctx.profesional on e.profesional_id equals pr.id
                          where e.id == idCab
                          && e.activo == 1
                          select new OrdenesPagoDetVM
                          {
                              idCab = e.id,
                              Fecha = e.fecha,
                              ImporteTotal = e.importe_total,
                              Profesional = pr.nombre,
                              ProfesionalId = e.profesional_id
                          }).First();                 
                }
            }
            catch
            {
                return null;
            }
        }
        public OrdenesPagoDetailsDetalle GetOrdenPagoDetails(long id)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from e in ctx.orden_pago_det
                            where e.id == id
                            select new OrdenesPagoDetailsDetalle
                            {
                                idCab = e.orden_pago_cab_id,
                                idDet = e.id,
                                FacturaProveedorId = e.factura_proveedores_id,
                                Importe = e.importe,
                                FormaPago = e.forma_pago,
                                Banco = e.banco,
                                NroCheque = e.nro_cheque,
                                NroCtaCte = e.nro_cuenta_corriente,
                                Observaciones = e.observaciones
                            }).First();
                }
            }
            catch
            {
                return null;
            }
        }
        public OrdenesPagoDetVM GetOrdenPagoDetalleEdit(long id)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from e in ctx.orden_pago_cab
                            join pr in ctx.profesional on e.profesional_id equals pr.id
                            join d in ctx.orden_pago_det on e.id equals d.orden_pago_cab_id
                            where d.id == id
                            && e.activo == 1
                            select new OrdenesPagoDetVM
                            {
                                idCab = e.id,
                                Fecha = e.fecha,
                                ImporteTotal = e.importe_total,
                                Profesional = pr.nombre,
                                ProfesionalId = e.profesional_id,
                                idDet = d.id,
                                FacturaProveedorId = d.factura_proveedores_id,
                                PagaTotal = d.paga_total,
                                Importe = d.importe,
                                FormaPago = d.forma_pago,
                                NroCheque = d.nro_cheque,
                                NroCtaCte = d.nro_cuenta_corriente,
                                Banco = d.banco,
                                Observaciones = d.observaciones
                            }).First();
                }
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<ReportOrdenPago> OrdenPagoReport(long? profesional_id, DateTime? fecha)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from e in ctx.orden_pago_cab
                            join pr in ctx.profesional on e.profesional_id equals pr.id
                            where (e.profesional_id == profesional_id || profesional_id == null)
                            && (e.fecha == fecha || fecha == null)
                            && e.activo == 1
                            select new ReportOrdenPago
                            {
                                id = e.id,
                                Fecha = e.fecha,
                                Importe = e.importe_total,
                                Profesional = pr.nombre
                            }).ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region --[COBROS]--
        public IEnumerable<ListCobros> CobrosList(string nroFactura)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from c in ctx.cobros
                            where (string.IsNullOrEmpty(nroFactura) || c.nro_factura.Contains(nroFactura))
                            && c.activo == 1
                            select new ListCobros
                            {
                                id = c.id,
                                nroRecibo = c.nro_recibo,
                                nroFactura = c.nro_factura,
                                fechaFactura = c.fecha_factura,
                                monto = c.monto,
                                cobroParcial = (double)c.cobro_parcial,
                                subtotalRecibo = (double)c.subtotal_recibo,
                                total = c.total,
                            }).ToList();
                }
            }
            catch
            { return null; }
        }

        public List<ReportCobros> CobrosReport(string nroFactura)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from c in ctx.cobros
                            join cli in ctx.client on c.cliente_id equals cli.id
                            where (string.IsNullOrEmpty(nroFactura) || c.nro_factura.Contains(nroFactura))
                            && c.activo == 1
                            select new ReportCobros
                            {
                                nroRecibo = c.nro_recibo,
                                cliente = cli.razonSocial,
                                nroFactura = c.nro_factura,
                                fechaFactura= c.fecha_factura,
                                monto = c.monto,
                                cobroParcial = (double)c.cobro_parcial,
                                subtotalRecibo = (double)c.subtotal_recibo,
                                total = c.total
                            }).ToList();
                }
            }
            catch
            {
                return null;
            }
        }

        public List<CobrosExportPDFVM> GetCobros(long? id)
        {
            try
            {
                using (AccountingEntities1 ctx = new AccountingEntities1())
                {
                    return (from c in ctx.cobros
                            join cli in ctx.client on c.cliente_id equals cli.id
                            where c.id == id 
                            && c.activo == 1
                            select new CobrosExportPDFVM
                            {
                                id=c.id,
                                nroRecibo = c.nro_recibo,
                                cliente = cli.razonSocial,
                                nroFactura = c.nro_factura,
                                fechaFactura = c.fecha_factura,
                                monto = c.monto,
                                cobroParcial = (double)c.cobro_parcial,
                                subtotalRecibo = (double)c.subtotal_recibo,
                                total = c.total
                            }).ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
