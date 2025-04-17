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
    public DbSet<TwoFactorAuth> TwoFactorAuths { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SubjectTeacher>()
            .HasKey(st => new { st.SubjectId, st.TeacherId });

        modelBuilder.Entity<SubjectTeacher>()
            .HasIndex(st => new { st.SubjectId, st.TeacherId })
            .IsUnique(); // Ensures unique subject-teacher pairs

        modelBuilder.Entity<SubjectTeacher>()
            .HasOne(st => st.Subject)
            .WithMany(s => s.SubjectTeachers)
            .HasForeignKey(st => st.SubjectId)
            .OnDelete(DeleteBehavior.Cascade); // Prevent orphan subject-teacher pairs

        modelBuilder.Entity<SubjectTeacher>()
            .HasOne(st => st.Teacher)
            .WithMany(t => t.SubjectTeachers)
            .HasForeignKey(st => st.TeacherId)
            .OnDelete(DeleteBehavior.Cascade); // Prevent orphan subject-teacher pairs


        modelBuilder.Entity<Course>()
            .HasIndex(c => c.Code)
            .IsUnique(); // ✅ Ensures unique course codes
        modelBuilder.Entity<Course>()
            .HasIndex(c => c.Name)
            .IsUnique(); // ✅ Ensures unique course codes
        modelBuilder.Entity<Course>()
            .HasMany(c => c.Sections)
            .WithOne(s => s.Course)
            .HasForeignKey(s => s.CourseId)
            .OnDelete(DeleteBehavior.Cascade); // ✅ Prevent orphan sections    

        modelBuilder.Entity<Subject>()
            .HasKey(s => s.Id); // ✅ Ensures unique subject IDs
        modelBuilder.Entity<Subject>()
            .HasIndex(s => s.Code)
            .IsUnique(); // ✅  Ensures the subject code is unique

        modelBuilder.Entity<Section>()
            .HasMany(s => s.Students)
            .WithOne(s => s.Section)
            .HasForeignKey(s => s.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Section>()
            .HasOne(s => s.Teacher)
            .WithMany() // ✅ Allows multiple sections per teacher
            .HasForeignKey(s => s.TeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ClassSchedule>()
            .HasOne(cs => cs.Section)
            .WithMany(s => s.ClassSchedules)
            .HasForeignKey(cs => cs.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClassSchedule>()
            .HasOne(cs => cs.Subject)
            .WithMany(s => s.ClassSchedules)
            .HasForeignKey(cs => cs.SubjectId)
            .OnDelete(DeleteBehavior.Cascade); // ✅ Prevent orphan schedules

        modelBuilder.Entity<ClassSchedule>()
            .HasOne(cs => cs.Teacher)
            .WithMany(t => t.ClassSchedules)
            .HasForeignKey(cs => cs.TeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Guardian)
            .WithOne(g => g.Student)
            .HasForeignKey<Guardian>(g => g.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AttendanceRecord>()
            .HasOne(ar => ar.ClassSchedule)
            .WithMany()
            .HasForeignKey(ar => ar.ClassScheduleId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<AttendanceRecord>()
            .HasOne(ar => ar.Student)
            .WithMany(s => s.AttendanceRecords)
            .HasForeignKey(ar => ar.StudentId)
            .OnDelete(DeleteBehavior.Cascade); // ✅ Prevent orphan records

        modelBuilder.Entity<User>()
            .HasMany(u => u.ClassSchedules)
            .WithOne(cs => cs.Teacher)
            .HasForeignKey(cs => cs.TeacherId)
            .OnDelete(DeleteBehavior.SetNull); // Prevents data loss when a teacher is removed

        modelBuilder.Entity<User>()
            .HasOne(u => u.Section)
            .WithMany(s => s.Students)
            .HasForeignKey(u => u.SectionId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique(); // Ensures only users have unique emails

        modelBuilder.Entity<TwoFactorAuth>()
            .HasIndex(t => t.Email)
            .IsUnique(); // Ensures only one active 2FA code per user
    }

}

