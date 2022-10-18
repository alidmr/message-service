using System.Reflection;
using MediatR;
using MessageService.Application.Services.RabbitMq;
using MessageService.Application.Services.Token;
using MessageService.Application.Services.UserLog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace MessageService.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<ITokenService, TokenService>();

            services.AddSingleton(sp =>
            {
                var connectionFactory = new ConnectionFactory()
                {
                    HostName = configuration["RabbitMqSettings:HostName"],
                    UserName = configuration["RabbitMqSettings:UserName"],
                    Password = configuration["RabbitMqSettings:Password"],
                    Port = Convert.ToInt32(configuration["RabbitMqSettings:Port"]),
                    DispatchConsumersAsync = true
                };
                return connectionFactory;
            });

            services.AddSingleton<IRabbitMqService, RabbitMqService>();

            services.AddHostedService<UserLogService>();
            services.AddHostedService<Services.Message.MessageService>();

            return services;
        }
    }
}