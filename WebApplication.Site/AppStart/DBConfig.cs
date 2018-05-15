using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore;
// using WebMatrix.WebData;

using WebApplication.Data;

namespace WebApplication.Site.AppStart
{
    public class DBConfig
    {
        public static void Config()
        {
            using (var context = new SContext())
            {
                if (context.Database.EnsureCreated())
                {
                    new DbInitializer().InitializeDatabase(context);
                }
            }

            
        }
    }
}

