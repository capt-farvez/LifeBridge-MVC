using Microsoft.EntityFrameworkCore;

namespace LifeBridge.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<RequestBloodDonation> BloodDonationRequests { get; set; }
        public DbSet<BloodDonationRecord> BloodDonationRecords { get; set; }
        public DbSet<RequestOrganDonation> OrganDonationRequests { get; set; }
        public DbSet<OrganDonationRecord> OrganDonationRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User - Blood Donation Requests (1-to-many)
            modelBuilder.Entity<RequestBloodDonation>()
                .HasOne(r => r.User)
                .WithMany(u => u.BloodDonationRequests)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - Blood Donation Records (1-to-many)
            modelBuilder.Entity<BloodDonationRecord>()
                .HasOne(r => r.User)
                .WithMany(u => u.BloodDonationRecords)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - Organ Donation Requests (1-to-many)
            modelBuilder.Entity<RequestOrganDonation>()
                .HasOne(r => r.User)
                .WithMany(u => u.OrganDonationRequests)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - Organ Donation Records (1-to-many)
            modelBuilder.Entity<OrganDonationRecord>()
                .HasOne(r => r.User)
                .WithMany(u => u.OrganDonationRecords)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}