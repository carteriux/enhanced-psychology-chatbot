using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPC.Domain.Entities;

namespace CPC.Domain.Aggregations.Users
{
    public interface IRepositoryUsers
    {
        // Create a new user
        Task<User> CreateUserAsync(User user);        

        // Read a user by ID
        Task<User> GetUserByIdAsync(int userId);

        // Read a user by ID
        Task<User> ValidateUserAsync(string EnrollmentNumber, string Email, string Password);

        // Read all users
        Task<List<User>> GetAllUsersAsync();

        // Update an existing user
        Task<bool> UpdateUserAsync(User user);

        // Delete a user by ID
        Task<bool> DeleteUserAsync(int userId);
    }
}
