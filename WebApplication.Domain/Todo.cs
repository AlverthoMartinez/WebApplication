using System;
using WebApplication.Domain.Base;

namespace WebApplication.Domain
{
    public class Todo : Entity, IEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
