using System;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Oxagile.Demos.Data.Entities;
using Oxagile.Demos.Api.Infrastructure.Filters.Exception;
using Oxagile.Demos.Api.Infrastructure.Swagger;
using StructureMap;
using Swashbuckle.AspNetCore.Swagger;
using WebApiContrib.Core.Formatter.Yaml;

namespace Oxagile.Demos.Api
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
                .AddMvcCore(_ => 
                {
                    _.Filters.Add(typeof(ExceptionFilter));
                })
                .AddFluentValidation()
                .AddJsonFormatters(_ => _.NullValueHandling = NullValueHandling.Ignore)
                .AddXmlSerializerFormatters()
                .AddYamlFormatters()
                .AddApiExplorer();

            services
                .AddDbContext<UserCompanyContext>(
                    _ => _.UseSqlServer(Configuration.GetConnectionString("Database")));

            services.AddOptions();
            services.Configure<Settings>(Configuration.GetSection("Settings"));

            services.AddSwaggerGen(_ =>
            {
                _.DescribeAllEnumsAsStrings();
                _.OperationFilter<FileOperationFilter>();
                _.SwaggerDoc("v1", new Info { Title = "Oxagile.Demos.Api", Version = "v1" });
            });

            services
                .AddMetrics(Configuration.GetSection("AppMetrics"))
                .AddHealthChecks()
                .AddMetricsMiddleware(Configuration.GetSection("AspNetMetrics"))
                .AddJsonSerialization();

            var container = new Container();
            container.Configure(config =>
            {
                config.AddRegistry<Registry>();
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All,    
            });
            
            app.UseResponseBuffering();

            app.UseMetrics();

            app.UseSwagger();
            app.UseSwaggerUI(_ =>
            {
                _.SwaggerEndpoint("/swagger/v1/swagger.json", "Oxagile.Demos.Api v1");
            });
            
            app.UseMvc();
        }
    }
}