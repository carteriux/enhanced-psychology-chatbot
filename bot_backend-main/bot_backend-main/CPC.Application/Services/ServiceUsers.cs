using CPC.Domain.Aggregations.Users;
using CPC.Domain.DTOs.@base;
using CPC.Domain.DTOs;
using CPC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPC.Infrastructure.CrossCutting.ICommon;
using CPC.Domain.DTO.ValueObjects;
using CPC.Infrastructure.Crosscutting.Mappers;
using MySqlX.XDevAPI.Common;
using Mysqlx.Session;

namespace CPC.Application.Services
{
    public class ServiceUsers : IServiceUsers
    {
        private readonly IRepositoryUsers repositoryUsers;

        private readonly ISecurity security;

        public ServiceUsers(IRepositoryUsers repositoryUsers, ISecurity security)
        {
            this.repositoryUsers = repositoryUsers;
            this.security = security;
        }

        public async Task<ResponseUsersDTO> CreateMultipleUsersAsync(List<RequestCreateUserDTO> users)
        {
            var response = new ResponseUsersDTO();            
            var resultUsers = new List<UserDTO>();

            users.ForEach(async user =>
            {
                try
                {                    
                    var result = await repositoryUsers.CreateUserAsync(user.ToUser());
                    resultUsers.Add(result.ToUserResponseDTO());
                }
                catch (Exception ex)
                {
                }
            });

            response.Users = resultUsers;
            return response;
        }

        public async Task<ResponseUserDTO> CreateUserAsync(RequestCreateUserDTO user)
        {
            var response = new ResponseUserDTO();
            var result = await repositoryUsers.CreateUserAsync(user.ToUser());
            response.User = result.ToUserResponseDTO();
            return response;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await repositoryUsers.DeleteUserAsync(userId);            
        }

        public async Task<ResponseUsersDTO> GetAllUsersAsync()
        {
            var response = new ResponseUsersDTO();
            var result = await repositoryUsers.GetAllUsersAsync();
            response.Users = result.Select(s => s.ToUserResponseDTO()).ToList();
            return response;
        }

        public async Task<ResponseUsersDTO> GetUsersByCohortAsync(string? cohort)
        {
            var response = new ResponseUsersDTO();
            var result = await repositoryUsers.GetAllUsersAsync();
            
            // Filter by cohort if specified
            if (!string.IsNullOrEmpty(cohort))
            {
                result = result.Where(u => u.Cohort != null && u.Cohort.Equals(cohort, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            
            response.Users = result.Select(s => s.ToUserResponseDTO()).ToList();
            return response;
        }

        public async Task<ResponseUserDTO> GetUserByIdAsync(int userId)
        {
            var response = new ResponseUserDTO();
            var result = await repositoryUsers.GetUserByIdAsync(userId);
            response.User = result.ToUserResponseDTO();
            return response;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {            
            return await repositoryUsers.UpdateUserAsync(user);            
        }

        public async Task<ResponseLoginDTO> ValidateUserAsync(string EnrollmentNumber, string Email, string Password, string key)
        {
            var response = new ResponseLoginDTO();
            var result = await repositoryUsers.ValidateUserAsync(EnrollmentNumber, Email, Password);
            var localsession = new LocalSession 
            { 
                Email = result.Email, 
                EnrollmentNumber = result.EnrollmentNumber,
                IdUser = result.IdUser,
                FechaUTC = DateTime.UtcNow,                
            };

            response.Token = security.CreateJwtToken(localsession, key);
            response.User = result.ToUserResponseDTO();
            return response;
        }
    }
}
