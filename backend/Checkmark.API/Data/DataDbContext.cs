using Microsoft.EntityFrameworkCore;
using Checkmark.API.Models;


namespace Checkmark.API.Data
{
  public class DataDbContext : DbContext
  {
    public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
    {

    }
    //DbSet = tabela no banco
    public DbSet<CheckmarkItem> CheckmarkItems { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    //enum para string no db
      modelBuilder.Entity<CheckmarkItem>()
        .Property(i => i.Priority)
        .HasConversion<string>();
    }
  
  }
}