using CPC.Domain.Aggregations.Users;
using CPC.Domain.Entities;
using CPC.Infraestructure.Crosscutting.DataObjects.Contracts;
using CPC.Infraestructure.Crosscutting.DataObjects.Core;
using CPC.Infrastructure.Crosscutting.Mappers;
using CPC.Infrastructure.CrossCutting.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using md = CPC.Domain.Models;

namespace CPC.Infrastructure.DataPersistent.Repositories
{
    public class RepositoryUsers : Repository<md.CpcContext>, IRepositoryUsers
    {
        public RepositoryUsers(md.CpcContext context, IRepository repository) : base(context)
        {            
        }
        
        public async Task<User> CreateUserAsync(User user)
        {
            var exist = (await this.Filter<md.User>(x => x.EnrollmentNumber == user.EnrollmentNumber && x.Email == user.Email)).FirstOrDefault();

            if (!ReferenceEquals(exist, null))
            {
                throw new Exception("Ya existe el usuario");
            }

            var modelUser = user.ToModel();
            modelUser.Password = modelUser.Password.Encrypt();

            var entityUser =  (await this.Create<md.User>(modelUser)).ToEntity();            

            var activities = (await this.GetTable<md.Activity>()).ToList();

            foreach (var activity in activities)
            {
                var userActivity = new md.Useractivity();
                userActivity.IdUser = entityUser.IdUser;
                userActivity.IdActivity = activity.IdActivity;
                userActivity.ProgressPercentage = 0;
                userActivity.FilePath = string.Empty;

                var result = await this.Create<md.Useractivity>(userActivity);
            }

            return entityUser;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await this.Delete<md.User>(await this.GetByID<md.User>(userId));
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = new List<User>();
            try
            {
                var modelUsers = (await this.Filter<md.User>(x => x.IsAdmin == false)).ToList();
                users = modelUsers.Select(user => user.ToEntity()).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error Repository Users: ", ex);
            }
            return users;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var exist = (await this.Filter<md.User>(x => x.IdUser == userId, "Useractivity")).FirstOrDefault();

            if (ReferenceEquals(exist, null))
            {
                throw new Exception("No existe el usuario");
            }

            var activities = (await this.GetTable<md.Activity>()).ToList();            

            return exist.ToEntity(activities);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var exist = (await this.Filter<md.User>(x => x.IdUser == user.IdUser)).FirstOrDefault();

            if (ReferenceEquals(exist, null))
            {
                throw new Exception("No existe el usuario");
            }

            return await this.Update<md.User>(user.ToModel());
        }

        public async Task<User> ValidateUserAsync(string EnrollmentNumber, string Email, string Password)
        {
            var exist = (await this.Filter<md.User>(x => (x.EnrollmentNumber == EnrollmentNumber || x.Email == Email) && x.Password == Password.Encrypt())).FirstOrDefault();

            if (ReferenceEquals(exist, null))
            {
                throw new Exception("Datos de usuario incorrectos");
            }

            exist.LastAccessDate = DateTime.Now;
            await this.Update<md.User>(exist);
            exist.Password = string.Empty;
            return exist.ToEntity();
        }
    }
}
