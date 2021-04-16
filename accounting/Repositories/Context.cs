using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace accounting.Repositories
{
    public class Context<T> : IContext<T> where T : class
    {
        public Context(DbContext context)
        {
            DbContext = context;
            DbSet = DbContext.Set<T>();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }

        public DbContext DbContext { get; private set; }
        public IDbSet<T> DbSet { get; private set; }
    }
}