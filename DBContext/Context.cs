using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedHardware.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace SharedHardware.Data
{
    public class Context : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Computation> Computations { get; set; }
        public DbSet<ComputationDeployment> ComputationDeployments { get; set; }
        public DbSet<ComputationSubscription> ComputationSubscriptions { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Credit> Credits { get; set; }
        public DbSet<Geolocation> Geo { get; set; }
        public DbSet<NotificationLog> NotificationLog { get; set; }
        public DbSet<Platform> Platform { get; set; }
        public DbSet<PlatformEventLog> PlatformEventLog { get; set; }
        public DbSet<PlatformOutage> PlatformOutages { get; set; }
        public DbSet<PlatformRequest> PlatformRequests { get; set; }
        public DbSet<PlatformSubscription> PlatformSubscriptions { get; set; }
        public DbSet<PlatformTag> PlatformTags { get; set; }
        public DbSet<PlatformType> PlatformTypes { get; set; }
        public DbSet<Run> Runs { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<SharedResource> SharedResources { get; set; }
        public DbSet<SystemDeployment> SystemDeployments { get; set; }
        public Context(DbContextOptions<DbContext> options)
            : base(options) {}
        public Context(DbContextOptions<Context> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);


            builder.Entity<User>().ToTable("User");
            builder.Entity<Role>().ToTable("Role");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaim");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaim");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserToken");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogin");

            builder.Entity<AccountConfirmationCode>()
                .Property(c => c.Code)
                .IsRequired()
                .HasColumnType("varchar(10)");
            builder.Entity<AccountConfirmationCode>()
                .Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(255);
            builder.Entity<AccountConfirmationCode>()
                .Property(c => c.ExpirationDate)
                .IsConcurrencyToken();
            builder.Entity<AccountConfirmationCode>()
                .Property(c => c.Password)
                .HasMaxLength(50)
                .IsRequired();

            builder.Entity<Computation>()
                .Property(c => c.BundleUrl)
                .IsRequired()
                .HasMaxLength(512);
            builder.Entity<Computation>()
                .Property(c => c.UpdateDate)
                .IsConcurrencyToken();
            builder.Entity<Computation>()
                .Property(c => c.EntryPoint)
                .IsRequired()
                .HasMaxLength(512);
            builder.Entity<Computation>()
                .HasOne(c => c.LastDeployment)
                .WithOne()
                .IsRequired(false)
                .HasForeignKey<Computation>(c => c.LastDeploymentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Contact>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Entity<Contact>()
                .Property(c => c.Address)
                .IsRequired()
                .HasMaxLength(255);
            builder.Entity<Contact>()
                .Property(c => c.UpdateDate)
                .IsConcurrencyToken();

            builder.Entity<Credit>()
                .HasOne(c => c.ResourceOwner)
                .WithMany(c => c.ToReceiveCredits)
                .HasForeignKey(c => c.ResourceOwnerId);
            //.OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Credit>()
                .HasOne(c => c.ResourceUser)
                .WithMany(c => c.ToPayCredits)
                .HasForeignKey(c => c.ResourceUserId);
                //.OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Geolocation>()
                .Property(c => c.Country)
                .IsRequired()
                .HasColumnType("char(2)"); //ISO 2
            builder.Entity<Geolocation>()
                .Property(c => c.State)
                .IsRequired(false)
                .HasMaxLength(30);
            builder.Entity<Geolocation>()
                .Property(c => c.City)
                .HasMaxLength(100);

            builder.Entity<NotificationLog>()
                .Property(c => c.Subject).HasMaxLength(200);
            builder.Entity<NotificationLog>()
                .Property(c => c.Text).IsRequired();

            builder.Entity<PlatformToPlatformTag>()
                .HasKey(c => new { c.PlatformId, c.PlatformTagId });
            builder.Entity<PlatformToPlatformTag>()
                .HasOne(c => c.Platform)
                .WithMany(c => c.PlatformTags)
                .HasForeignKey(c => c.PlatformId);
            builder.Entity<PlatformToPlatformTag>()
                .HasOne(c => c.PlatformTag)
                .WithMany(c => c.PlatformTags)
                .HasForeignKey(c => c.PlatformTagId);

            builder.Entity<Platform>()
                .Property(c => c.UpdateDate).IsConcurrencyToken();
            builder.Entity<Platform>()
                .Property(c => c.Comment).HasMaxLength(255);
            builder.Entity<Platform>()
                .Property(c => c.DetectedOsVersion).IsRequired().HasMaxLength(100);
            builder.Entity<Platform>()
                .Property(c => c.PublicIP).HasColumnType("varchar(39)");
            builder.Entity<Platform>()
                .HasOne(c => c.LastOutage)
                .WithOne()
                .HasForeignKey<Platform>(c => new { c.Id, c.LastOutageId })
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<PlatformEventLog>()
                .HasKey(l => new { l.PlatformId, l.EventId });

            builder.Entity<PlatformRequest>()
                .Property(pr => pr.Comment).HasMaxLength(255);
            builder.Entity<PlatformRequest>()
                .Property(pr => pr.UpdateDate).IsConcurrencyToken();

            builder.Entity<PlatformTag>()
                .Property(pr => pr.Name).IsRequired().HasMaxLength(100);
            builder.Entity<PlatformTag>()
                .Property(pr => pr.Description).HasMaxLength(255);

            builder.Entity<PlatformType>()
                .Property(pr => pr.Name).IsRequired().HasMaxLength(100);
            builder.Entity<PlatformType>()
                .Property(pr => pr.LogoUrl).HasMaxLength(255);

            builder.Entity<Schedule>()
                .Property(pr => pr.CronSchedule).HasColumnType("varchar(100)");

            builder.Entity<SharedResource>()
                .Property(pr => pr.UpdateDate).IsConcurrencyToken();

            builder.Entity<SharedResource>()
                .HasOne(r => r.AvailabilityTime)
                .WithMany()
                .HasForeignKey(c => c.AvailabilityTimeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<PlatformOutage>()
                .HasKey(c => new { c.PlatformId, c.OutageId });

            builder.Entity<PlatformSubscription>()
                .HasKey(c => new { c.PlatformId, c.ContactId, c.Type });

            builder.Entity<ComputationSubscription>()
                .HasKey(c => new { c.ComputationId, c.ContactId, c.Type });

            builder.Entity<Run>()
                .HasOne(c => c.SharedResource)
                .WithMany()
                .HasForeignKey(c => c.SharedResourceId);
            //.OnDelete(DeleteBehavior.Restrict);

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                if (relationship.DeleteBehavior == DeleteBehavior.Cascade)
                    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }
    }
}
