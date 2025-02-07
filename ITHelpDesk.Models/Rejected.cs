using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace ITHelpDesk.Models
{
    public class Rejected
    {
        [Key]
        public int RejectedId { get; set; }
        [ForeignKey("RequestId")]
        public int? RequestId { get; set; }
        public int? WorkerId { get; set; }
        [ForeignKey("WorkerId")]
        public Workers Worker { get; set; }
        public Request Request { get; set; }

        [NotMapped]
        public DateTime rDateTime { get; set; }
    }
}