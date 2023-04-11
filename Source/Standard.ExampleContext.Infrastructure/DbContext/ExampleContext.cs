using Microsoft.EntityFrameworkCore;
using Standard.ExampleContext.Domain.DbContext;
using Standard.ExampleContext.Domain.Entities;

namespace Standard.ExampleContext.Infrastructure.DbContext;

public class ExampleContext : Microsoft.EntityFrameworkCore.DbContext, IExampleContext
{
    public ExampleContext(DbContextOptions<ExampleContext> options) : base(options)
    {
    }

    public DbSet<Example> Examples { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ExampleModelBuilder(modelBuilder);
    }

    private void ExampleModelBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Example>().ToTable("Example");

        modelBuilder.Entity<Example>()
            .Property(x => x.Email)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Example>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<Example>()
            .Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Example>()
            .Property(x => x.Password)
            .HasMaxLength(2000)
            .IsRequired();

        modelBuilder.Entity<Example>()
            .Property(x => x.Surname)
            .HasMaxLength(100)
            .IsRequired();
    }
}