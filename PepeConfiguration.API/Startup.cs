using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PepeConfiguration.API.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PepeConfiguration.API.Infraestructura;
using Microsoft.OpenApi.Models;

namespace PepeConfiguration.API
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
            services.AddControllers();
            services.AddSignalR();


            services.AddDbContext<ApplicationContext>(config =>
            {
                config.UseSqlServer(@"Server = DESKTOP-J4947VK\SQLEXPRESS; Database = PruebaRestful; Integrated Security = True; ");
            });


            services.AddSwaggerGen(c=>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Configuracion Centralizada", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c=>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Configuracion Centralizada");
            });

            app.UseAuthorization();

           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<PepeConfigurationHub>("/pepeConfiguracion");
            });

           
        }
    }
}
