using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Context;
using MessageService.Infrastructure.Repositories;
using MessageService.Infrastructure.Services.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace MessageService.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDbSection = configuration.GetSection("MongoDbSettings");
            services.Configure<MongoDbSettings>(mongoDbSection);

            services.AddScoped<IMessageServiceContext, MessageServiceContext>();
            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserLogRepository, UserLogRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IBlockUserRepository, BlockUserRepository>();
            
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

            return services;
        }
    }
}