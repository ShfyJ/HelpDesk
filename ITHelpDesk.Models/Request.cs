using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ITHelpDesk.Models
{
    public partial class Request
    {
        [Key]
        public int RequestId { get; set; }
        public string RName { get; set; }
        public string RDescription { get; set; }
        public string RStatus { get; set; }
        public byte? RWeight { get; set; }
        public DateTime? RDateTime { get; set; }
        public string Room { get; set; }
        public int? RequestmakerId { get; set; }
        public int? AddressId { get; set; }
        public int? ManagerId { get; set; }
        public int? WorkerId { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }
        [ForeignKey("ManagerId")]
        public virtual Manager Manager { get; set; }
        [ForeignKey("RequestmakerId")]
        public  RequestMakers Requestmaker { get; set; }
        [ForeignKey("WorkerId")]
        public virtual Workers Worker { get; set; }

    }
}
