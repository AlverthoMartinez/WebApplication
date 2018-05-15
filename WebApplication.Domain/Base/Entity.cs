using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Domain.Base
{
    public class Entity
    {
        [Key]
        public Guid Id { get; set; }
    }
}