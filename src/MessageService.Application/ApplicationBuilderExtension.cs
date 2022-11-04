using MessageService.Application.Services.UserLog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MessageService.Application
{
    public static class ApplicationBuilderExtension
    {
        private static UserLogTestService Listener { get; set; }

        public static IApplicationBuilder UseRabbitMqListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<UserLogTestService>();
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            if (life == null)
                return app;

            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStopping);
            return app;
        }

        private static void OnStarted()
        {
            Listener.Consume();
        }

        private static void OnStopping()
        {
            Listener.Disconnect();
        }
    }
}