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
        public DbSet<User> users { get; set; }
        public DbSet<Room> rooms { get; set; }
        public DbSet<RoomMember> roomMembers { get; set; }
        public DbSet<Models.Task> tasks { get; set; }

        // Configure entity relationships and constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Task>(entity =>
            {
                // Tell EF that 'user_id' is the foreign key for 'User'
                entity.HasOne(t => t.User)
                      .WithMany() // or .WithMany(u => u.Tasks) if you added that collection
                      .HasForeignKey(t => t.user_id);

                // Tell EF that 'room_id' is the foreign key for 'Room'
                entity.HasOne(t => t.Room)
                      .WithMany()
                      .HasForeignKey(t => t.room_id);
            });

            // If room deleted delete all its tasks
            modelBuilder.Entity<Models.Task>()
                .HasOne(t => t.Room)
                .WithMany()
                .HasForeignKey(t => t.room_id)
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
