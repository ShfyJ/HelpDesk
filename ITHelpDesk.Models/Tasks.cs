using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace ITHelpDesk.Models
{
    public class Tasks
    {
        [Key]
        public int TaskId { get; set; }
        public string type { get; set; }
        public string child { get; set; }
        public int weight { get; set; }
        public ICollection<Request> Requests { get; set; }
    }
}
