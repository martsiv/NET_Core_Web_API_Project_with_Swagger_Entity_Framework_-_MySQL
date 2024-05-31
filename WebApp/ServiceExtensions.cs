using WebApp.Interfaces;
using WebApp.Services;

namespace WebApp
{
    public static class ServiceExtensions
    {
        public static void AddTokenService(this IServiceCollection services)
        {
            services.AddScoped<ITokenStorageService, TokenStorageService>();
        }
    }
}
