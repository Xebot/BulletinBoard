using System;
using BulletinDomain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BulletinDomain


{

    public class BulletinDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<IQuarable> Categories { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentLiker> CommentLikers { get; set; }
        public BulletinDbContext(DbContextOptions<BulletinDbContext> options) : base(options)
        {
        }


        //public BulletinDbContext()
        //{
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    //var ConnectionString = "Server=HOMEPC\\SQLEXPRESS; Database=BulletinBoard; user id=bulletinUser;password=bulletinUser;MultipleActiveResultSets=True";
        //    var ConnectionString = "Server=MACPC\\SQLEXPRESS; Database=BulletinBoard; user id=bulletinUser; password=bulletinUser; MultipleActiveResultSets=True";

        //    //var ConnectionString = "Server=MACPC\\SQLEXPRESS; Database=BulletinBoard; user id=bulletinUser;password=bulletinUser;MultipleActiveResultSets=True";


        //    optionsBuilder.UseSqlServer(ConnectionString);
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Users");
                entity.Ignore(user => user.TwoFactorEnabled);
                entity.Ignore(user => user.ConcurrencyStamp);
                entity.Ignore(user => user.LockoutEnabled);
                entity.Ignore(user => user.LockoutEnd);
                
            });

            builder.Entity<IdentityRole<Guid>>(entity =>
            {
                entity.ToTable(name: "AspNetRoles");
            });

            // ВАЖНО!!!! Без данной конфигурации не происходит обновление объявления (ошибка обновления поля AdvertNumber со свойством Identity)
            var scenarioBuilder = builder.Entity<Advert>();
            scenarioBuilder.Property(p => p.AdvertNumber).UseSqlServerIdentityColumn();
            scenarioBuilder.Property(p => p.AdvertNumber).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
            // Конец ВАЖНО!!!!
            
            //builder.Entity<Role>(entity =>
            //{
            //    entity.ToTable(name: "Roles");
            //});

            //builder.Entity<UserRole>(entity =>
            //{
            //    entity.ToTable(name: "UserRoles");
            //});

        }
    }


    //public class BulletinDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    ////DbContext
    //{
    //    //public DbSet<Advert> Advert { get; set; }

    //    // public DbSet<Comments> AdvertComments { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {

    //        var ConnectionString = "Server=MACPC\\SQLEXPRESS; Database=BulletinBoard; user id=bulletinUser;password=bulletinUser;MultipleActiveResultSets=True";
    //        //var ConnectionString = "Server=DESKTOP-MVOUBKL\\SQLEXPRESS; Database=BulletinBoard; user id=bulletinUser; password=bulletinUser; MultipleActiveResultSets=True";

    //        optionsBuilder.UseSqlServer(ConnectionString);
    //    }
    //} 
}
