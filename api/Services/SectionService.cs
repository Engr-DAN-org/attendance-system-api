using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Exceptions;
using api.Interfaces.Repository;
using api.Interfaces.Service;
using api.Models.DTOs;
using api.Models.Entities;
using api.Models.QueryParams;

namespace api.Services
{
    public class SectionService(ISectionRepository sectionRepository) : ISectionService
    {
        private readonly ISectionRepository _sectionRepository = sectionRepository;

        public async Task<GetSectionDTO> CreateSectionAsync(CreateSectionDTO section)
        {
            try
            {
                var newSection = await _sectionRepository.CreateSectionAsync(section);

                return new GetSectionDTO(newSection);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteSectionAsync(int sectionId)
        {
            try
            {
                await _sectionRepository.DeleteSectionAsync(sectionId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GetSectionDTO> GetSectionByIdAsync(int sectionId)
        {
            try
            {
                var section = await _sectionRepository.GetSectionByIdAsync(sectionId) ?? throw new NotFoundException(nameof(Section));
                return new GetSectionDTO(section);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SectionQueryDTO> GetSectionsAsync(SectionQueryParams queryParams)
        {
            try
            {
                return await _sectionRepository.GetSectionsAsync(queryParams);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GetSectionDTO> UpdateSectionAsync(int sectionId, CreateSectionDTO section)
        {
            try
            {
                var updatedSection = await _sectionRepository.UpdateSectionAsync(sectionId, section);
                return new GetSectionDTO(updatedSection);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}