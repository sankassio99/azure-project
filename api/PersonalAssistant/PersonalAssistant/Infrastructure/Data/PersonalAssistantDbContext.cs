using Microsoft.EntityFrameworkCore;
using PersonalAssistant.Domain.Entities;

namespace PersonalAssistant.Infrastructure.Data;

public class PersonalAssistantDbContext(DbContextOptions<PersonalAssistantDbContext> options) : DbContext(options)
{
    public DbSet<Expense> Expenses => Set<Expense>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Value).HasColumnType("decimal(18,2)");
        });
    }
}
