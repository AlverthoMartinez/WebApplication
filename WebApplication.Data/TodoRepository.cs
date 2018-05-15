using System;
using WebApplication.Data.Base;
using WebApplication.Domain;

namespace WebApplication.Data
{
    public class TodoRepository: EntityRepository<Todo>, ITodoRepository
    {
        public TodoRepository()
            : this(new SContext())
        { }

        public TodoRepository(SContext context)
            : base(context)
        {
        }
    }
    public interface ITodoRepository : IEntityRepository<Todo>, IDisposable
    {
    }
}
