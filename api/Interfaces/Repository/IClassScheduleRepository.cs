using System;

using api.Models.Entities;

namespace api.Interfaces.Repository;

public interface IClassScheduleRepository
{

    public Task<ClassSchedule[]> GetScheduleBySectionIdAsync(string sectionId);
    public Task<ClassSchedule> GetScheduleByIdAsync(string id);
    public Task<ClassSchedule> UpdateScheduleAsync(ClassSchedule schedule);
    public Task<ClassSchedule> CreateScheduleAsync(ClassSchedule schedule);
    public Task<ClassSchedule> DeleteScheduleAsync(string id);
}
