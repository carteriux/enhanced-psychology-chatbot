using CPC.Domain.DTOs.@base;
using CPC.Domain.DTOs;
using CPC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPC.Domain.Aggregations.Users
{
    public interface IServiceUsers
    {
        // Create a new user
        Task<ResponseUserDTO> CreateUserAsync(RequestCreateUserDTO user);

        // Create multiple users
        Task<ResponseUsersDTO> CreateMultipleUsersAsync(List<RequestCreateUserDTO> users);

        // Read a user by ID
        Task<ResponseUserDTO> GetUserByIdAsync(int userId);

        // Read a user by ID
        Task<ResponseLoginDTO> ValidateUserAsync(string EnrollmentNumber, string Email, string Password, string key);

        // Read all users
        Task<ResponseUsersDTO> GetAllUsersAsync();

        // Read users filtered by cohort
        Task<ResponseUsersDTO> GetUsersByCohortAsync(string? cohort);

        // Update an existing user
        Task<bool> UpdateUserAsync(User user);

        // Delete a user by ID
        Task<bool> DeleteUserAsync(int userId);
    }
}
