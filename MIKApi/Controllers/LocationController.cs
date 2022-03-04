using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MIKApi.DAL.DB;
using MIKApi.DTO;

namespace MIKApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : Controller
    {
        private readonly MikContext _context;
        private IWebHostEnvironment _env;
        private readonly IConfiguration Configuration;

        public LocationController(MikContext context)
        {
            _context = context;
        }
        //Get: api/location
        [HttpGet()]
        public IEnumerable<LocationDTO> Get()
        {

            var locations = _context.Locations;
            List<LocationDTO> locationList = new List<LocationDTO>();
            var locWithNauhaCnt = from g in _context.LocationGroups
                                  join l in locations on g.Id equals l.LocationGroupId 
                                  orderby g.Name 
                                  select new { l.LocationID, l.LocationName, l.SubLocationName, g.Id, g.Name, g.ParentName};

            foreach (var item in locWithNauhaCnt)
            {
               // var count = _context.Nauhas.Where(x => x.LocationID == item.LocationID).ToList().Count();
                locationList.Add(new LocationDTO()
                {
                    locationId = item.LocationID ,
                    LocationName = $"{item.LocationName} {item.SubLocationName} {item.Name} {item.ParentName}",
                    LocationGroupId = item.Id,
                    LocationGroupName = item.Name
                });
            }
            return locationList;
        }

        [HttpGet]
        [Route("[action]/{locationId}")]
        public IActionResult GetNauhaByLocation([FromRoute] int locationId)
        {
            var location = _context.Locations.FirstOrDefault(x => x.LocationID == locationId);
            var list = _context.Nauhas.Where(x => x.LocationID == locationId);
            List<NauhaDTO> nauhaList = new List<NauhaDTO>();
            if (list == null)
            {
                return NotFound();
            }
            foreach (var item in list)
            {
                var nauhaDto = new NauhaDTO().TranslateEntityToDto(item, location);              
                nauhaList.Add(nauhaDto);
            }
            return Ok(nauhaList);
        }



        // GET: api/Location/5
        [HttpGet("{id}")]

        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var nauha = _context.Nauhas.FirstOrDefault(x => x.LocationID == id);

            if (nauha == null)
            {
                return NotFound();
            }

            return Ok(nauha);
        }
    }
}