using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MIKApi.DAL.DB;
using MIKApi.DAL.Models;
using MIKApi.DTO;

namespace MIKApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class VideosController : ControllerBase
    {
        private readonly MikContext _context;
        private IWebHostEnvironment _env;
        private readonly IConfiguration Configuration;
        public VideosController(MikContext context, IWebHostEnvironment env, IConfiguration Conf)
        {
            _context = context;
            _env = env;
            Configuration = Conf;
        }

        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetAll(ContentType type)
        {
            var _videos = await _context.Videos.Where(x => x.ContentType == type && x.Approved == true).OrderBy(a => a.Rank).ToListAsync();
            if (_videos != null && _videos.Count == 0)
            {
                return NotFound();
            }
            return Ok(_videos);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _videos = await _context.Videos.ToListAsync();
            if (_videos != null)
            {
                return NotFound();
            }
            return Ok(_videos);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _videos = await _context.Videos.FindAsync(id);
            if (_videos != null)
            {
                return NotFound();
            }
            return Ok(_videos);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Save([FromBody] VideoDTO videoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var video = new Video() {
                Approved = true, ChannelName = videoDto.ChannelName, CreatedOn = DateTime.Now,
                Description = videoDto.Description,
                EmbedUrl = videoDto.EmbedUrl, Tags = videoDto.Tags, Title = videoDto.Title };

            //if (videoDto.ContentType == "1")
            //{
            //    video.ContentType = ContentType.Live;
            //}
            //else
            //{
            //    video.ContentType = ContentType.Static;
            //}

            _context.Add(video);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = video.Id }, video);

        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody] VideoDTO videoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != videoDto.Id)
            {
                return BadRequest();
            }
            var entity = _context.Videos.Find(id);
            if (entity == null) return NotFound();
            entity = new VideoDTO().TranslateDtoToEntity(videoDto, entity);
            _context.Entry(entity).Property(x => x.Rank).IsModified = false;
            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                    throw ex;
               
            }
            return NoContent();

        }

    }
}
