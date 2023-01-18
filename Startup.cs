

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using FunctionAppPerfTest;
using AutoMapper;
using System.Reflection;
using MediatR;
using System.Linq;
using GreenDotLogger;
using FunctionAppLoggerTest.MaskHandlers;

[assembly: FunctionsStartup(typeof(Startup))]
namespace FunctionAppPerfTest
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            //settings include app insight connection
            builder.ConfigurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"host.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.ConfigurationBuilder.Build();
            var conn = config["ConnectionString"];

            //builder.ConfigurationBuilder.AddAzureAppConfiguration(conn);

        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = builder.GetContext().Configuration;

            builder.Services.AddLogging();
            builder.Services.AddSingleton<ILogger>(sp => sp.GetService<ILogger<FunctionsStartup>>()!);

            //register GDApplicationInsights logger provider
            //builder.Services.AddGDApplicationInsights(config);

            //builder.Services.AddSingleton<IMaskHandler, SSNMaskHandler>();

            //builder.Services.AddSingleton(sp =>
            //    new MapperConfiguration(conf => { conf.AddProfile(new MapperProfile()); }).CreateMapper());

            builder.Services.AddAutoMapper(typeof(Startup));

            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

            // set logger filters 
            //builder.Services.AddLogging(loggingBuilder =>
            //{
            //    loggingBuilder.AddFilter<GDApplicationInsightsLoggerProvider>(
            //           "", LogLevel.Information);
            //    loggingBuilder.AddFilter<GDApplicationInsightsLoggerProvider>(
            //           "", LogLevel.Trace);

            //    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);//Trace
            //});

            //log startup information
            //var list=builder.Services.Where(c => c.ServiceType == typeof(ILoggerProvider)).ToList();
            //var loggers = builder.Services.Where(c => c.ServiceType == typeof(ILogger)).ToList();
            //if (loggers.Any())
            //{
            //    var serviceProvider = builder.Services.BuildServiceProvider();
            //    var provider = serviceProvider.GetRequiredService<ILogger>();
            //}

            //var serviceProvider = builder.Services.BuildServiceProvider();
            //var loggers=serviceProvider.GetServices<ILogger>();
            //var provider = serviceProvider.GetRequiredService<ILoggerProvider>();
            //var logger = provider.CreateLogger("Startup");
            //logger.LogInformation("Got Here in Startup");

            //var GDLoggerProvider = serviceProvider.GetRequiredService<GDApplicationInsightsLoggerProvider>();
            //var logger = GDLoggerProvider.CreateLogger("Startup");

            //logger.LogInformation("Got Here in Startup");
        }
    }
}
