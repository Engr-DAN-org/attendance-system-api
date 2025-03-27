using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces.Repository;
using api.Interfaces.Service;
using api.Models.DTOs;

namespace api.Services
{
    public class SectionService(ISectionRepository sectionRepository) : ISectionService
    {
        private readonly ISectionRepository _sectionRepository = sectionRepository;

        public async Task<GetSectionDTO> CreateSectionAsync(CreateSectionDTO section)
        {
            await _sectionRepository.BeginTransactionAsync();
            try
            {
                var newSection = await _sectionRepository.CreateSectionAsync(section);

                await _sectionRepository.CommitTransactionAsync();

                return new GetSectionDTO(newSection);
            }
            catch (Exception)
            {
                await _sectionRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteSectionAsync(int sectionId)
        {
            await _sectionRepository.BeginTransactionAsync();
            try
            {
                await _sectionRepository.DeleteSectionAsync(sectionId);

                await _sectionRepository.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _sectionRepository.RollbackTransactionAsync();

                throw;
            }
        }

        public async Task<GetSectionDTO?> GetSectionByIdAsync(int sectionId)
        {
            var section = await _sectionRepository.GetSectionByIdAsync(sectionId);
            return section == null ? null : new GetSectionDTO(section);
        }

        public Task<GetSectionDTO> UpdateSectionAsync(int sectionId, CreateSectionDTO section)
        {
            throw new NotImplementedException();
        }
    }
}