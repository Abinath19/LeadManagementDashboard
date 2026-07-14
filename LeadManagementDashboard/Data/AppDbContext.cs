using LeadManagementDashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace LeadManagementDashboard.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Status> Statuses => Set<Status>();
        public DbSet<Lead> Leads => Set<Lead>();
        public DbSet<LeadActivity> LeadActivities => Set<LeadActivity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---- Statuses ----
            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(s => s.Name).HasMaxLength(50).IsRequired();
                entity.Property(s => s.ColorCode).HasMaxLength(20).IsRequired();
            });

            // ---- Leads ----
            modelBuilder.Entity<Lead>(entity =>
            {
                entity.Property(l => l.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(l => l.LastName).HasMaxLength(100).IsRequired();
                entity.Property(l => l.Email).HasMaxLength(200).IsRequired();
                entity.Property(l => l.Phone).HasMaxLength(30);
                entity.Property(l => l.Company).HasMaxLength(150);

                // Lead -> Status: many leads per status
                entity.HasOne(l => l.Status)
                      .WithMany(s => s.Leads)
                      .HasForeignKey(l => l.StatusId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ---- LeadActivities ----
            modelBuilder.Entity<LeadActivity>(entity =>
            {
                entity.Property(a => a.Note).HasMaxLength(500);

                entity.HasOne(a => a.Lead)
                      .WithMany(l => l.Activities)
                      .HasForeignKey(a => a.LeadId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.FromStatus)
                      .WithMany()                         
                      .HasForeignKey(a => a.FromStatusId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.ToStatus)
                      .WithMany()
                      .HasForeignKey(a => a.ToStatusId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            Seed(modelBuilder);
        }

        private static void Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2026, 07, 01, 9, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "New", DisplayOrder = 1, ColorCode = "#0d6efd", IsActive = true },
                new Status { Id = 2, Name = "Contacted", DisplayOrder = 2, ColorCode = "#ffc107", IsActive = true },
                new Status { Id = 3, Name = "Qualified", DisplayOrder = 3, ColorCode = "#198754", IsActive = true },
                new Status { Id = 4, Name = "Closed", DisplayOrder = 4, ColorCode = "#6c757d", IsActive = true }
            );

            modelBuilder.Entity<Lead>().HasData(
                new Lead { Id = 1, FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com", Phone = "+1-555-0101", Company = "Acme Corp", StatusId = 1, CreatedAt = seedDate },
                new Lead { Id = 2, FirstName = "Bob", LastName = "Martinez", Email = "bob.martinez@example.com", Phone = "+1-555-0102", Company = "Globex Ltd", StatusId = 2, CreatedAt = seedDate.AddDays(1) },
                new Lead { Id = 3, FirstName = "Carla", LastName = "Nguyen", Email = "carla.nguyen@example.com", Phone = "+1-555-0103", Company = "Initech", StatusId = 3, CreatedAt = seedDate.AddDays(2) },
                new Lead { Id = 4, FirstName = "David", LastName = "Okafor", Email = "david.okafor@example.com", Phone = "+1-555-0104", Company = "Umbrella Inc", StatusId = 4, CreatedAt = seedDate.AddDays(3) },
                new Lead { Id = 5, FirstName = "Elena", LastName = "Petrova", Email = "elena.petrova@example.com", Phone = "+1-555-0105", Company = "Stark Industries", StatusId = 1, CreatedAt = seedDate.AddDays(4) }
            );

            modelBuilder.Entity<LeadActivity>().HasData(
                new LeadActivity { Id = 1, LeadId = 2, FromStatusId = null, ToStatusId = 1, ChangedAt = seedDate.AddDays(1), Note = "Lead created" },
                new LeadActivity { Id = 2, LeadId = 2, FromStatusId = 1, ToStatusId = 2, ChangedAt = seedDate.AddDays(2), Note = "Status changed by user" },

                new LeadActivity { Id = 3, LeadId = 3, FromStatusId = null, ToStatusId = 1, ChangedAt = seedDate.AddDays(2), Note = "Lead created" },
                new LeadActivity { Id = 4, LeadId = 3, FromStatusId = 1, ToStatusId = 2, ChangedAt = seedDate.AddDays(3), Note = "Status changed by user" },
                new LeadActivity { Id = 5, LeadId = 3, FromStatusId = 2, ToStatusId = 3, ChangedAt = seedDate.AddDays(4), Note = "Status changed by user" }
            );
        }
    }
}
