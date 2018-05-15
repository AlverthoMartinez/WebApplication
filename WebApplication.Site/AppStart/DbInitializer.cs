using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Data;
using WebApplication.Domain;

namespace WebApplication.Site.AppStart
{
    public class DbInitializer
    {
        public void InitializeDatabase (SContext context) {
            context.Todo.Add(new Todo(){ Name = "Renew Drive license", Description = "Go to the control vehicular institute to renew my drive license."});

            context.SaveChanges();
        }
    }
}