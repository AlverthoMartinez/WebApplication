using System;

namespace WebApplication.Site.Models.DTO
{
    public class TodoDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}