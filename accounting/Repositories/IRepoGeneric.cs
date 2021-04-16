using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace accounting.Repositories
{
    public interface IRepoGeneric<T>
    {

        #region --[ADD]--

        /// <summary>
        /// Crea una entidad, después de aplicar los cambios.
        /// </summary>
        /// <param name="entity">Nueva Entidad</param>
        /// <returns>Entidad creada</returns>
        T Add(T entity);

        /// <summary>
        /// Crea un listado de una entidad, después de aplicar los cambios.
        /// </summary>
        /// <param name="entities">Listado de entidad</param>
        /// <returns>Entidades agregadas</returns>
        IEnumerable<T> AddRange(IEnumerable<T> entities);

        #endregion --[ADD]--

        #region --[REMOVE]--

        /// <summary>
        /// Elimina una entidad, después de aplicar los cambios.
        /// </summary>
        /// <param name="entity">Entidad a eliminar</param>
        /// <returns>Entidad eliminada</returns>
        T Remove(T entity);

        /// <summary>
        /// Elimina un rango de una entidad, después de aplicar los cambios.
        /// </summary>
        /// <param name="entities">Listado de entidad</param>
        /// <returns>Listado de entidad</returns>
        IEnumerable<T> RemoveRange(IEnumerable<T> entities);

        #endregion --[REMOVE]--

        #region --[UPDATE]--

        /// <summary>
        /// Actualiza una entidad, después de aplicar los cambios.
        /// </summary>
        /// <param name="entity">Entidad</param>
        /// <returns>Entidad actualizada</returns>
        T Update(T entity);

        #endregion --[UPDATE]--

        #region --[SAVE]--

        /// <summary>
        /// Aplica los cambios en la DB.
        /// </summary>
        void SaveChanges();

        #endregion --[SAVE]--

        #region --[GET]--

        /// <summary>
        /// Obtiene toda la entidad
        /// </summary>
        /// <returns>Todo el contenido de una entidad</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Obtiene un registro de la entidad que se busca por Key.
        /// </summary>
        /// <returns>El contenido de la entidad</returns>
        T Get(object key);

        /// <summary>
        /// Busca un entidad con fitros custom.
        /// </summary>
        /// <param name="match">filtros a aplicar</param>
        /// <returns>Entidad que coincide con los filtros</returns>
        T Find(Expression<Func<T, bool>> match);

        /// <summary>
        /// Retoran un colección de una entidad, aplicando filtros custom.
        /// </summary>
        /// <param name="match">Filtros custom</param>
        /// <returns>Colleccion-es que coinciden con los filtros.</returns>
        ICollection<T> FindAll(Expression<Func<T, bool>> match);

        #endregion --[GET]--

        #region --[COUNT]--

        /// <summary>
        /// Obtiene la cantidad de registros de una entidad.
        /// </summary>
        /// <param name="entity">Entidad a contar</param>
        /// <returns>Cantidad</returns>
        long Count(T entity);

        /// <summary>
        /// Obtiene la cantidad de registros de una entidad que coinciden con los filtros custom.
        /// </summary>
        /// <param name="match">Filtros custom</param>
        /// <returns>Cantidad de registros</returns>
        long CountFind(Expression<Func<T, bool>> match);

        #endregion --[COUNT]--

        #region --[EXECUTE]--

        /// <summary>
        /// Obtiene un listado de una entidad, mediante un procedimiento almacenado
        /// </summary>
        /// <param name="sql">procedimiento almacendo</param>
        /// <param name="parameters">parámetros</param>
        /// <returns>Listado de la entidad</returns>
        IEnumerable<T> SqlQueryExecute(string sql, params object[] parameters);

        /// <summary>
        /// Actualiza una entidad y retorna la misma, mediante un procedimiento almacenado
        /// </summary>
        /// <param name="sql">procedimiento almacendo</param>
        /// <param name="parameters">parámetros</param>
        /// <returns>Entidad Actuaizada</returns>
        T SqlQueryExecuteUpdate(string sql, params object[] parameters);

        /// <summary>
        /// Permite crear, edita o actualizar.  
        /// </summary>
        /// <param name="sql">Query que crea actualiza o elimina</param>
        /// <param name="parameters"> parametros que se le pasan a la query</param>
        void AddOrUpdateOrDelete(string sql, params object[] parameters);

        #endregion --[EXECUTE]--
    }
}