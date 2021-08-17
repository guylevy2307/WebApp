using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IpWebApp.Models
{
    public class TamplateTask
    {
        [Key]
        public int TamplateTaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [Display(Name = "Additional Days")]
        public int AddDays { get; set; }

        [Display(Name = "Additional Months")]
        public int AddMonths { get; set; }

        [Display(Name = "Additional Years")]
        public int AddYears { get; set; }

        [Range(0, 100000000)]
        public double Pricing { get; set; }
        public DateType dateType { get; set; }
    }
    public enum DateType
    {
        nextAction,
        RenewalDate,
        RegistrationDate
    }
}