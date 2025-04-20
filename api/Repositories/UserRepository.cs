using System;
using api.Data;
using api.Enums;
using api.Exceptions;
using api.Interfaces.Repository;
using api.Models.DTOs;
using api.Models.Entities;
using api.Models.QueryParams;
using api.Utils;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<User> CreateUserAsync(RegisterUserDTO user, string password)
    {
        var createdUser = await _context.Users.AddAsync(new User
        {
            IdNumber = user.IdNumber,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserRole = user.UserRole,
            PhoneNumber = user.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        });
        await _context.SaveChangesAsync();
        return createdUser.Entity;
    }

    public async Task<User> DeleteUserAsync(string id)
    {
        try
        {
            var user = await FindByIdAsync(id);
            if (user.GuardianId != null)
            {
                var guardian = await _context.Guardians.FindAsync(user.GuardianId);
                if (guardian != null)
                {
                    _context.Guardians.Remove(guardian);
                }
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<User> FindFirstAdmin()
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserRole == UserRole.Admin) ?? throw new NotFoundException(nameof(User)); ;
    }

    public async Task<User> FindByEmailOrIdNoAsync(string emailOrIdNo)
    {
        return await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == emailOrIdNo || u.IdNumber == emailOrIdNo) ?? throw new NotFoundException(nameof(User)); ;
    }

    public async Task<User> FindByIdAsync(string id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id) ?? throw new NotFoundException(nameof(User));
    }


    public async Task<User> FindBySchoolIdNoAsync(string schoolId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.IdNumber == schoolId) ?? throw new NotFoundException(nameof(User)); ;
    }

    public async Task<UsersQueryDTO> GetUsersAsync(UsersQueryParams queryParams)
    {
        try
        {
            // Apply filters
            var query = _context.Users.AsQueryable()
                    .Where(u => u.UserRole != UserRole.Admin);

            if (!string.IsNullOrWhiteSpace(queryParams.Name))
            {
                var searchName = queryParams.Name.ToLower();
                query = query.Where(u => (u.FirstName + " " + u.LastName).ToLower().Contains(searchName));
            }

            if (queryParams.Role.Length > 0)
            {
                var roleEnums = queryParams.Role
                        .Select(r => Enum.Parse<UserRole>(r, ignoreCase: true))
                        .ToList();
                query = query.Where(u => roleEnums.Contains(u.UserRole));
            }

            if (queryParams.Status.Length > 0)
            {
                var statusEnums = queryParams.Status
                        .Select(s => Enum.Parse<UserStatus>(s, ignoreCase: true))
                        .ToList();
                query = query.Where(u => statusEnums.Contains(u.Status));
            }

            // Get total count and total pages
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / queryParams.PageSize);

            // Apply pagination
            var data = await query
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            return new UsersQueryDTO(totalCount, totalPages, queryParams.PageNumber, queryParams.PageSize, data);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> IsEmailUsedAsync(string email, User? user = null)
    {
        if (user != null)
        {
            return await _context.Users.AnyAsync(u => u.Email == email && u.Id != user.Id);
        }
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
    public async Task<bool> IsIdNumberUsedAsync(string idNumber, User? user = null)
    {
        if (user != null)
        {
            return await _context.Users.AnyAsync(u => u.IdNumber == idNumber && u.Id != user.Id);
        }
        return await _context.Users.AnyAsync(u => u.IdNumber == idNumber);
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
