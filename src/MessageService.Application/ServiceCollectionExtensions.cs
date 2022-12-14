using System.Reflection;
using MediatR;
using MessageService.Application.Services.Token;
using MessageService.Application.Services.UserLog;
using Microsoft.Extensions.DependencyInjection;


namespace MessageService.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<ITokenService, TokenService>();

            services.AddHostedService<UserLogService>();
            services.AddHostedService<Services.Message.MessageService>();

            return services;
        }
    }
}