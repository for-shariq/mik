using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MIKApi.DAL.DB;
using MIKApi.DTO;

namespace MIKApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationGroupController : ControllerBase
    {
        private readonly MikContext _context;
        private IWebHostEnvironment _env;
        private readonly IConfiguration Configuration;
        public LocationGroupController(MikContext context, IWebHostEnvironment env, IConfiguration Conf)
        {
            _context = context;
            _env = env;
            Configuration = Conf;
           
        }
        // GET: api/LocationGroup
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var _groups = await _context.LocationGroups.OrderBy(x => x.Name).ToListAsync();
            var data = from g in _groups select new { Id = g.Id, Name = String.Concat(g.Name,", ", g.ParentName).Trim(',').TrimEnd(',') };
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var locationGroup = new LocationGroupDTO();
            var groups = await _context.LocationGroups.FirstOrDefaultAsync(x => x.Id == id);
            if (groups == null)
            {
                return NotFound();
            }
            locationGroup.Id = groups.Id;
            locationGroup.Name = groups.Name;
            locationGroup.locations = new List<LocationDTO>();
            var childLocations = _context.Locations?.Where(x => x.LocationGroupId == locationGroup.Id);
            foreach (var item in childLocations)
            {
                var locationDto = new LocationDTO().TranslateEntityToDto(item);
                try
                {
                    locationDto.NauhaCount = (long)_context.Nauhas?.Where(x => x.LocationID == locationDto.locationId)?.ToList().Count();
                }
                catch (Exception)
                {

                    locationDto.NauhaCount = 0;
                }

                locationGroup.locations.Add(locationDto);
            }


            return Ok(locationGroup);
        }
    }
}
