using System;
using api.Models.DTOs;
using api.Models.Entities;

namespace api.Interfaces.Repository;

public interface ISectionRepository
{
    public Task<Section> GetSectionByIdAsync(int id);
    public Task<Section[]> GetSectionByTeacherIdAsync(string id);
    public Task<Section[]> GetSectionsAsync();
    public Task<Section> UpdateSectionAsync(int sectionId, CreateSectionDTO createSectionDTO);
    public Task<Section> CreateSectionAsync(CreateSectionDTO createSectionDTO);
    public Task DeleteSectionAsync(int id);

    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();

}
