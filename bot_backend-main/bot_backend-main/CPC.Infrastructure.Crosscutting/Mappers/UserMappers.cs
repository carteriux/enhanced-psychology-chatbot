using CPC.Domain.DTOs;
using System.Collections.Generic;
using System.Linq;
using Entities = CPC.Domain.Entities;
using Models = CPC.Domain.Models;

namespace CPC.Infrastructure.Crosscutting.Mappers
{
    public static class UserMapper
    {
        
        public static Models.User ToModel(this Entities.User entity)
        {
            if (entity == null) return null;

            return new Models.User
            {
                IdUser = entity.IdUser,
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                MiddleName = entity.MiddleName,
                EnrollmentNumber = entity.EnrollmentNumber,
                Password = entity.Password,
                IsFirstTime = entity.IsFirstTime,
                LastAccessDate = entity.LastAccessDate,
                IsAdmin = entity.IsAdmin
            };
        }

        public static Entities.User ToEntity(this Models.User model, List<Models.Activity> activities = null)
        {
            if (model == null) return null;

            return new Entities.User
            {
                IdUser = model.IdUser,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                EnrollmentNumber = model.EnrollmentNumber,
                Password = model.Password,
                IsFirstTime = model.IsFirstTime,
                LastAccessDate = model.LastAccessDate,
                IsAdmin = model.IsAdmin,
                Useractivity = model.Useractivity?.Select(ua => new Entities.Useractivity
                {
                    Id = ua.Id,
                    IdUser = ua.IdUser,
                    IdActivity = ua.IdActivity,
                    ActivityName = activities != null ? activities.Where(a => a.IdActivity == ua.IdActivity).Select(a => a.ActivityName).FirstOrDefault() ?? string.Empty : string.Empty,
                    ProgressPercentage = ua.ProgressPercentage,
                    FilePath = ua.FilePath,
                    StartDateTime = ua.StartDateTime,
                    EndDateTime = ua.EndDateTime

                }).ToList() ?? new List<Entities.Useractivity>()
            };
        }

        public static Entities.User ToUser(this RequestCreateUserDTO requestUserDTO)
        {
            return new Entities.User
            {
                Email = requestUserDTO.Email,
                FirstName = requestUserDTO.FirstName,
                LastName = requestUserDTO.LastName,
                MiddleName = requestUserDTO.MiddleName,
                EnrollmentNumber = requestUserDTO.EnrollmentNumber,
                Password = requestUserDTO.Password,
                IsFirstTime = false,
                IsAdmin = false,
            };
        }

        public static UserDTO ToUserDTO(this Entities.User model)
        {
            return new UserDTO
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                EnrollmentNumber = model.EnrollmentNumber,                
                IsFirstTime = false,
                IsAdmin = false,
            };
        }

        public static UserDTO ToUserResponseDTO(this Entities.User model)
        {
            return new UserDTO
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                EnrollmentNumber = model.EnrollmentNumber,
                IsFirstTime = model.IsFirstTime,
                IsAdmin = model.IsAdmin,
                IdUser = model.IdUser,
            };
        }

    }
}