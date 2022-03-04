using MIKApi.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIKApi.DTO
{
    public class YearDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Year TranslateEntityToDto(YearDTO dto)
        {
            return new Year()
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
        public YearDTO TranslateDtoToEntity(Year loc)
        {
            return new YearDTO()
            {
                Id = loc.Id,
                Name = loc.Name
            };
        }
    }
}
