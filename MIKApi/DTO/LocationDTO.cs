using MIKApi.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIKApi.DTO
{
    public class LocationGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LocationDTO> locations { get; set; }
    }

    public class LocationDTO
    {
        public int locationId { get; set; }
        public string LocationName { get; set; }
        public int LocationGroupId { get; set; }
        public string LocationGroupName { get; set; }
        public long NauhaCount { get; set; }

        public LocationDTO()
        {

        }
        public Location TranslateDtoToEntity(LocationDTO dto)
        {
            return new Location() {
                LocationID = dto.locationId,
                LocationName = dto.LocationName,
                LocationGroupId = dto.LocationGroupId
            };
        }
        public LocationDTO TranslateEntityToDto(Location loc)
        {
            return new LocationDTO()
            {
                locationId = loc.LocationID,
                LocationName = loc.LocationName,
                LocationGroupId = loc.LocationGroupId
            };
        }
    }
}
