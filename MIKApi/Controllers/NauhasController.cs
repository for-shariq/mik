using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MIKApi.DAL.DB;
using MIKApi.DAL.Models;
using System.IO;
using Microsoft.Extensions.Configuration;
using MIKApi.DTO;
using System.Net.Mime;
using MIKApi.Helpers;
using Microsoft.AspNetCore.Authorization;


namespace MIKApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class NauhasController : ControllerBase
    {
        private readonly MikContext _context;
        private IWebHostEnvironment _env;
        private readonly IConfiguration Configuration;
        private string localFileUrl = "";
        private StorageHelper storageHelper;

        public NauhasController(MikContext context, IWebHostEnvironment env, IConfiguration Conf)
        {
            _context = context;
            _env = env;
            Configuration = Conf;
            storageHelper = new StorageHelper(Conf);
        }


        #region GET
        // GET: api/Nauhas
        [HttpGet]
        public IEnumerable<NauhaDTO> GetNauhas()
        {

            var nauhas = from n in _context.Nauhas.OrderBy(x => x.Rank.GetValueOrDefault(uint.MaxValue))
                         join loc in _context.Locations on n.LocationID equals loc.LocationID
                         select new { n, loc };


            List<NauhaDTO> nauhaList = new List<NauhaDTO>();
            foreach (var item in nauhas)
            {
                //var location = _context.Locations.FirstOrDefault(x => x.LocationID == item.LocationID);
                nauhaList.Add(new NauhaDTO().TranslateEntityToDto(item.n,item.loc));
            }

            return nauhaList;
            //return new List<NauhaDTO>() { new NauhaDTO() { Title = Configuration.GetConnectionString("MikContext") } };
        }
        // GET: api/Nauhas/top-ranked
        [HttpGet("top-ranked")]
        public IEnumerable<NauhaDTO> GetRankedNauhas()
        {
            //var nauhas = _context.Nauhas.OrderBy(x => x.Rank).Take(3).Join(_context.Locations, n => n.LocationID, l => l.LocationID()

            var _nauhas = from n in _context.Nauhas.Where(x=>x.Approved == true)
                          join l in _context.Locations on n.LocationID equals l.LocationID
                          orderby n.Rank.GetValueOrDefault(int.MaxValue) ascending
                          select new { n,l };

            List<NauhaDTO> nauhaList = new List<NauhaDTO>();
            foreach (var item in _nauhas.Take(3))
            {
                //var location = _context.Locations.FirstOrDefault(x => x.LocationID == item.LocationID);
                nauhaList.Add(new NauhaDTO().TranslateEntityToDto(item.n, item.l));
            }

            return nauhaList;
            //return new List<NauhaDTO>() { new NauhaDTO() { Title = Configuration.GetConnectionString("MikContext") } };
        }
        private List<NauhaDTO> GetTranslatedNauhas(IEnumerable<Nauha> nauhas)
        {
            List<NauhaDTO> nauhaList = new List<NauhaDTO>();
            foreach (var item in nauhas)
            {
                var location = _context.Locations.FirstOrDefault(x => x.LocationID == item.LocationID);
                nauhaList.Add(new NauhaDTO().TranslateEntityToDto(item, location));
            }

            return nauhaList;
        }
       // [Authorize]
        [HttpGet("loc/{locationId}/{approved}")]
        //GET: api/Nauhas/loc/id
        public async Task<IActionResult> GetNauhaByLocation(int locationId, int approved)
        {
            if (locationId == 0)
            {
                return BadRequest();
            }
            var isApproved = false;
            if (approved == 1)
            {
                isApproved = true;
            }
            else if (approved == 2)
            {
                isApproved = false;
            }
            var nauhas = await _context.Nauhas.Where(x => x.LocationID == locationId && x.Approved == isApproved).ToListAsync();
            if (nauhas == null || nauhas.Count == 0)
            {
                return NotFound();
            }

           return Ok(GetTranslatedNauhas(nauhas));
        }
        // GET: api/Nauhas/5
        [HttpGet("{id}")]
       
        public async Task<IActionResult> GetNauha([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nauha = await _context.Nauhas.FindAsync(id);

            if (nauha == null)
            {
                return NotFound();
            }

            return Ok(nauha);
        }
        // GET: api/Nauhas/5
        [HttpGet("{id}/{d}")]
        public async Task<IActionResult> GetNauha([FromRoute] int id,[FromRoute] string d)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nauha = await _context.Nauhas.FindAsync(id);
            var data = System.IO.File.ReadAllBytes(nauha.UrlPath);
            if (data != null)
            {
                return File(data, "audio/mpeg","");
            }

            if (nauha == null)
            {
                return NotFound();
            }

            return Ok(nauha);
        }
        #endregion

        #region PRIVATE
        private void CreateFileObject(NauhaDTO NauhaDTO)
        {
           
            if (NauhaDTO.fileBinaries != null && NauhaDTO.fileBinaries.Length > 0)
            {
                string rootPath = _env.WebRootPath;//_env.ContentRootPath;
                //var arout = ;
                //var _rootDirectory = Path.Combine(rootPath, Configuration["RootStorageContainer"]);
                NauhaDTO.LocationDto.LocationName = GetLocationName(NauhaDTO.LocationDto.locationId);
                NauhaDTO = storageHelper.Upload(NauhaDTO);
                if (NauhaDTO == null)
                {
                    throw new Exception("Unable to upload");
                }
                else
                {
                    localFileUrl = NauhaDTO.UrlPath;
                }
              //  var _locationName = Path.Combine(_rootDirectory, locDBName);
                //var relatePath = $"{Configuration["RootStorageContainer"]}\\{locDBName}\\";
                //var _year = Convert.ToDateTime(NauhaDTO.Year).Year;

                //if (!string.IsNullOrEmpty(_locationName) && _year > 2021) //TODO: remove 2021
                //{
                //    if (CreateDirectory(_rootDirectory) && CreateDirectory(_locationName))//&& CreateDirectory(Path.Combine(_locationName,_year.ToString()))
                //    {
                //        //var _containerPath = $"{_locationName}\\{_year}\\{Guid.NewGuid().ToString().Replace("-","_")}.mp3";
                //        var fileName = $"{Guid.NewGuid().ToString().Replace(" - ", "_")}.mp3";
                //        relatePath += fileName;
                //        var _containerPath = $"{_locationName}\\{fileName}";
                //        // System.IO.File.WriteAllBytes(_containerPath, NauhaDTO.fileBinaries);
                //        using (var fileStream = new FileStream(_containerPath, FileMode.Create))
                //        {
                //            NauhaDTO.fileBinaries.CopyTo(fileStream);
                //        }

                //        //NauhaDTO.fileBinaries.
                //        localFileUrl = relatePath;
                //    }

                //}


            }

        }
        private bool CreateDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        private  string GetLocationName(int id)
        {
            var location = _context.Locations.Find(id);
            return location.LocationName;
        }
        private bool NauhaExists(int id)
        {
            return _context.Nauhas.Any(e => e.Id == id);
        }
        private Nauha TranslateNauhaDTOtoEntity(NauhaDTO nauhaDTO)
        {
            return new Nauha
            {
                LocationID = nauhaDTO.LocationDto.locationId,
                Lyricist = nauhaDTO.Lyricist,
                Orator = nauhaDTO.Orator,
                Title = nauhaDTO.Title,
                UrlPath = nauhaDTO.UrlPath,
                CreatedOn = nauhaDTO.CreatedOn,
                Id = nauhaDTO.Id,
                Year = nauhaDTO.Year
            };
        }
        #endregion

        #region PUT
        // PUT: api/Nauhas/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutNauha([FromRoute] int id, [FromBody] NauhaDTO nauha)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != nauha.Id)
            {
                return BadRequest();
            }
            var entity = _context.Nauhas.Find(id);
            if (entity == null) return NotFound();
            nauha.UrlPath = entity.UrlPath;
            entity = nauha.TranslateNauhaDTOtoEntity(nauha,entity);
            
            _context.Entry(entity).Property(x => x.UrlPath).IsModified = false;
            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!NauhaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //[Authorize]
        [HttpPut("approve")]
        // PUT: api/Nauhas/approve
        public async Task<IActionResult> PutApproved([FromBody] int[] ids)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            foreach (var id in ids)
            {
                var nauha = await _context.Nauhas.FindAsync(id);

                if (nauha == null)
                {
                    return NotFound();
                }
                nauha.Approved = true;
                _context.Entry(nauha).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;
                }
                
            }
            return NoContent();
        }
        #endregion

        #region POST
        // POST: api/Nauhas
        [HttpPost]
        public async Task<IActionResult> PostNauha([FromForm] NauhaDTO nauha)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                nauha.CreatedOn = DateTime.Now;
                
                CreateFileObject(nauha);
                nauha.fileBinaries = null;
                nauha.UrlPath = localFileUrl;
                _context.Nauhas.Add(nauha.TranslateNauhaDTOtoEntity(nauha));
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetNauha", new { id = nauha.Id }, nauha);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion
        

        #region DELETE
        // DELETE: api/Nauhas/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteNauha([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nauha = await _context.Nauhas.FindAsync(id);
            if (nauha == null)
            {
                return NotFound();
            }

            _context.Nauhas.Remove(nauha);
            await _context.SaveChangesAsync();

            return Ok(nauha);
        }
        #endregion




    }
}