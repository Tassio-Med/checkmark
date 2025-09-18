using Microsoft.EntityFrameworkCore;
using Checkmark.API.Models;

namespace Checkmark.API.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<CheckmarkItem> CheckmarkItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CheckmarkItem>(entity =>
            {
                entity.ToTable("CheckmarkItems");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                    
                entity.Property(e => e.Description)
                    .IsRequired();
                    
                entity.Property(e => e.IsCompleted)
                    .IsRequired();
                    
                entity.Property(e => e.DueDate)
                    .IsRequired(false);
                    
                entity.Property(e => e.Priority)
                    .HasConversion<string>()
                    .IsRequired();
                    
                entity.Property(e => e.CreatedAt)
                    .IsRequired();
                    
                entity.Property(e => e.UpdatedAt)
                    .IsRequired(false);
            });
        }
    }
}