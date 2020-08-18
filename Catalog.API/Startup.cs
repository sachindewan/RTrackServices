using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using Catalog.API.Data;
using Catalog.API.Data.Interfaces;
using Catalog.API.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Catalog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CatalogDatabaseSettings>(Configuration.GetSection(nameof(CatalogDatabaseSettings)));
            services.AddSingleton<ICatalogDatabaseSettings>(Sp => Sp.GetRequiredService<IOptions<CatalogDatabaseSettings>>().Value);
            services.AddControllers();
            services.AddTransient<ICatalogContext, CatalogContext>();
            services.AddTransient<Catalog.API.Repositories.Interfaces.IProductRepository, Catalog.API.Repositories.ProductRepository>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Catalog Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "Catalog Api v1");
                //options.RoutePrefix = "";
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
