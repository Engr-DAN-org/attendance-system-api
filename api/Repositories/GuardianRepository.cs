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

    public async Task<Guardian> CreateGuardianAsync(User student, CreateGuardianDTO createGuardianDTO)
    {
        var guardian = _context.Guardians.Add(new Guardian
        {
            StudentId = student.Id,
            Address = createGuardianDTO.Address,
            Student = student,
            FirstName = createGuardianDTO.FirstName,
            LastName = createGuardianDTO.LastName,
            Email = createGuardianDTO.Email,
            ContactNumber = createGuardianDTO.ContactNumber,
            Relationship = createGuardianDTO.Relationship
        });
        student.GuardianId = guardian.Entity.Id;
        student.Guardian = guardian.Entity;
        await _context.SaveChangesAsync(); // Ensure the guardian is saved
        return guardian.Entity;
    }


    public async Task<bool> DeleteGuardianAsync(string studentId)
    {
        var guardian = await _context.Guardians.FirstOrDefaultAsync(g => g.StudentId == studentId);

        if (guardian != null)
        {
            _context.Guardians.Remove(guardian);
            await _context.SaveChangesAsync();
        }

        return true;
    }

    public async Task<Guardian?> GetGuardianByStudentIdAsync(string studentId)
    {
        return await _context.Guardians.FirstOrDefaultAsync(g => g.StudentId == studentId);
    }

    public async Task<Guardian> UpdateGuardianAsync(User student, CreateGuardianDTO guardian)
    {
        var existingGuardian = await _context.Guardians.FirstOrDefaultAsync(g => g.StudentId == student.Id);

        if (existingGuardian == null)
        {
            existingGuardian = await CreateGuardianAsync(student, guardian);
        }
        else
        {
            existingGuardian.FirstName = guardian.FirstName;
            existingGuardian.LastName = guardian.LastName;
            existingGuardian.Email = guardian.Email;
            existingGuardian.ContactNumber = guardian.ContactNumber;
            existingGuardian.Relationship = (Enums.StudentGuardianRelationship)guardian.Relationship;
            existingGuardian.Address = guardian.Address;

            _context.Guardians.Update(existingGuardian);
        }

        await _context.SaveChangesAsync();

        return existingGuardian;
    }
}
