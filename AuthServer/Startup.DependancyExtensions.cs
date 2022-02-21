using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Models.Contracts;
using Models.Repositories;
using Models.Services;

namespace AuthServer
{
    public static class StartupExtensions
    {
        public static IServiceCollection ConfigureDI(this IServiceCollection services)
        {
            services.AddTransient<ICourtService, CourtService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ILawsuitService, LawsuitService>();            
            services.AddTransient<IUserService, UserService>();            

            return services;
        }
    }
}
