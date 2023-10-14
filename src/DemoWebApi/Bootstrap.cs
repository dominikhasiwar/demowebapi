using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Castle.Facilities.AspNetCore;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using DemoWebApi.Attributes;
using DemoWebApi.Common;
using DemoWebApi.Handlers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace DemoWebApi
{
    public static class Bootstrap
    {
        private static WebApplication _application;

        static Bootstrap()
        {
            // Configure serilog
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(AppSettings.Config)
                .CreateLogger();
        }

        public static Task Run(params string[] args)
        {
            Log.Information($"Starting {AppInfo.Current.AppName} v{AppInfo.Current.AppVersion}...");
            Log.Information("Application Configuration:");
            Log.Information(JsonConvert.SerializeObject(AppSettings.Current, Formatting.Indented));

            // Create application
            _application = CreateApplication(args);

            // Start application
            return _application.RunAsync();
        }

        private static WebApplication CreateApplication(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(opt =>
            {
                opt.Filters.Add(new TypeFilterAttribute(typeof(OperationLoggerAttribute)));
                opt.Filters.Add(new TypeFilterAttribute(typeof(OperationValidatorAttribute)));
            });

            // Configure api versioning
            builder.Services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = ApiVersion.Default;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("x-api-version"),
                    new MediaTypeApiVersionReader("x-api-version"));
            }).AddApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureOptions<SwaggerConfigOptions>();

            builder.WebHost.UseUrls(AppSettings.Current.BaseAddress);

            // Configure logging
            builder.Services.AddLogging(cfg =>
            {
                cfg.ClearProviders();
                cfg.AddSerilog(Log.Logger);
            });

            // Configure dependency injection
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            var container = new WindsorContainer();

            container.AddFacility<AspNetCoreFacility>(f => f.CrossWiresInto(builder.Services));

            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, true));
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel, true));
            container.Install(FromAssembly.InThisApplication(typeof(Bootstrap).Assembly));

            builder.Services.AddWindsor(container);

            // Build app
            var app = builder.Build();

            app.ConfigureExceptionHandler();

            app.UseSwagger();

            app.UseSwaggerUI(opt =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    opt.SwaggerEndpoint($"{desc.GroupName}/swagger.json", desc.ApiVersion.ToString());
                }
            });

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
