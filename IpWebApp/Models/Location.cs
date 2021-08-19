using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IpWebApp.Models
{
    public class Location
    {
        [ForeignKey("Client")]
        public int LocationId { get; set; }
        [Required(ErrorMessage = "Please enter city name")]
        [Display(Name = "City Name")]
        public string PriorityCountry { get; set; }
        [Required(ErrorMessage = "Please enter city latitude")]
        public double Latitude { get; set; }
        [Required(ErrorMessage = "Please enter city longitude ")]
        public double Longitude { get; set; }
        
        public virtual Client Client { get; set; }
}
}