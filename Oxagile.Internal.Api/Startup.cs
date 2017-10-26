using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oxagile.Internal.Api.Entities;
using StructureMap;
using Swashbuckle.AspNetCore.Swagger;

namespace Oxagile.Internal.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddJsonFormatters()
                .AddXmlSerializerFormatters()
                .AddApiExplorer();

            services
                .AddDbContext<UserCompanyContext>(
                    _ => _.UseSqlServer(Configuration.GetConnectionString("Database")));

            services.AddSwaggerGen(_ =>
            {
                _.SwaggerDoc("v1", new Info { Title = "Oxagile.Internal.Api", Version = "v1" });
            });

            var container = new Container();
            container.Configure(config =>
            {
                config.Scan(_ => 
                {
                    _.WithDefaultConventions();
                });
                config.AddRegistry<Registry>();
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
