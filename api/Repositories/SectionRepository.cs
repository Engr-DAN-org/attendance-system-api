using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Exceptions;
using api.Interfaces.Repository;
using api.Models.DTOs;
using api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class SectionRepository(AppDbContext context) : ISectionRepository
    {
        private readonly AppDbContext _context = context;


        public async Task<Section> CreateSectionAsync(CreateSectionDTO createSectionDTO)
        {
            var section = new Section()
            {
                YearLevel = createSectionDTO.YearLevel,
                Name = createSectionDTO.Name,
                Description = createSectionDTO.Description,
                TeacherId = createSectionDTO.TeacherId
            };

            await _context.Sections.AddAsync(section);
            await _context.SaveChangesAsync();
            return section;
        }

        public async Task DeleteSectionAsync(int id)
        {
            try
            {
                var section = await GetSectionByIdAsync(id);
                _context.Sections.Remove(section);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                throw;
            }

        }

        public async Task<Section> GetSectionByIdAsync(int id)
        {
            return await _context.Sections.FirstOrDefaultAsync(sec => sec.Id == id) ?? throw new NotFoundException(nameof(Section));
        }

        public async Task<List<Section>> GetSectionByTeacherIdAsync(string id)
        {
            return await _context.Sections.Where(sec => sec.TeacherId == id).ToListAsync() ?? throw new NotFoundException(nameof(Section));
        }

        public Task<List<Section>> GetSectionsAsync()
        {
            return _context.Sections.ToListAsync() ?? throw new NotFoundException(nameof(Section));
        }

        public async Task RollbackTransactionAsync()
        {

            await _context.Database.RollbackTransactionAsync();
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }
        public async Task<Section> UpdateSectionAsync(int sectionId, CreateSectionDTO createSectionDTO)
        {
            try
            {
                var section = await GetSectionByIdAsync(sectionId);

                section.YearLevel = createSectionDTO.YearLevel;
                section.Name = createSectionDTO.Name;
                section.Description = createSectionDTO.Description;
                section.TeacherId = createSectionDTO.TeacherId;

                await _context.SaveChangesAsync();
                return section;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}