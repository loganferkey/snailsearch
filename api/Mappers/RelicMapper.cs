using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO;
using api.Models;

namespace api.Mappers
{
    public static class RelicMapper
    {
        /// Returns a more bundled relic object for list pages,
        // I only need the full information of a relic for detail pages, not lists :D
        public static RelicDto ToRelicDto(this Relic instance)
        {
            return new RelicDto
            {
                Id = instance.Id,
                Name = instance.Name,
                Rank = instance.Rank,
                AFFCT = instance.AFFCT,
                Special = instance.Special,
                Skill1 = instance.Skill1,
                Skill2 = instance.Skill2,
                Skill3 = instance.Skill3,
                Description = instance.Description
            };
        }
    }
}