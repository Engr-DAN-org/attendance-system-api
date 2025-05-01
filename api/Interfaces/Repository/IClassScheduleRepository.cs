using System;
using api.Models.DTOs;
using api.Models.Entities;
using api.Models.QueryParams;

namespace api.Interfaces.Repository;

public interface IClassScheduleRepository
{
    public Task<List<GetClassScheduleDTO>> QueryAsync(ClassScheduleQueryParams queryParams);

    public Task<List<GetClassScheduleDTO>> GetBySectionOrTeacherAsync(ScheduleTeacherSectionQuery queryParams);
    public Task<ClassSchedule> GetScheduleByIdAsync(int id, bool? includeNullSection = true);
    public Task<GetClassScheduleDTO> UpdateScheduleAsync(int id, CreateClassScheduleDTO scheduleDTO);
    public Task<GetClassScheduleDTO> CreateScheduleAsync(CreateClassScheduleDTO scheduleDTO);
    public Task DeleteScheduleAsync(int id);
}
