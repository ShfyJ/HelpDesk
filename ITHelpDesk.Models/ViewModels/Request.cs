using System;
using System.Collections.Generic;
using System.Text;

namespace ITHelpDesk.Models
{
    public partial class Requests
    {
        public int RequestId { get; set; }
        public string RName { get; set; }
        public string RDescription { get; set; }
        public string RStatus { get; set; }
        public byte? RWeight { get; set; }
        public DateTime? RDateTime { get; set; }
        public int? RequestmakerId { get; set; }
        public int? AddressId { get; set; }
        public int? ManagerId { get; set; }
        public int? WorkerId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Manager Manager { get; set; }
        public virtual RequestMakers Requestmaker { get; set; }
        public virtual Workers Worker { get; set; }
    }
}
