using System;
using api.Models;

namespace api.Interfaces.Repository;

public interface ISectionRepository
{
    public Task<Section> GetSectionByIdAsync(string id);
    public Task<Section[]> GetSectionByTeacherIdAsync(string id);
    public Task<Section[]> GetSectionsAsync();
    public Task<Section> UpdateSectionAsync(Section section);
    public Task<Section> CreateSectionAsync(Section section);
    public Task<Section> DeleteSectionAsync(string id);

}
