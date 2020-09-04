using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ITHelpDesk.Models
{
    public class RequestMakers
    {
        public RequestMakers()
        {
            Request = new HashSet<Request>();
        }
        [Key]
        public int RequestmakerId { get; set; }
        #nullable enable
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
        public virtual ICollection<Request> Request { get; set; }

    }
}
