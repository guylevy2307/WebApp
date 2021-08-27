using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IpWebApp.Models
{
    public class Subtask
    {
      
        [Key]
        public int SubtaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]

        public DateTime Deadline { get; set; }
        public TaskStatus Status { get; set; }
        public int MainTaskId { get; set; }
        public virtual Task MainTask { get; set; }

        public double Pricing { get; set; }
        public string creatorId { get; set; }

    }

}