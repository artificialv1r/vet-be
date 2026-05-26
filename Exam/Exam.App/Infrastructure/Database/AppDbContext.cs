using Exam.App.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<AnimalSpecies> AnimalSpecies { get; set; }
    public DbSet<Vet> Vets { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Assistant> Assistants { get; set; }
    public DbSet<Examination> Examinations { get; set; }
    public DbSet<ExamReport> ExamReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Owner>()
            .HasOne(o => o.User)
            .WithOne(u => u.Owner)
            .HasForeignKey<Owner>(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Owner>()
            .HasIndex(o => o.UserId)
            .IsUnique();
        
        modelBuilder.Entity<Vet>()
            .HasOne(v => v.User)
            .WithOne(u => u.Vet)
            .HasForeignKey<Vet>(v => v.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Vet>()
            .HasIndex(v => v.UserId)
            .IsUnique();

        modelBuilder.Entity<Vet>()
            .HasMany(v => v.Examinations)
            .WithOne(e => e.Vet)
            .HasForeignKey(e => e.VetId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Assistant>()
            .HasOne(a => a.User)
            .WithOne(u => u.Assistant)
            .HasForeignKey<Assistant>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Assistant>()
            .HasIndex(a => a.UserId)
            .IsUnique();
        
        modelBuilder.Entity<Patient>()
            .HasOne(p => p.Vet)
            .WithMany(v => v.Patients)
            .HasForeignKey(p => p.VetId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Patient>()
            .HasOne(p => p.Owner)
            .WithMany(o => o.Pets)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Patient>()
            .HasOne(p => p.AnimalSpecies)
            .WithMany()
            .HasForeignKey(p => p.AnimalSpeciesId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Examinations)
            .WithOne(e => e.Patient)
            .HasForeignKey(e => e.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Examination>()
            .HasOne(e => e.Report)
            .WithOne(er => er.Examination)
            .HasForeignKey<ExamReport>(e => e.ExaminationId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Seed Roles
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "b1c4c3f8-4d2f-4e62-9a4b-8e3a2a9e8a01", Name = "Administrator", NormalizedName = "ADMINISTRATOR"
            },
            new IdentityRole { Id = "1bfe3e46-2f8f-4a9c-bb7b-2f0d8c2e6d02", Name = "User", NormalizedName = "USER" },
            new IdentityRole { Id = "85be95d2-d19e-4c7f-bfb6-b39f6b16b3fe", Name = "Vet", NormalizedName = "VET" },
            new IdentityRole { Id = "532fb9b8-ebed-4d98-b985-b533adeb7601", Name = "Owner", NormalizedName = "OWNER"},
            new IdentityRole { Id = "7abc0c69-b407-4a57-bb31-c62b8ff740e1", Name = "Assistant", NormalizedName = 
                "ASSISTANT"}
    );

    // Seed Entities
        modelBuilder.Entity<AnimalSpecies>(e =>
        {
            e.HasData(
                new AnimalSpecies { Id = 1, Name = "Pas" },
                new AnimalSpecies { Id = 2, Name = "Mačka" },
                new AnimalSpecies { Id = 3, Name = "Papagaj" },
                new AnimalSpecies { Id = 4, Name = "Kornjača" },
                new AnimalSpecies { Id = 5, Name = "Zec" },
                new AnimalSpecies { Id = 6, Name = "Hrčak" }
            );
        });

    }
}
