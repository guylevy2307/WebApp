using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IpWebApp.Models
{
    public class Applicant
    {
        [Key]
        public int ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Formation { get; set; }
        public string POA { get; set; } 
        public virtual ICollection<Record> Records { get; set; }

        public Applicant()
        {
            Records = new List<Record>();
        }
    }
}