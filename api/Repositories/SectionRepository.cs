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
            var section = await GetSectionByIdAsync(id);
            _context.Sections.Remove(section);
            await _context.SaveChangesAsync();
        }

        public async Task<Section> GetSectionByIdAsync(int id)
        {
            var section = await _context.Sections.FirstOrDefaultAsync(sec => sec.Id == id) ?? throw new NotFoundException(nameof(Section));

            return section;
        }

        public Task<Section[]> GetSectionByTeacherIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Section[]> GetSectionsAsync()
        {
            throw new NotImplementedException();
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
            var section = await GetSectionByIdAsync(sectionId);

            section.YearLevel = createSectionDTO.YearLevel;
            section.Name = createSectionDTO.Name;
            section.Description = createSectionDTO.Description;
            section.TeacherId = createSectionDTO.TeacherId;

            await _context.SaveChangesAsync();
            return section;

        }
    }
}