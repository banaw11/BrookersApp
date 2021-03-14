using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, 
        AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        
        public DataContext( DbContextOptions options) : base(options)
        {
        }

        public DbSet<Friend> Friends { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UnreadMessage> UnreadMessages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppUser>()
                .HasMany(f => f.Friends)
                .WithOne(u => u.User)
                .HasForeignKey(ui => ui.UserId);

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<Friend>()
                .HasKey(f => f.Id);

            builder.Entity<Message>()
               .HasKey(k => k.Id);

            builder.Entity<Message>()
                .HasOne(x => x.Sender)
                .WithMany(x => x.MessagesSent)
                .HasForeignKey(x => x.SenderId);

            builder.Entity<Message>()
                .HasOne(x => x.Receiver)
                .WithMany(x => x.MessagesReceived)
                .HasForeignKey(x => x.ReceiverId);

            builder.Entity<Connection>()
                .HasKey(x => x.Id);

            builder.Entity<Connection>()
                 .HasOne(x => x.User)
                 .WithMany(x => x.Connections)
                 .HasForeignKey(x => x.UserId);

            builder.Entity<Notification>()
                .HasKey(x => x.Id);

            builder.Entity<Notification>()
                .HasOne(x => x.User)
                .WithOne(x => x.Notification)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Entity<Notification>()
                .HasMany(x => x.UnreadMessages)
                .WithOne(x => x.Notification)
                .HasForeignKey(x => x.NotificationId);

        }
    }
}
