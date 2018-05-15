using System;

namespace WebApplication.Domain.Base
{
    public interface IEntity
    {
         Guid Id { get; set; }
    }
}