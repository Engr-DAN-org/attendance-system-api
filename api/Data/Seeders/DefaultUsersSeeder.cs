using System;
using api.Enums;
using api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Seeders;

public class DefaultUsersSeeder(AppDbContext context, ILogger<DefaultUsersSeeder> logger)
{
    private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ILogger<DefaultUsersSeeder> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task SeedAsync()
    {
        var usersToSeed = new List<User>
        {
            new() {
                Email = "attendancesystemtestnoreply@gmail.com",
                IdNumber = 999.ToString(),
                FirstName = "Admin",
                LastName = "Account",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), // Hashed password
                UserRole = UserRole.Admin
            },

            new() {
                Email = "arludave23@gmail.com",
                IdNumber = 3200385.ToString(),
                FirstName = "Dave",
                LastName = "Teacher",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                UserRole = UserRole.Teacher
            },

            new() {
                Email = "arludave28@gmail.com",
                IdNumber = 3200386.ToString(),
                FirstName = "Dave Arlu",
                LastName = "Student",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                UserRole = UserRole.Student
            }
        };

        // ✅ Check existence in the database dynamically instead of loading all users
        foreach (var user in usersToSeed)
        {
            bool userExists = await _context.Users.AnyAsync(u => u.Email == user.Email);
            if (!userExists)
            {
                _context.Users.Add(user);
                _logger.LogInformation("User {Email} added.", user.Email);
            }
        }

        // ✅ Ensure student exists before adding a guardian
        var existingStudent = await _context.Users.FirstOrDefaultAsync(st => st.UserRole == UserRole.Student);
        if (existingStudent != null)
        {
            var existingGuardian = await _context.Guardians.FirstOrDefaultAsync(g => g.StudentId == existingStudent.Id);
            if (existingGuardian == null)
            {
                bool guardianExists = await _context.Guardians.AnyAsync(g => g.Email == "arludave23@gmail.com");
                if (!guardianExists)
                {
                    Guardian guardianToSeed = new()
                    {
                        StudentId = existingStudent.Id,
                        FirstName = "Test Guardian",
                        LastName = "LastName",
                        Email = "arludave23@gmail.com",
                        Address = "Danao City",
                        Student = existingStudent
                    };
                    existingStudent.Guardian = guardianToSeed;
                    existingStudent.GuardianId = guardianToSeed.Id;


                    _context.Guardians.Add(guardianToSeed);
                    _logger.LogInformation("Guardian {FirstName} added.", guardianToSeed.FirstName);
                }
            }
        }

        await _context.SaveChangesAsync();
    }
}
