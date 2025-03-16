using System;
using api.Data;
using api.Exceptions;
using api.Interfaces.Repository;
using api.Models.DTOs;
using api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class GuardianRepository(AppDbContext context) : IGuardianRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Guardian> CreateGuardianAsync(Guid studentId, CreateGuardianDTO guardian)
    {
        var student = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == studentId.ToString())
            ?? throw new NotFoundException("Student");

        var newGuardian = new Guardian
        {
            StudentId = student.Id,
            Address = guardian.Address,
            Student = student,
            FirstName = guardian.FirstName,
            LastName = guardian.LastName,
            Email = guardian.Email,
            ContactNumber = guardian.ContactNumber,
            Relationship = guardian.Relationship
        };

        _context.Guardians.Add(newGuardian);
        await _context.SaveChangesAsync(); // Ensure the guardian is saved

        return newGuardian;
    }


    public async Task<bool> DeleteGuardianAsync(Guid studentId)
    {
        var guardian = await _context.Guardians.FirstOrDefaultAsync(g => g.StudentId == studentId.ToString());

        if (guardian != null)
        {
            _context.Guardians.Remove(guardian);
            await _context.SaveChangesAsync();
        }

        return true;
    }

    public async Task<Guardian?> GetGuardianByStudentIdAsync(Guid studentId)
    {
        return await _context.Guardians.FirstOrDefaultAsync(g => g.StudentId == studentId.ToString());
    }

    public async Task<bool> UpdateGuardianAsync(Guid studentId, UpdateGuardianDTO guardian)
    {
        var existingGuardian = await _context.Guardians.FirstOrDefaultAsync(g => g.StudentId == studentId.ToString()) ?? throw new NotFoundException("Guardian");

        existingGuardian.FirstName = guardian.FirstName;
        existingGuardian.LastName = guardian.LastName;
        existingGuardian.Email = guardian.Email;
        existingGuardian.ContactNumber = guardian.ContactNumber;
        existingGuardian.Relationship = guardian.Relationship;
        existingGuardian.Address = guardian.Address;

        _context.Guardians.Update(existingGuardian);
        await _context.SaveChangesAsync();

        return true;
    }
}
