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
        public int Request { get; set; }
        public int? Worker { get; set; }

    }
}