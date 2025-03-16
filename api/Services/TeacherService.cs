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
    public class TeacherService(IUserRepository userRepository) : ITeacherService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<GetTeacherDTO> CreateTeacherAsync(CreateTeacherDTO teacher)
        {
            var user = new User()
            {
                IdNumber = teacher.IdNumber,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Email = teacher.Email,
                UserRole = UserRole.Teacher
            };

            var createdUser = await _userRepository.CreateUserAsync(user);

            return new GetTeacherDTO(createdUser);
        }

        public async Task<GetTeacherDTO> UpdateTeacherAsync(UpdateTeacherDTO teacher)
        {
            var user = new User
            {
                IdNumber = teacher.IdNumber,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Email = teacher.Email,
            };

            var updatedUser = await _userRepository.UpdateUserAsync(user);

            return new GetTeacherDTO(updatedUser);
        }

        public async Task DeleteTeacherAsync(string id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<GetTeacherDTO?> GetTeacherByIdAsync(string id)
        {
            var user = await _userRepository.FindByIdAsync(id);

            if (user == null || user.UserRole != UserRole.Teacher)
            {
                return null;
            }

            return new GetTeacherDTO(user);
        }

        public async Task<List<GetTeacherDTO>> GetTeachersAsync(int page, TeacherQueryDTO teacherQueryDTO)
        {

            return await _userRepository.GetUsersAsync<GetTeacherDTO>(UserRole.Teacher, page, queryCallback: query =>
                {
                    if (!string.IsNullOrEmpty(teacherQueryDTO.Name)) // Filter by Name if provided
                        query = query.Where(u => u.FullName.Contains(teacherQueryDTO.Name, StringComparison.OrdinalIgnoreCase));
                    if (!string.IsNullOrEmpty(teacherQueryDTO.IdNumber))
                        query = query.Where(u => u.IdNumber == teacherQueryDTO.IdNumber);
                    if (!string.IsNullOrEmpty(teacherQueryDTO.Email))
                        query = query.Where(u => u.FullName.Contains(teacherQueryDTO.Email, StringComparison.OrdinalIgnoreCase));
                }, selectCallback: query =>
                {
                    return query.Select(u => new GetTeacherDTO(u));
                });
        }

    }
}
