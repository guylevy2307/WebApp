using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IpWebApp.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        
        [Display(Name = "Client Name")]
        public string Name { get; set; }
        
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }
        
        [Display(Name = "Contact Email")]
        public string ContactEmail { get; set; }

        [Display(Name = "Contact Position")]
        public string ContactPosition { get; set; }

        [Display(Name = "Billing Contact Name")]
        public string BillingName { get; set; }
        
        [Display(Name = "Billing Contact Email")]
        public string BillingEmail { get; set; }
        
        public Currency? Currency { get; set; }
        
        [Display(Name = "VAT Number")]
        public string VatNumber { get; set; }
        
        public string Referent { get; set; }

        [Range(-1000000, 100000000)] 
        public double Balance { get; set; }

        public double? PaymentIssued { get; set; }

        public double? PaymentReceived { get; set; }
        [Display(Name = "Notes")]
        public string Notes { get; set; }
        public virtual ICollection<Record> Records { get; set; }

        public virtual Location Location { get; set; }
        public Client()
        {
            Records = new List<Record>();
        }

    }

    public enum Currency
    {
        ILS,
        USD,
        EUR
    }
}