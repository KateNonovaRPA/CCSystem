using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Models.Contracts;
using Models.Services;

namespace AuthServer
{
    public static class StartupExtensions
    {
        public static IServiceCollection ConfigureDI(this IServiceCollection services)
        {
            services.AddTransient<ILawsuitService, LawsuitService>();
            services.AddTransient<ICourtService, CourtService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
