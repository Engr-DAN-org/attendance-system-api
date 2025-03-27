using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Enums;
using api.Interfaces.Repository;
using api.Interfaces.Service;
using api.Models.DTOs;
using api.Models.Entities;

namespace api.Services
{
    public class StudentService(IUserRepository userRepository, IGuardianRepository guardianRepository) : IStudentService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IGuardianRepository _guardianRepository = guardianRepository;


        public async Task<GetStudentDTO> CreateStudentAsync(CreateStudentDTO student)
        {
            await _userRepository.BeginTransactionAsync();
            try
            {
                var user = await _userRepository.CreateUserAsync(new User()
                {
                    IdNumber = student.IdNumber,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.Email,
                    UserRole = UserRole.Student,
                    SectionId = student.SectionId
                });

                var guardian = await _guardianRepository.CreateGuardianAsync(user, student.Guardian);

                user.GuardianId = guardian.Id;
                user.Guardian = guardian;

                await _userRepository.CommitTransactionAsync();
                return new GetStudentDTO(user);
            }
            catch (Exception)
            {
                await _userRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteStudentAsync(string id)
        {
            await _userRepository.BeginTransactionAsync();
            try
            {
                await _userRepository.DeleteUserAsync(id);
                await _guardianRepository.DeleteGuardianAsync(id);

                await _userRepository.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _userRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<GetStudentDTO?> GetStudentByIdAsync(string id)
        {
            var student = await _userRepository.FindByIdAsync(id);
            return student?.UserRole == UserRole.Student ? new GetStudentDTO(student) : null;
        }

        public async Task<List<GetStudentDTO>> GetStudentsAsync(StudentQueryDTO studentQueryDTO)
        {
            //     public int? YearLevel { get; set; }
            return await _userRepository.GetUsersAsync<GetStudentDTO>(UserRole.Teacher, studentQueryDTO.Page,
            queryCallback: query =>
              {
                  if (!string.IsNullOrEmpty(studentQueryDTO.Name)) // Filter by Name if provided
                      query = query.Where(u => u.FullName.Contains(studentQueryDTO.Name, StringComparison.OrdinalIgnoreCase));
                  if (!string.IsNullOrEmpty(studentQueryDTO.IdNumber))
                      query = query.Where(u => u.IdNumber == studentQueryDTO.IdNumber);
                  if (!string.IsNullOrEmpty(studentQueryDTO.Email))
                      query = query.Where(u => u.FullName.Contains(studentQueryDTO.Email, StringComparison.OrdinalIgnoreCase));
                  if (!string.IsNullOrEmpty(studentQueryDTO.SectionId) && int.TryParse(studentQueryDTO.SectionId, out int sectionId))
                      query = query.Where(u => u.SectionId == sectionId);
                  if (!string.IsNullOrEmpty(studentQueryDTO.GuardianName))
                      query = query.Where(u => u.Guardian != null && u.Guardian.FullName.Contains(studentQueryDTO.GuardianName));
                  if (studentQueryDTO.YearLevel != null)
                      query = query.Where(u => u.Section != null && u.Section.YearLevel == studentQueryDTO.YearLevel);
              },
              selectCallback: query =>
              {
                  return query.Select(u => new GetStudentDTO(u));
              });
        }

        public async Task<GetStudentDTO> UpdateStudentAsync(UpdateStudentDTO updateStudentDTO)
        {
            await _userRepository.BeginTransactionAsync();
            try
            {
                var student = await _userRepository.FindBySchoolIdNoAsync(updateStudentDTO.IdNumber) ?? throw new Exception("Student not found");

                var user = await _userRepository.UpdateUserAsync(new User
                {
                    Id = student.Id,
                    IdNumber = updateStudentDTO.IdNumber,
                    FirstName = updateStudentDTO.FirstName,
                    LastName = updateStudentDTO.LastName,
                    Email = updateStudentDTO.Email,
                    SectionId = updateStudentDTO.SectionId
                });
                if (updateStudentDTO.Guardian != null)
                {
                    var guardian = await _guardianRepository.UpdateGuardianAsync(user, updateStudentDTO.Guardian);
                }

                await _userRepository.CommitTransactionAsync();
                return new GetStudentDTO(user);
            }
            catch (Exception)
            {
                await _userRepository.RollbackTransactionAsync();
                throw;
            }

        }
    }
}