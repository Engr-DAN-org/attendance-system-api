using System;
using api.Data;
using api.Enums;
using api.Exceptions;
using api.Interfaces.Repository;
using api.Models.DTOs;
using api.Models.Entities;
using api.Utils;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<User> CreateUserAsync(User user)
    {
        var createdUser = await _context.Users.AddAsync(new User
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            IdNumber = user.IdNumber,
            UserRole = user.UserRole,
            SectionId = user.SectionId,
        });
        await _context.SaveChangesAsync();
        return createdUser.Entity;
    }

    public async Task<User> DeleteUserAsync(string id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == id) ?? throw new NotFoundException("User");
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> FindFirstAdmin()
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserRole == UserRole.Admin);
    }

    public async Task<User?> FindByEmailOrIdNoAsync(string emailOrIdNo)
    {
        return await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == emailOrIdNo || u.IdNumber == emailOrIdNo);
    }

    public async Task<User?> FindByIdAsync(string id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == id);
    }


    public async Task<User?> FindBySchoolIdNoAsync(string schoolId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.IdNumber == schoolId);
    }

    public async Task<List<T>> GetUsersAsync<T>(
        UserRole userRole,
        int page,
        Action<IQueryable<User>>? queryCallback = null,
        Func<IQueryable<User>, IQueryable<T>>? selectCallback = null)
    {
        var pageSize = 10;
        var query = _context.Users.AsQueryable()
            .Where(u => u.UserRole == userRole);

        queryCallback?.Invoke(query);

        // Apply selection (convert to DTOs)
        var resultQuery = selectCallback != null ? selectCallback(query) : query.Select(u => (T)(object)u);

        return await resultQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }


    public async Task<User> UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    // private readonly IGuardianRepository _guardianRepository = guardianRepository ?? throw new ArgumentNullException(nameof(guardianRepository));
    // public async Task<GetStudentDTO> CreateStudentAsync(CreateStudentDTO studentDTO)
    // {
    //     using var transaction = await _context.Database.BeginTransactionAsync();
    //     try
    //     {
    //         var createdStudent = _context.Users.Add(new User
    //         {
    //             FirstName = studentDTO.FirstName,
    //             LastName = studentDTO.LastName,
    //             Email = studentDTO.Email,
    //             IdNumber = studentDTO.IdNumber,
    //             UserRole = studentDTO.UserRole,
    //             SectionId = studentDTO.SectionId,
    //         });
    //         await _context.SaveChangesAsync();

    //         var guardian = _guardianRepository.CreateGuardianAsync(createdStudent.Entity, studentDTO.Guardian);
    //         createdStudent.Entity.GuardianId = guardian.Id;
    //         createdStudent.Entity.Guardian = guardian;
    //         await _context.SaveChangesAsync();
    //         await transaction.CommitAsync();

    //         return TransformUserInfoUtils.GetStudentInfo(createdStudent.Entity);
    //     }
    //     catch (Exception)
    //     {
    //         await transaction.RollbackAsync();
    //         throw;
    //     }
    // }

    // public async Task<List<GetStudentDTO>> GetStudentsAsync(StudentQueryDTO studentQueryDTO)
    // {
    //     var query = _context.Users.AsQueryable()
    //         .Where(u => u.UserRole == studentQueryDTO.UserRole);

    //     if (!string.IsNullOrWhiteSpace(studentQueryDTO.Name))
    //     {
    //         query = query.Where(u => u.FullName.Contains(studentQueryDTO.Name));
    //     }
    //     if (!string.IsNullOrWhiteSpace(studentQueryDTO.IdNumber))
    //     {
    //         query = query.Where(u => u.IdNumber.Contains(studentQueryDTO.IdNumber));
    //     }
    //     if (!string.IsNullOrWhiteSpace(studentQueryDTO.SectionId))
    //     {
    //         if (int.TryParse(studentQueryDTO.SectionId, out int sectionId))
    //         {
    //             query = query.Where(u => u.SectionId == sectionId);
    //         }
    //     }
    //     if (!string.IsNullOrWhiteSpace(studentQueryDTO.GuardianName))
    //     {
    //         query = query.Where(u => u.Guardian != null && u.Guardian.FullName.Contains(studentQueryDTO.GuardianName));
    //     }
    //     if (studentQueryDTO.YearLevel != null)
    //     {
    //         query = query.Where(u => u.Section != null && u.Section.YearLevel == studentQueryDTO.YearLevel);
    //     }
    //     return await query
    //     .Skip((studentQueryDTO.Page - 1) * studentQueryDTO.PageSize)
    //     .Take(studentQueryDTO.PageSize)
    //     .Select(u => TransformUserInfoUtils.GetStudentInfo(u))
    //     .ToListAsync();
    // }
}
