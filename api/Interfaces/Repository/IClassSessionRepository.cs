using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.DTOs;
using api.Models.Entities;

namespace api.Interfaces.Repository
{
    public interface IClassSessionRepository
    {
        Task<ClassSession> CreateAsync(ClassSchedule classSchedule, CreateClassSessionDTO dto);
        Task<ClassSession> GetByIdAsync(string id, bool? includesRelation = false);
        Task<List<ClassSession>> GetListByClassScheduleIdAsync(int classScheduleId);
        Task<ClassSession> EndClassSessionAsync(string id);
        Task<ClassSession> CancelClassSessionAsync(string id);

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

    }
}