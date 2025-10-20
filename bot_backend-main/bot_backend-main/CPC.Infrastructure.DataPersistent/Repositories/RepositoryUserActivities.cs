using CPC.Domain.Aggregations.UserActivities;
using CPC.Domain.Entities;
using CPC.Infraestructure.Crosscutting.DataObjects.Contracts;
using CPC.Infraestructure.Crosscutting.DataObjects.Core;
using CPC.Infrastructure.Crosscutting.Mappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using md = CPC.Domain.Models;

namespace CPC.Infrastructure.DataPersistent.Repositories
{
    public class RepositoryUserActivities : Repository<md.CpcContext>, IRepositoryUserActivities
    {
        public RepositoryUserActivities(md.CpcContext context, IRepository repository) : base(context)
        {
        }

        public async Task<List<Useractivity>> GetActivitiesByUserIdAsync(int idUser)
        {
            var result = (await this.Filter<md.Useractivity>(x => x.IdUser == idUser));

            foreach (var x in result)
            {
                x.IdActivityNavigation = (await this.GetByID<md.Activity>(x.IdActivity));
            }

            return result.Select(x => x.ToEntity()).ToList();
        }

        public async Task<Useractivity> GetActivitysById(int idActivity, int idUser)
        {
            var activity = (await this.Filter<md.Useractivity>(x => x.Id == idActivity && x.IdUser == idUser)).FirstOrDefault();

            if (ReferenceEquals(activity, null))
            {
                throw new Exception("Actividad no encontrada");
            }
            activity.IdActivityNavigation = (await this.GetByID<md.Activity>(activity.IdActivity));

            return activity.ToEntity();
        }

        public async Task<Useractivity> UpdateActivityByUserIdAsync(Useractivity useractivity)
        {                            
            var result = await this.Update(useractivity.ToModel());
            if (result)
            {
                return useractivity;
            }
            return null;            
        }
    }
}
