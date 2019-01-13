using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using SimpleRegister;
using AutoMapper;
using BulletinDomain.MapperProfiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Authenticaton.Contracts.JwtAuthentication.Options;
using Authentication.AppServices.JwtAuthentication;
using Authentication.AppServices.CookieAuthentication;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace WebApiService
{
    public class Startup
    {
        //private Container container = new Container();
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            AutoMapperInitializer.Initialize();
            services.AddAutoMapper();           
            services.AddDependencyInjection(Configuration.GetConnectionString("DefaultConnection"));
            services.RegisterIdentity();
            services.Configure<JwtServerAuthenticationOptions>(Configuration.GetSection("JwtAuthentication"));
            services.Configure<JwtBaseAuthenticationOptions>(Configuration.GetSection("JwtAuthentication"));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            });
            services.AddAuthentication(options =>
            {                
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;                
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = new JwtServerAuthenticationOptions();
                Configuration.GetSection("JwtAuthentication").Bind(jwtOptions);
                if (HostingEnvironment.IsDevelopment())
                {
                    options.RequireHttpsMetadata = true;
                }
                options.SaveToken = true;
                options.Audience = jwtOptions.Audience;
                options.ClaimsIssuer = jwtOptions.Issuer;
                options.TokenValidationParameters = JwtDefaultsProvider.GetTokenValidationParameters(
                    jwtOptions.Issuer, jwtOptions.Audience, jwtOptions.Secret);
            });            
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;                
            });
            services.AddMiniProfiler(options => 
            {
                options.RouteBasePath = "/profiler";
                options.TrackConnectionOpenClose = false;
            }).AddEntityFramework();


            services.AddScoped<IJwtBasedCookieAuthenricationService, JwtBasedCookieAuthenricationService>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);      
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {          
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("ru")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseMiniProfiler();
            app.UseMvc();
            
        }        
    }
}
