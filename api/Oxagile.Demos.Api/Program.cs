﻿using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Elasticsearch;

namespace Oxagile.Demos.Api
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(new CompactJsonFormatter(), LogEventLevel.Information)
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(
                        new Uri("http://oxagile.elasticsearch:9200"))
                        {
                            InlineFields = true,
                            AutoRegisterTemplate = true,
                            IndexFormat = "oxagile-api-logs-{0:yyyy.MM.dd}",
                            CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage:true)
                        })
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                BuildWebHost(args).Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();
    }
}
