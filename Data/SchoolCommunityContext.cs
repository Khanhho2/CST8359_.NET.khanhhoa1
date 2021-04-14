using A1.Models;
using Microsoft.EntityFrameworkCore;

namespace A1.Data
{
    public class SchoolCommunityContext : DbContext
    {
        public SchoolCommunityContext(DbContextOptions<SchoolCommunityContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<CommunityMembership> CommunityMemberships { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommunityMembership>().HasKey(i => new { i.StudentID, i.CommunityID });
            modelBuilder.Entity<CommunityMembership>().HasOne(a => a.Student).WithMany(s => s.Membership).HasForeignKey(m => m.StudentID);
            modelBuilder.Entity<CommunityMembership>().HasOne(m => m.Community).WithMany(c => c.Membership).HasForeignKey(m => m.CommunityID);
            modelBuilder.Entity<Community>().HasMany(c => c.Advertisements).WithOne(ad => ad.Community);
            base.OnModelCreating(modelBuilder);
        }
    }
}
