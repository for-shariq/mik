using Microsoft.AspNetCore.Http;
using MIKApi.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MIKApi.DTO
{
    public class NauhaDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }


        public string Lyricist { get; set; }
        public string Orator { get; set; }
        public DateTime Year { get; set; }
        public string UrlPath { get; set; }
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
        public IFormFile fileBinaries { get; set; }
        public LocationDTO LocationDto { get; set; }
        public bool Approved { get; set; }
        public uint? Rank { get; set; }
        public NauhaDTO()
        {
            if (LocationDto != null)
            {
                LocationDto = new LocationDTO();
            }
        }
        public Nauha TranslateNauhaDTOtoEntity(NauhaDTO nauhaDTO)
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
                Year = nauhaDTO.Year,
                Description = nauhaDTO.Description,
                Approved = nauhaDTO.Approved,
                Rank = nauhaDTO.Rank
            };
        }
        public Nauha TranslateNauhaDTOtoEntity(NauhaDTO nauhaDTO, Nauha nauha)
        {
            nauha.LocationID = nauhaDTO.LocationDto.locationId;
            nauha.Lyricist = nauhaDTO.Lyricist;
            nauha.Orator = nauhaDTO.Orator;
            nauha.Title = nauhaDTO.Title;
            nauha.UrlPath = nauhaDTO.UrlPath;
            nauha.CreatedOn = nauhaDTO.CreatedOn;
            nauha.Id = nauhaDTO.Id;
            nauha.Year = nauhaDTO.Year;
            nauha.Description = nauhaDTO.Description;
            nauha.Approved = nauhaDTO.Approved;
            nauha.Rank = nauhaDTO.Rank;
            return nauha;
        }
        public NauhaDTO TranslateEntityToDto(Nauha nauha, Location location)
        {
            return new NauhaDTO
            {
                Id = nauha.Id,
                CreatedOn = nauha.CreatedOn,
                LocationDto = new LocationDTO { locationId = location.LocationID, LocationName = location.LocationName },
                Lyricist = nauha.Lyricist,
                Orator = nauha.Orator,
                Title = nauha.Title,
                Year = nauha.Year,
                UrlPath = nauha.UrlPath,
                Description = nauha.Description,
                Approved = nauha.Approved,
                Rank = nauha.Rank
            };
        }
    }




    public class VideoDTO
    {
        public VideoDTO()
        {
            CreatedOn = DateTime.Now;
            Approved = false;
        }
       
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
       
        public string Title { get; set; }
        public string Description { get; set; }
        public string EmbedUrl { get; set; }
        public string ChannelName { get; set; }
        public ContentType ContentType { get; set; }
        public bool Approved { get; set; }
        public string Tags { get; set; }
        public uint? Rank { get; set; }
        public DateTime CreatedOn { get; set; }


        public Video TranslateDtoToEntity(VideoDTO dto, Video video)
        {
            video.Id = dto.Id;
            video.Rank = dto.Rank;
            video.Tags = dto.Tags;
            video.Title = dto.Title;
            video.Description = dto.Description;
            video.Approved = dto.Approved;
            video.ChannelName = dto.ChannelName;
            video.ContentType = dto.ContentType;
            video.EmbedUrl = dto.EmbedUrl;
            return video;

        }
    }

   
}
