using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace ITHelpDesk.Models
{
    public partial class Manager
    {
        public Manager()
        {
            Request = new HashSet<Request>();
            Workers = new HashSet<Workers>();
        }

        [Key]
        public int ManagerId { get; set; }
        public string Flag { get; set; }
        public Boolean? Head { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]

        public virtual Users User { get; set; }
        public virtual ICollection<Request> Request { get; set; }
        public virtual ICollection<Workers> Workers { get; set; }
    }
}

