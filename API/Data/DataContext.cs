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


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<Friend>()
                .HasKey(f => new {f.UserId, f.FriendId });


            builder.Entity<Friend>()
                .HasOne(x => x.User)
                .WithMany(x => x.FriendsAccepted)
                .HasForeignKey(x => x.UserId);

            builder.Entity<Friend>()
                .HasOne(x => x.FriendUser)
                .WithMany(x => x.FriendsInvited)
                .HasForeignKey(x => x.FriendId);

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
                .HasKey(x => x.ConnectionId);

            builder.Entity<Connection>()
                 .HasOne(x => x.User)
                 .WithMany(x => x.Connections)
                 .HasForeignKey(x => x.UserId);

        }
    }
}
