using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace accounting.Repositories
{
    public class RepoGeneric<T> : IRepoGeneric<T> where T : class
    {
        #region --[GLOBAL]--

        private readonly IContext<T> _context;

        #endregion --[GLOBAL]--

        #region --[CONSTRUCTOR]--

        public RepoGeneric(DbContext context)
        {
            _context = new Context<T>(context);
        }

        public RepoGeneric(IContext<T> context)
        {
            _context = context;
        }

        #endregion --[CONSTRUCTOR]--

        #region --[ADD]--

        /// <summary>
        /// Crea una entidad, después de aplicar los cambios.
        /// </summary>
        /// <param name="entity">Nueva Entidad</param>
        /// <returns>Entidad creada</returns>
        public T Add(T entity)
        {
            return _context.DbSet.Add(entity);
        }

        /// <summary>
        /// Crea un listado de una entidad, después de aplicar los cambios.
        /// </summary>
        /// <param name="entities">Listado de entidad</param>
        /// <returns>Entidades agregadas</returns>
        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            return _context.DbContext.Set<T>().AddRange(entities);
        }

        #endregion --[ADD]--

        #region --[REMOVE]--

        /// <summary>
        /// Elimina una entidad, después de aplicar los cambios.
        /// </summary>
        /// <param name="entity">Entidad a eliminar</param>
        /// <returns>Entidad eliminada</returns>
        public T Remove(T entity)
        {
            return _context.DbSet.Remove(entity);
        }

        /// <summary>
        /// Elimina un rango de una entidad, después de aplicar los cambios.
        /// </summary>
        /// <param name="entities">Listado de entidad</param>
        /// <returns>Listado de entidad</returns>
        public IEnumerable<T> RemoveRange(IEnumerable<T> entities)
        {
            return _context.DbContext.Set<T>().RemoveRange(entities);
        }

        #endregion --[REMOVE]--

        #region --[UPDATE]--

        /// <summary>
        /// Actualiza una entidad, después de aplicar los cambios.
        /// </summary>
        /// <param name="entity">Entidad</param>
        /// <returns>Entidad actualizada</returns>
        public T Update(T entity)
        {
            var updated = _context.DbSet.Attach(entity);
            _context.DbContext.Entry(entity).State = EntityState.Modified;
            return updated;
        }

        #endregion --[UPDATE]--

        #region --[SAVE]--

        /// <summary>
        /// Aplica los cambios en la DB.
        /// </summary>
        public void SaveChanges()
        {
            _context.DbContext.SaveChanges();
        }

        #endregion --[SAVE]--

        #region --[GET]--

        /// <summary>
        /// Obtiene toda la entidad
        /// </summary>
        /// <returns>Todo el contenido de una entidad</returns>
        public IQueryable<T> GetAll()
        {
            return _context.DbSet;
        }

        /// <summary>
        /// Obtiene un registro de la entidad que se busca por Key.
        /// </summary>
        /// <returns>El contenido de la entidad</returns>
        public T Get(object key)
        {
            return _context.DbSet.Find(key);
        }

        /// <summary>
        /// Busca un entidad con fitros custom.
        /// </summary>
        /// <param name="match">filtros a aplicar</param>
        /// <returns>Entidad que coincide con los filtros</returns>
        public T Find(Expression<Func<T, bool>> match)
        {
            return _context.DbSet.SingleOrDefault(match);
        }

        /// <summary>
        /// Retoran un colección de una entidad, aplicando filtros custom.
        /// </summary>
        /// <param name="match">Filtros custom</param>
        /// <returns>Colleccion-es que coinciden con los filtros.</returns>
        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return _context.DbSet.Where(match).ToList();
        }

        #endregion --[GET]--

        #region --[COUNT]--

        /// <summary>
        /// Obtiene la cantidad de registros de una entidad.
        /// </summary>
        /// <param name="entity">Entidad a contar</param>
        /// <returns>Cantidad</returns>
        public long Count(T entity)
        {
            return _context.DbSet.Count();
        }

        /// <summary>
        /// Obtiene la cantidad de registros de una entidad que coinciden con los filtros custom.
        /// </summary>
        /// <param name="match">Filtros custom</param>
        /// <returns>Cantidad de registros</returns>
        public long CountFind(Expression<Func<T, bool>> match)
        {
            return _context.DbSet.Count(match);
        }

        #endregion --[COUNT]--

        #region --[EXECUTE]--

        /// <summary>
        /// Obtiene un listado de una entidad, mediante un procedimiento almacenado
        /// </summary>
        /// <param name="sql">procedimiento almacendo</param>
        /// <param name="parameters">parámetros</param>
        /// <returns>Listado de la entidad</returns>
        public IEnumerable<T> SqlQueryExecute(string sql, params object[] parameters)
        {
            return _context.DbContext.Database.SqlQuery<T>(sql, parameters).ToList();
        }

        /// <summary>
        /// Actualiza una entidad y retorna la misma, mediante un procedimiento almacenado
        /// </summary>
        /// <param name="sql">procedimiento almacendo</param>
        /// <param name="parameters">parámetros</param>
        /// <returns>Entidad Actuaizada</returns>
        public T SqlQueryExecuteUpdate(string sql, params object[] parameters)
        {
            return _context.DbContext.Database.SqlQuery<T>(sql, parameters).FirstOrDefault();
        }

        /// <summary>
        /// Permite crear, edita o actualizar.  
        /// </summary>
        /// <param name="sql">Query que crea actualiza o elimina</param>
        /// <param name="parameters"> parametros que se le pasan a la query</param>
        public void AddOrUpdateOrDelete(string sql, params object[] parameters)
        {
            _context.DbContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        #endregion --[EXECUTE]--

    }
}