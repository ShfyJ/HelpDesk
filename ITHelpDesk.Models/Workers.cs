using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ITHelpDesk.Models
{
    public class Workers
    {
        public Workers()
        {
            Request = new HashSet<Request>();
        }

        [Key]
        public int WorkerId { get; set; }
        public int? Score { get; set; }
        public int? NOfWorks { get; set; }
        public string State { get; set; }
        public int? Queue { get; set; }
        public string Flag { get; set; }

#nullable enable
        public string? UserId { get; set; }

        public int? ManagerId { get; set; }


        [ForeignKey("ManagerId")]
        public virtual Manager Manager { get; set; }
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
        public virtual ICollection<Request> Request { get; set; }
    }

}

