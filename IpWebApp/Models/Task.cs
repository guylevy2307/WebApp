using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IpWebApp.Models
{
    public class Task
    {
        [Key]
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Deadline Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Deadline { get; set; }
        public TaskStatus Status { get; set; }
         public int RecordId { get; set; }
        public virtual Record Record { get; set; }

        [Range(0,100000000)]
        public double Pricing { get; set; }

        public string Assignee { get; set; }
        public string creatorId { get; set; }

        public virtual ICollection<Subtask> Subtasks { get; set; }
        public Task()
        {
            Subtasks = new List<Subtask>();
        }

        public String print()
        {
            String s = "********************************" + "\n";
            s += ("The task id is: " + TaskId +"\n");
            s += ("The Title  is: " + Title + "\n");
            s += ("The Description  is: " + Description + "\n");
            s += ("The Deadline  is: " + Deadline + "\n");
            s += ("The Status  is: " + Status + "\n");
            return s;
        }
    }

    //The status of a patent may be: filed, approved, denied, withdrawn, reexamined, reissued or became inactive.
    // Patent status can be grouped into four macro-categories: pending, PGPub, active and inactive
    public enum TaskStatus
    {
        Pending,
        Completed,
        Overdue
    }
}