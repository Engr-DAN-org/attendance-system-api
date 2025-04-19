using System;
using api.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<Guardian> Guardians { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<SubjectTeacher> SubjectTeachers { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<ClassSchedule> ClassSchedules { get; set; }
    public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
    public DbSet<TwoFactorAuth> TwoFactorAuths { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // === User Config ===
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasOne(u => u.Guardian)
            .WithOne(g => g.Student)
            .HasForeignKey<Guardian>(g => g.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Section)
            .WithMany(s => s.Students)
            .HasForeignKey(u => u.SectionId)
            .OnDelete(DeleteBehavior.SetNull);

        // === Guardian Config ===
        modelBuilder.Entity<Guardian>()
            .HasKey(g => g.Id);

        // === Course Config ===
        modelBuilder.Entity<Course>()
            .HasIndex(c => c.Code)
            .IsUnique();

        modelBuilder.Entity<Course>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<Course>()
            .HasMany(c => c.Sections)
            .WithOne(s => s.Course)
            .HasForeignKey(s => s.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // === Section Config ===
        modelBuilder.Entity<Section>()
            .HasMany(s => s.Students)
            .WithOne(s => s.Section)
            .HasForeignKey(s => s.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Section>()
            .HasOne(s => s.Teacher)
            .WithMany()
            .HasForeignKey(s => s.TeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Section>()
            .HasMany(s => s.ClassSchedules)
            .WithOne(cs => cs.Section)
            .HasForeignKey(cs => cs.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        // === Subject Config ===
        modelBuilder.Entity<Subject>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<Subject>()
            .HasIndex(s => s.Code)
            .IsUnique();

        modelBuilder.Entity<Subject>()
            .HasIndex(s => s.Name)
            .IsUnique();

        modelBuilder.Entity<Subject>()
            .HasMany(s => s.SubjectTeachers)
            .WithOne(st => st.Subject)
            .HasForeignKey(st => st.SubjectId);

        // === SubjectTeacher Config ===
        modelBuilder.Entity<SubjectTeacher>()
            .HasKey(st => st.Id);

        modelBuilder.Entity<SubjectTeacher>()
            .HasIndex(st => new { st.SubjectId, st.TeacherId })
            .IsUnique();

        modelBuilder.Entity<SubjectTeacher>()
            .HasOne(st => st.Teacher)
            .WithMany(t => t.SubjectTeachers)
            .HasForeignKey(st => st.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        // === ClassSchedule Config ===
        modelBuilder.Entity<ClassSchedule>()
            .HasIndex(cs => new { cs.SectionId, cs.SubjectTeacherId, cs.StartTime, cs.EndTime })
            .IsUnique();

        modelBuilder.Entity<ClassSchedule>()
            .HasOne(cs => cs.SubjectTeacher)
            .WithMany(st => st.ClassSchedules)
            .HasForeignKey(cs => cs.SubjectTeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        // === AttendanceRecord Config ===
        modelBuilder.Entity<AttendanceRecord>()
            .HasOne(ar => ar.ClassSchedule)
            .WithMany()
            .HasForeignKey(ar => ar.ClassScheduleId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<AttendanceRecord>()
            .HasOne(ar => ar.Student)
            .WithMany(s => s.AttendanceRecords)
            .HasForeignKey(ar => ar.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // === TwoFactorAuth Config ===
        modelBuilder.Entity<TwoFactorAuth>()
            .HasIndex(t => t.Email)
            .IsUnique();
    }
}
