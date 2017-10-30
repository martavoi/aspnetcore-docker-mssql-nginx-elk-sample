using System;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Oxagile.Internal.Api.Entities;
using Oxagile.Internal.Api.Filters.Exception;
using Oxagile.Internal.Api.Formatters;
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
                .AddMvcCore(_ => 
                {
                    _.Filters.Add(typeof(ExceptionFilter));

                    _.OutputFormatters.RemoveType<TextOutputFormatter>();
                    _.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
                })
                .AddFluentValidation()
                .AddJsonFormatters(_ => _.NullValueHandling = NullValueHandling.Ignore)
                .AddXmlSerializerFormatters()
                .AddCsvSerializerFormatters()
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
                _.SwaggerDoc("v1", new Info { Title = "Oxagile.Api", Version = "v1" });
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
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All,    
            });
            app.UseResponseBuffering();

            app.UseSwagger();
            app.UseSwaggerUI(_ =>
            {
                _.SwaggerEndpoint("/swagger/v1/swagger.json", "Oxagile.Api v1");
            });
            
            app.UseMvc();
        }
    }
}