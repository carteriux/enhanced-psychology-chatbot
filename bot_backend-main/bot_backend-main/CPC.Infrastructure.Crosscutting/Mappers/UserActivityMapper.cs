using CPC.Domain.Entities;
using md = CPC.Domain.Models;

namespace CPC.Infrastructure.Crosscutting.Mappers
{
    public static class UserActivityMapper
    {
        public static Useractivity ToEntity(this md.Useractivity model)
        {
            return new Useractivity
            {
                Id = model.Id,
                IdUser = model.IdUser,
                IdActivity = model.IdActivity,
                Count = model.Count,
                ProgressPercentage = model.ProgressPercentage,
                StartDateTime = model.StartDateTime,
                EndDateTime = model.EndDateTime,
                FilePath = model.FilePath,
                ActivityName = model.IdActivityNavigation.ActivityName
            };
        }

        public static md.Useractivity ToModel(this Useractivity entity)
        {
            return new md.Useractivity
            {
                Id = entity.Id,
                IdUser = entity.IdUser,
                IdActivity = entity.IdActivity,
                Count = entity.Count,
                ProgressPercentage = entity.ProgressPercentage,
                StartDateTime = entity.StartDateTime,
                EndDateTime = entity.EndDateTime,
                FilePath = entity.FilePath
            };
        }
    }
}
