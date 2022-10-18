using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Context;
using MessageService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            return services;
        }
    }
}