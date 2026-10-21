using System.IO;
using Microsoft.EntityFrameworkCore;
using WebLab.RTR.Models;

namespace WebLab.RTR.Services;

public class AppDbContext : DbContext
{
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<GlossaryItem> GlossaryItems => Set<GlossaryItem>();
    public DbSet<QuizQuestion> QuizQuestions => Set<QuizQuestion>();

    public static string DatabaseFilePath
    {
        get
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var folder = Path.Combine(appData, "WebLabRTR");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return Path.Combine(folder, "weblab.db");
        }
    }

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite($"Data Source={DatabaseFilePath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).IsRequired().HasMaxLength(250);
            entity.Property(t => t.Category).IsRequired().HasMaxLength(100);
            entity.Property(t => t.Description).HasMaxLength(1000);
            entity.HasIndex(t => t.OrderNumber);
            entity.HasIndex(t => t.Category);
        });

        modelBuilder.Entity<GlossaryItem>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Term).IsRequired().HasMaxLength(150);
            entity.Property(g => g.Category).HasMaxLength(100);
            entity.HasIndex(g => g.Term);
        });

        modelBuilder.Entity<QuizQuestion>(entity =>
        {
            entity.HasKey(q => q.Id);
            entity.Property(q => q.QuestionText).IsRequired().HasMaxLength(500);
            entity.HasIndex(q => q.TopicId);
        });
    }
}
