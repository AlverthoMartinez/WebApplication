using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using WebApplication.Data.utils;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.FileProviders;

using WebApplication.Data;

using WebApplication.Site.AppStart; 

namespace WebApplication.Site
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IConfigurationRoot ConfigurationRoot { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddAutoMapper(typeof(AutoMaperProfile));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 

            ConfigurationRoot = AppConfigurationBuilder.GetBuilder();

            var connectionString = ConfigurationRoot.GetConnectionString("WebApplicationContext");
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<SContext>(options => options.UseNpgsql(connectionString));

            services.AddTransient<ITodoRepository, TodoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true,
                    ConfigFile = "./build/webpack.dev.conf.js"
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "dist")),
                RequestPath = ""
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            DBConfig.Config();
        }
    }
}
