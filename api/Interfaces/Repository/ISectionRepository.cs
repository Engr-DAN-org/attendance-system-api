using System;
using api.Models.DTOs;
using api.Models.Entities;
using api.Models.QueryParams;

namespace api.Interfaces.Repository;

public interface ISectionRepository
{
    public Task<Section> GetSectionByIdAsync(int id);
    public Task<List<Section>> GetSectionByTeacherIdAsync(string id);
    public Task<SectionQueryDTO> GetSectionsAsync(SectionQueryParams queryParams);
    public Task<Section> UpdateSectionAsync(int sectionId, CreateSectionDTO createSectionDTO);
    public Task<Section> CreateSectionAsync(CreateSectionDTO createSectionDTO);
    public Task DeleteSectionAsync(int id);

}
