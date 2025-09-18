using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NLog;
using NLog.Web;
using System;
using System.IO.Compression;
using System.Reflection;
using Xim.AppApi.ActionFilters;
using Xim.AppApi.Contexts;
using Xim.AppApi.Models;
using Xim.AppApi.Services;
using Xim.Application;
using Xim.Domain.Mssql;
using Xim.Library.Extensions;
using Xim.Storage;
using Xim.AppApi.Constants;
const string CORS_KEY = "AllowAllOrigins";
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    var services = builder.Services;
    var configuration = builder.Configuration;


    // Add NLog for Logging
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    logger.Debug("Init main");
    //app config
    var appSettingsSection = configuration.InjectConfig<AppConfig>(services);
    
    //cors
    services.AddCors(options =>
    {
        options.AddPolicy(CORS_KEY,
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
    });

    //compress
    services.AddResponseCompression();
    services.Configure<GzipCompressionProviderOptions>(options =>
    {
        options.Level = CompressionLevel.Fastest;
    });

    //repo-service
    var mysqlConnection = builder.Configuration.GetConnectionString("App");
    Console.WriteLine($"----- ConnectionString.App {mysqlConnection}");
    //MysqlFactory.ConfigureAppRepository(services, mysqlConnection);
    MssqlFactory.ConfigureAppRepository(services, mysqlConnection);
    ServiceFactory.ConfigureService(services);

    //jwt
    services.AddJwtToken(configuration);

    //api context
    services.AddScoped<IContextService, ApiService>();

    services
        .AddControllers(option =>
        {
            option.Filters.Add<ExceptionActionFilter>();
        })
        .AddNewtonsoftJson(options =>
        {
            //options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            //options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            options.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        });

    services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Portal PTN API", Version = "v1" });
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        // Define the BearerAuth scheme
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter into field the word 'Bearer' followed by a space and the JWT",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });

        // Apply BearerAuth globally to all operations
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    //storage    
    configuration.UseFileStorage(services);

    var app = builder.Build();

    // Sử dụng cấu hình CORS
    app.UseCors(CORS_KEY);

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });
    //}
    app.UseStaticFiles();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    Console.WriteLine($"---- app.Run()");

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}