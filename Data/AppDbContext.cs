using EasyManagement.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EasyManagement.API.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor to pass options to the base DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        // Define DbSets for each entity
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomMember> RoomMembers { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }

        // Configure entity relationships and constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Task>(entity =>
            {
                // Tell EF that 'user_id' is the foreign key for 'User'
                entity.HasOne(t => t.User)
                      .WithMany() // or .WithMany(u => u.Tasks) if you added that collection
                      .HasForeignKey(t => t.UserId);

                // Tell EF that 'room_id' is the foreign key for 'Room'
                entity.HasOne(t => t.Room)
                      .WithMany()
                      .HasForeignKey(t => t.RoomId);
            });

            // If room deleted delete all its tasks
            modelBuilder.Entity<Models.Task>()
                .HasOne(t => t.Room)
                .WithMany()
                .HasForeignKey(t => t.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // If room deleted delete all its members
            modelBuilder.Entity<RoomMember>()
                .HasOne(rm => rm.Room)
                .WithMany()
                .HasForeignKey(rm => rm.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
