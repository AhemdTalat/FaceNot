using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserLike>()
                .HasKey(userLike => new { userLike.SourceUserId, userLike.LikedUserId });

            modelBuilder.Entity<UserLike>()
                .HasOne(userLike => userLike.SourceUser)
                .WithMany(appUser => appUser.LikedUsers)
                .HasForeignKey(userLike => userLike.SourceUserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<UserLike>()

                .HasOne(userLike => userLike.LikedUser)
                .WithMany(appUser => appUser.LikedByUsers)
                .HasForeignKey(userLike => userLike.LikedUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}