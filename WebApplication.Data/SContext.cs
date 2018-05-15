
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using WebApplication.Domain;
using WebApplication.Domain.Base;
using WebApplication.Data.utils;
// using System.Data.Entity.ModelConfiguration.Conventions;
// using StackExchange.Profiling.Data;
// using StackExchange.Profiling;

namespace WebApplication.Data
{
    public class SContext : DbContext
    {
        public DbSet<Todo> Todo { get; set; }

        public SContext(DbContextOptions<SContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = AppConfigurationBuilder.GetBuilder();
                
                var connectionString = configuration.GetConnectionString("WebApplicationContext");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        public SContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

        }
    }
}


