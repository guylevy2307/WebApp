using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IpWebApp.Models
{
    public class Record
    {
        [Key]
        public int RecordId { get; set; }

        [Display(Name = "Title")]
        public string Name { get; set; }

        [Display(Name = "Application Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? ApplicationDate { get; set; }


        [Display(Name = "Registration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? RegistrationDate { get; set; }


        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        [Display(Name = "Renewal Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? RenewalDate { get; set; }


        [Display(Name = "Next Action Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? NextActionDate { get; set; }


        [Display(Name = "Record Type")]
        public RecordType Type { get; set; }

        [Display(Name = "Territory")]
        public string Country { get; set; }

        [Display(Name = "Client")]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public string Inventor { get; set; }

        public string Classes { get; set; }

        [Display(Name = "Expiration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? ExpirationDate { get; set; }

        public byte[] Image { get; set; }


        public RecordStatus Status { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        public virtual ICollection<Applicant> Applicants { get; set; }

        public bool Priority { get; set; }

        [Display(Name = "Priority Country")]
        public string PriorityCountry { get; set; }

        [Display(Name = "Priority Number")]
        public string PriorityNumber { get; set; }

        public int? ParentId { get; set; }
        public virtual Record Parent { get; set; }

        public virtual ICollection<Record> Divisionals { get; set; }


        [Display(Name = "Priority Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? PriorityDate { get; set; }

        public string creatorId { get; set; }

        public Record()
        {
            Applicants = new List<Applicant>();
            Divisionals = new List<Record>();
        }


    }

    public enum RecordType
    {
        Patent,
        Trademark,
        Design
    }

    public enum RecordStatus
    {
        YetToFile,
        Filed,
        InExamination,
        Published,
        Opposition,
        Granted,
        Abandoned,
        Expired,
        Transferred,
        Cancelled,
        Reinstating,
        Recorded,
        Accepted,
        FastTracked,
    }

}