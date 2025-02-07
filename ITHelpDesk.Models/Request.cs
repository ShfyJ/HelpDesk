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
        public DateTime RDateTime { get; set; }
        public string review { get; set; }
        public string Worker_Comment { get; set; }
        [NotMapped]
        public int Year { get; set; }
        [NotMapped]
        public int Month { get; set; }
        [NotMapped]
        public int YourOrder { get; set; }
        [NotMapped]
        public string JavascriptToRun { get; set;}
        public string date => RDateTime.ToString("dd-MMM-yyy HH:mm");
        public DateTime? Completed_at { get; set; } = new DateTime(00-00-00, 00-00);
        //[NotMapped]
        //public string completed => Completed_at.ToString("dd-MMM-yyy HH:mm");
        public string Room { get; set; }
        public int? TaskId { get; set; }
        
        public int? RequestmakerId { get; set; }
        public int? AddressId { get; set; }
        public int? ManagerId { get; set; }
        public int? WorkerId { get; set; }
        
        [ForeignKey("TaskId")]
        public virtual Tasks Tasks { get; set; }

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
