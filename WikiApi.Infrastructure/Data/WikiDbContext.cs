using Microsoft.EntityFrameworkCore;
using WikiApi.Domain.Entities;

namespace WikiApi.Infrastructure.Data;

public class WikiDbContext : DbContext
{
    public WikiDbContext(DbContextOptions<WikiDbContext> options) : base(options) { }

    public DbSet<Article> Articles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(builder =>
        {
            builder.HasKey(article => article.Id);
            builder.Property(article => article.Title).IsRequired();
            builder.Property(article => article.Content).IsRequired();
            builder.Property(article => article.Tags);
            builder.Property(article => article.Category);
            builder.Property(article => article.CreatedAt).HasDefaultValueSql("now()");
        });
    }
}