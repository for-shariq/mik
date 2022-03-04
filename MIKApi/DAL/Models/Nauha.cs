using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MIKApi.DAL.Models
{
    public class Nauha
    {
        public Nauha()
        {
            CreatedOn = DateTime.Now;
            Approved = false;
        }

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [Column(TypeName = "varchar(500)")]
        public string Title { get; set; }

        public int LocationID { get; set; }
        public string Lyricist { get; set; }
        public string Orator { get; set; }
        public string Description { get; set; }
        public DateTime Year { get; set; }
        public string UrlPath { get; set; }
    
        public DateTime CreatedOn { get; set; }
        public bool Approved { get; set; }
        public uint? Rank  { get; set; }

    }


    public class Video
    {
        public Video()
        {
            CreatedOn = DateTime.Now;
            Approved = false;
        }
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [Column(TypeName = "varchar(500)")]
        public string Title { get; set; }
        public string Description { get; set; }
        public string EmbedUrl { get; set; }
        public string ChannelName { get; set; }
        public ContentType ContentType { get; set; }
        public bool Approved { get; set; }
        public string Tags { get; set; }
        public uint? Rank { get; set; }
        public DateTime CreatedOn { get; set; }

    }

    public enum ContentType
    { 
        Static = 0,
        Live = 1
    }
}
