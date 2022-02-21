using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
using Models.Contracts;
using Models.Services;
using Models.ViewModels;
using System.Globalization;
using System.Reflection;
using System.Text;

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
            var optionsBuilder = new DbContextOptionsBuilder<MainContext>();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("MainDB"));
            services.AddDbContext<MainContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MainDB"),
                                    opt =>
                                    {
                                        opt.MigrationsAssembly(migrationsAssembly);
                                    });
            });

            string tmp = Configuration.GetConnectionString("MainDB");
            var APIKey = Configuration.GetValue<string>("APIKey");
            var key = Encoding.ASCII.GetBytes(APIKey);

            var emailConfig = Configuration
            .GetSection("EmailConfiguration")
            .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddControllers();

            services.ConfigureDI();
            // services.AddSingleton<IAuthService>(new AuthService(tokenKey));
            //services.AddScoped<IAuthService>(_ => new AuthService(tokenKey));
            services.AddSingleton<IAuthService>(new AuthService(APIKey, optionsBuilder.Options));
            services.AddSignalR();

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

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = false,
            //        ValidateAudience = false
            //    };
            //});
            //services.AddSingleton<IAuthService>(new AuthService(tokenKey));
            //var tokenProvider = new Models.Services.AuthenticationService("issuer", "audience", "mykeyname");
            //services.AddSingleton((Models.Repositories.IAuthenticationService)tokenProvider);

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.RequireHttpsMetadata = false;
            //        options.TokenValidationParameters = tokenProvider.GetValidationParameters();
            //    });

            // This is for the [Authorize] attributes.
            services.AddAuthorization(auth =>
            {
                auth.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

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