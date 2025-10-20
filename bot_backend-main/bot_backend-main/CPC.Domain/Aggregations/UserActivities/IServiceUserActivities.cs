using CPC.Domain.DTOs;
using CPC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPC.Domain.Aggregations.UserActivities
{
    public interface IServiceUserActivities
    {
        Task<List<Useractivity>> GetActivitiesByUserIdAsync(int idUser);

        Task<Useractivity> GetActivityByIdAsync(int id, int idUser);

        Task<ResponseMessageDTO> UpdateActivityByUserIdAsync(RequestUpdateActivityDTO request);

        Task<ResponseFileDTO> EndActivityByUserIdAsync(RequestEndActivityDTO request);

        Task<ResponseFileDTO> GetFileActivityByUserIdAsync(RequestGetActivityDTO request);

        Task<ResponseMessageDTO> ResetUserActivitiesAsync(int idUser);
    }
}
