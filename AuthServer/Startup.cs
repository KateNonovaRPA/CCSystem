using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Models.Context;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Models.Repositories;
using Models.Services;

namespace AuthServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var migrationsAssembly = typeof(MainContext).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<MainContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MainDB"),
                                    opt =>
                                    {
                                        opt.MigrationsAssembly(migrationsAssembly);
                                    });
            });

            string tmp = Configuration.GetConnectionString("MainDB");

            // Url Helper configured for injection
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(options =>
                 {
                     options.AccessDeniedPath = "/Account/ErrorForbidden";
                     options.LoginPath = "/Account/Login";
                 });

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            // .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new CultureInfo[]
                {
                    new CultureInfo("bg"),
                    new CultureInfo("en")
                };

                options.DefaultRequestCulture = new RequestCulture("bg");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            var tokenKey = Configuration.GetValue<string>("TokenKey");
            var key = Encoding.ASCII.GetBytes(tokenKey);

            services.ConfigureDI();
            services.AddSingleton<IJWTAuthenticationService>(new JWTAuthenticationService(tokenKey));
            services.AddSignalR();

           

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            //services.AddSingleton<IAuthService>(new AuthService(tokenKey));

            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });
           
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRequestLocalization();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            // Records exceptions and info to the POP Forums database.
            // logger.AddPopForumsLogger(app);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                                  name: "page by id",
                                  template: "page/view/{id?}/{url?}",
                                  defaults: new { controller = "Page", action = "Preview", area = "" }
                                  );

                routes.MapRoute(
                                  name: "page by url",
                                  template: "page/{pageType}/{url}",
                                  defaults: new { controller = "Page", action = "PreviewByUrl", area = "" }
                                  );

                routes.MapRoute(
                                    name: "areaRoute",
                                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                                    name: "default",
                                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                                    name: "lawsuits data",
                                    template: "{controller=Lawsuits}/{action=LawsuitsData}");
            });

            logger.AddLog4Net();
        }
    }
}