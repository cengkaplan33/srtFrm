using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Surat.Entities
{
    public class GenericRepository<T> : IDisposable,IRepository<T> where T : class
    {
        #region Constructor

        public GenericRepository(DbContext context)
        {
            if (context == null)
            {                
                throw new ArgumentNullException("GenericRepository.Context");
            }
            
            this.GenericDBContext = context;
            this.dbSet = this.GenericDBContext.Set<T>();
        }

        #endregion

        #region Private Members 

        private DbSet<T> dbSet { get; set; }

        #endregion

        #region Protected Members

        protected DbContext GenericDBContext { get; set; }

        #endregion

        #region IDisposable

        public virtual void Dispose()
        {
            
        }

        #endregion

        #region Methods

        public IQueryable<T> GetAll()
        {
            return this.dbSet;
        }

        public T GetById(long id)
        {
            return this.dbSet.Find(id);
        }

        public virtual T GetObjectByParameters(Expression<Func<T, bool>> predicate)
        {
            return this.dbSet.Where(predicate).FirstOrDefault();
        }

        public virtual IQueryable<T> GetObjectsByParameters(Expression<Func<T, bool>> predicate)
        {
            return this.dbSet.Where(predicate);
        }

        public void Add(T entity)
        {
            DbEntityEntry entry = this.GenericDBContext.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                this.dbSet.Add(entity);
              
            }
        }

        public void Update(T entity)
        {
            DbEntityEntry entry = this.GenericDBContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.dbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            DbEntityEntry entry = this.GenericDBContext.Entry(entity);

            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                this.dbSet.Attach(entity);
                this.dbSet.Remove(entity);
            }
        }

        public void Delete(long id)
        {
            var entity = this.GetById(id);

            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public void Detach(T entity)
        {
            DbEntityEntry entry = this.GenericDBContext.Entry(entity);

            entry.State = EntityState.Detached;
        }

        #endregion

        
    }
}