using System.Text;
using MessageService.Application;
using MessageService.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace MessageService.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            ConfigureLogging();

            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Logging.ClearProviders();
            builder.Host.UseSerilog(Log.Logger);
            builder.Logging.AddSerilog(Log.Logger);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Token:Issuer"],
                        ValidAudience = configuration["Token:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddApplication(configuration);
            builder.Services.AddInfrastructure(configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }


        private static void ConfigureLogging()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .Build();

            var loggerConfiguration = new LoggerConfiguration();
            Log.Logger = loggerConfiguration
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithProperty("Environment", environment!)
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .WriteTo.Async(c => c.Console(new ElasticsearchJsonFormatter()))
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
                    new Uri(configuration["Logging:ElasticSearch:Url"]))
                {
                    MinimumLogEventLevel = LogEventLevel.Debug,
                    IndexFormat = "message-service-{0:yyyy.MM.dd}"
                })
                .CreateLogger();
        }
    }
}