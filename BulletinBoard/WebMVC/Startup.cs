using Authentication.AppServices.CookieAuthentication;
using Authentication.AppServices.JwtAuthentication;
using Authenticaton.Contracts.JwtAuthentication.Options;
using AutoMapper;
using BulletinDomain.MapperProfiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleRegister;
using System.Globalization;

namespace WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources"); //
            services.AddDependencyInjection(Configuration.GetConnectionString("DefaultConnection"));
            services.Configure<JwtClientAuthenticatonOptions>(Configuration.GetSection("JwtAuthentication"));
            services.Configure<JwtBaseAuthenticationOptions>(Configuration.GetSection("JwtAuthentication"));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {
                    o.LoginPath = "/authorization/signin";
                });                

            services.AddHttpContextAccessor();
            AutoMapperInitializer.Initialize();
            services.AddAutoMapper();
            
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IJwtBasedCookieAuthenricationService, JwtBasedCookieAuthenricationService>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddDataAnnotationsLocalization().AddViewLocalization();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Localization
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
            //End Localization


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                //comments
                routes.MapRoute("get new comments information", "get-new-comments-information/{lastPublicationTime?}", new { controller = "Advert", action = "GetNewCommentsInformation" });
                routes.MapRoute("add comment action", "advert/add-comment-action", new { controller = "Advert", action = "AddCommentAction" });
                routes.MapRoute("delete comment action", "advert/delete-comment-action/{id?}", new { controller = "Advert", action = "DeleteCommentAction" });
                routes.MapRoute("show more comments", "advert/show-more-comments", new { controller = "Advert", action = "ShowMoreComments" });
                routes.MapRoute("add like", "advert/add-like/{commentId?}", new { controller = "Advert", action = "AddLike" });
                routes.MapRoute("delete like", "advert/delete-like/{commentId?}", new { controller = "Advert", action = "DeleteLike" });
                
                //
                routes.MapRoute("category", "categories/{categoryUrl?}", new { controller = "Category", action = "CategoryPage"  });
                routes.MapRoute("advert", "advert/{id?}", new { controller = "Advert", action = "AdvertPage" });
                routes.MapRoute("add advert", "add-advert", new { controller = "Advert", action = "AddAdvert" });
                routes.MapRoute("edit advert", "edit-advert/{id?}", new { controller = "Advert", action = "EditAdvert" });
                routes.MapRoute("edit advert error", "edit-advert-error/{id?}", new { controller = "Advert", action = "EditAdvertError" });
                
                //default
                routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}