using Data;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SampleJTWAuthNoMiddleware.Handlers;
using Services;

namespace SampleJTWAuthNoMiddleware.Configure
{
    public static class ConfigureDependency
    {
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddTransient<ApiHandler>();
            services.AddHttpClient<IApiService, ApiService>()
                .AddHttpMessageHandler<ApiHandler>();

            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IUserRepository, UserRepository>();

        }
    }
}
