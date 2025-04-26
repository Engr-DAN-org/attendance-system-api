using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.DTOs;
using api.Models.QueryParams;

namespace api.Interfaces.Service
{
    public interface ISectionService
    {
        public Task<GetSectionDTO> CreateSectionAsync(CreateSectionDTO section);
        public Task<GetSectionDTO> UpdateSectionAsync(int sectionId, CreateSectionDTO section);

        public Task<SectionQueryDTO> GetSectionsAsync(SectionQueryParams queryParams);

        public Task DeleteSectionAsync(int sectionId);
        public Task<GetSectionDTO> GetSectionByIdAsync(int sectionId);
    }
}