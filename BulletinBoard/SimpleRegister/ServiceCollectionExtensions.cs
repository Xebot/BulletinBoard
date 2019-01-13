using System;
using BulletinDomain.Entities;
using BulletinDomain.RepositoryInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AppServices.Interfaces;
using AppServices.Services;
using BulletinDomain;
using BulletinDomain.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Authentication.AppServices.JwtAuthentication;

namespace SimpleRegister
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterIdentity(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddIdentity<User, IdentityRole<Guid>>().AddEntityFrameworkStores<BulletinDbContext>().AddDefaultTokenProviders();
               
            return serviceCollection;
        }
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, string connectionString)
        {
            //services.AddIdentity<User, IdentityRole<Guid>>().AddEntityFrameworkStores<BulletinDbContext>().AddDefaultTokenProviders();
            services.AddDbContext<BulletinDbContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<IAdvertRepository, AdvertRepository>();
            services.AddTransient<IAdvertService, AdvertService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICategoryRepository,CategoryRepository>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IRegionRepository, RegionRepository>();
            services.AddTransient<IRegionService, RegionService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IJwtTokenService, JwtTokenService>();
            services.AddTransient<IJwtAuthenticationService, JwtAuthenticationService>();
            services.AddTransient<IRegionRepository, RegionRepository>();
            services.AddTransient<IRegionService, RegionService>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<ICommentLikerRepository, CommentLikerRepository>();
            services.AddTransient<ICommentLikerService, CommentLikerService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IEmailSender, EmailSender>();
            
            return services;          
      
    }
    }
}
