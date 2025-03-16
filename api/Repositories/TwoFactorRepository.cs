using System;
using api.Data;
using api.Interfaces.Repository;
using api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class TwoFactorRepository(AppDbContext context) : ITwoFactorRepository
{

    private readonly AppDbContext _context = context;

    /*
    * Finds the 2FA entry by email
    */
    public async Task<TwoFactorAuth?> FindByEmailAsync(string email)
    {
        return await _context.TwoFactorAuths
            .AsNoTracking() // Prevents tracking to improve performance
            .FirstOrDefaultAsync(twoFA => twoFA.Email == email);
    }


    public async Task<TwoFactorAuth> CreateAsync(string email)
    {
        try
        {
            var oldTwoFactorAuths = _context.TwoFactorAuths.Where(twoFA => twoFA.Email == email);
            if (await oldTwoFactorAuths.AnyAsync())
            {
                _context.TwoFactorAuths.RemoveRange(oldTwoFactorAuths);
                await _context.SaveChangesAsync();
            }

            var twoFactorAuth = new TwoFactorAuth { Email = email };
            _context.TwoFactorAuths.Add(twoFactorAuth);
            await _context.SaveChangesAsync();
            return twoFactorAuth;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating TwoFactorAuth for {email}: {ex.Message}");
        }
    }


    public async Task DeleteAsync(TwoFactorAuth twoFactorAuth)
    {
        try
        {
            var entity = await _context.TwoFactorAuths.FindAsync(twoFactorAuth.Id);
            if (entity != null)
            {
                _context.TwoFactorAuths.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting TwoFactorAuth: {ex.Message}");
        }
    }


}
