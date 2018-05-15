using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL; 

using WebApplication.Domain;
using WebApplication.Domain.Base;

namespace WebApplication.Data.Base
{
    public class EntityRepository<T> : IEntityRepository<T> where T : class, IEntity
    {
        protected SContext context;

        public EntityRepository()
            : this(null) { }

        public EntityRepository(SContext context)
        {
            this.context = context ?? new SContext();
        }

        protected void SetStateAsDeleted<E, C>(E current, Func<E, ICollection<C>> pathToChildren,
            Action<C> after = null)
            where E : IEntity
            where C : class, IEntity
        {
            if (current == null)
            {
                return;
            }

            var currentList = pathToChildren(current);
            if (currentList == null)
            {
                return;
            }
            var identifierComparer = new Func<C, C, bool>((c, a) => a.Id == c.Id);

            var toDelete = currentList.ToList();

            if (after != null)
            {
                foreach (var item in toDelete)
                {
                    after(item);
                }
            }

            // Delete not found
            toDelete.ForEach(x => context.Set<C>().Remove(x));

        }
        protected void SetState<E, C>(
            E current, E actual,
            Func<E, ICollection<C>> pathToChildren)
            where E : IEntity
            where C : class, IEntity
        {
            this.SetState(current, actual, pathToChildren, null, null);
        }

        protected void SetState<E, C>(
            E current, E actual,
            Func<E, ICollection<C>> pathToChildren,
            Action<C, C> after)
            where E : IEntity
            where C : class, IEntity
        {
            this.SetState(current, actual, pathToChildren, null, after);
        }

        protected void SetState<E, C>(
            E current, E actual,
            Func<E, ICollection<C>> pathToChildren,
            Func<C, C, bool> comparer)
            where E : IEntity
            where C : class, IEntity
        {
            this.SetState(current, actual, pathToChildren, comparer, null);
        }

        protected void SetState<E, C>(
            E current, E actual,
            Func<E, ICollection<C>> pathToChildren,
            Func<C, C, bool> comparer,
            Action<C, C> after)
            where E : IEntity
            where C : class, IEntity
        {
            if (current == null || actual == null)
            {
                return;
            }

            var currentList = pathToChildren(current);
            var newList = pathToChildren(actual);
            if (currentList == null || newList == null)
            {
                return;
            }
            var identifierComparer = comparer ?? new Func<C, C, bool>((c, a) => a.Id == c.Id);

            var toDelete = currentList.Where(c => !newList.Any(n => identifierComparer(c, n))).ToList();
            var toAdd = newList.Where(n => !currentList.Any(c => identifierComparer(c, n))).ToList();
            var toModify = currentList.Join(newList, c => c.Id, n => n.Id, (c, n) => new
            {
                Current = c,
                New = n,
            }).ToList();

            if (after != null)
            {
                foreach (var pair in toModify)
                {
                    after(pair.Current, pair.New);
                }
            }

            // Delete not found
            toDelete.ForEach(x => context.Set<C>().Remove(x));
            // Modified found
            toModify.ForEach(x => context.Entry(x.Current).CurrentValues.SetValues(x.New));
            // Add new                   
            toAdd.ForEach(x => currentList.Add(x));
        }

        protected void SetState<C>(
            IEnumerable<C> currentList,
            IEnumerable<C> newList,
            Func<C, C, bool> comparer = null)
            where C : class, IEntity
        {
            var identifierComparer = comparer ?? new Func<C, C, bool>((c, a) => a.Id == c.Id);

            var toDelete = currentList.Where(c => !newList.Any(n => c.Id == n.Id)).ToList();
            var toAdd = newList.Where(n => !currentList.Any(c => c.Id == n.Id)).ToList();
            var toModify = currentList.Join(newList, c => c.Id, n => n.Id, (c, n) => new
            {
                Current = c,
                New = n,
            }).ToList();


            // Delete not found
            toDelete.ForEach(x => context.Set<C>().Remove(x));
            // Modified found
            toModify.ForEach(x => context.Entry(x.Current).CurrentValues.SetValues(x.New));
            // Add new                   
            toAdd.ForEach(x => context.Set<C>().Add(x));
        }

        public virtual IQueryable<T> All
        {
            get { return context.Set<T>(); }
        }

        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public virtual T Find(Guid id)
        {
            return context.Set<T>().Find(id);
        }

        public virtual IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().Where(predicate);
        }

        public virtual void InsertOrUpdate(T entity)
        {
            if (entity.Id == default(Guid))
            {
                // New entity
                context.Set<T>().Add(entity);
            }
            else
            {
                // Existing entity
                context.Entry(entity).State = EntityState.Modified;
            }
        }

        public virtual void Delete(Guid id)
        {
            var entity = context.Set<T>().Find(id);
            context.Set<T>().Remove(entity);
        }

        //public virtual void DeleteAll(List<PropertyTaxes> propertyTaxes)
        //{
        //    foreach(var pt in propertyTaxes){
        //        context.Set<PropertyTaxes>().Remove(pt);
        //    }
            
        //}

        public virtual void Save()
        {
            context.SaveChanges();
        }


        public virtual void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
            }
        }
    }

    public interface IEntityRepository<T> : IDisposable
        where T : class, IEntity
    {
        IQueryable<T> All { get; }
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        void Delete(Guid id);
        T Find(Guid id);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate);
        void InsertOrUpdate(T entity);
        void Save();

    }
}
