using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIKApi.DAL.Models
{
    public class Location
    {
        public int LocationID { get; set; }
        public string LocationName  { get; set; }
        public string SubLocationName { get; set; }
        public int LocationGroupId { get; set; }
    }

    public class LocationGroup
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
    }
}
