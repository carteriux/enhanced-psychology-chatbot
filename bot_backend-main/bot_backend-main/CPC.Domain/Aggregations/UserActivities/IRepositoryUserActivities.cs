using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CPC.Domain.Entities;

namespace CPC.Domain.Aggregations.UserActivities
{
    public interface IRepositoryUserActivities
    {
        // Update activity by user ID
        Task<Useractivity> UpdateActivityByUserIdAsync(Useractivity useractivity);

        // Get activities assigned to a user by user ID
        Task<List<Useractivity>> GetActivitiesByUserIdAsync(int idUser);

        // Get activity by ID
        Task<Useractivity> GetActivitysById(int idActivity, int idUser);
    }
}