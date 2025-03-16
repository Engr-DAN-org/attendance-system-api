using System;
using api.Data;
using api.Enums;
using api.Interfaces.Repository;
using api.Models.DTOs;
using api.Models.Entities;
using api.Utils;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public Task<User> CreateUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> DeleteUserAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<GetStudentDTO>> GetStudentsAsync(StudentQueryDTO studentQueryDTO)
    {
        var query = _context.Users.AsQueryable()
            .Where(u => u.UserRole == studentQueryDTO.UserRole);

        if (!string.IsNullOrWhiteSpace(studentQueryDTO.Name))
        {
            query = query.Where(u => u.FullName.Contains(studentQueryDTO.Name));
        }
        if (!string.IsNullOrWhiteSpace(studentQueryDTO.IdNumber))
        {
            query = query.Where(u => u.IdNumber.Contains(studentQueryDTO.IdNumber));
        }
        if (!string.IsNullOrWhiteSpace(studentQueryDTO.SectionId))
        {
            if (int.TryParse(studentQueryDTO.SectionId, out int sectionId))
            {
                query = query.Where(u => u.SectionId == sectionId);
            }
        }
        if (!string.IsNullOrWhiteSpace(studentQueryDTO.GuardianName))
        {
            query = query.Where(u => u.Guardian != null && u.Guardian.FullName.Contains(studentQueryDTO.GuardianName));
        }
        if (studentQueryDTO.YearLevel != null)
        {
            query = query.Where(u => u.Section != null && u.Section.YearLevel == studentQueryDTO.YearLevel);
        }
        return await query
        .Skip((studentQueryDTO.Page - 1) * studentQueryDTO.PageSize)
        .Take(studentQueryDTO.PageSize)
        .Select(u => TransformUserInfoUtils.GetStudentInfo(u))
        .ToListAsync();
    }

    public async Task<User?> GetUserByEmailOrIdNoAsync(string emailOrIdNo)
    {
        return await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == emailOrIdNo || u.Id == emailOrIdNo);
    }


    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public Task<User> UpdateUserAsync(User user)
    {
        throw new NotImplementedException();
    }
}
