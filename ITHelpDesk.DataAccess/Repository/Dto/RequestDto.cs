using System;
using System.Collections.Generic;
using System.Text;

namespace ITHelpDesk.DataAccess.Repository.Dto
{
        public class RequestDto
    {
        //public int RequestId { get; set; }
        public string RName { get; set; }
        public string RDescription { get; set; }
        public string RStatus { get; set; }
        public string RDateTime { get; set; }

        public string Room { get; set; }
        public int? RequestmakerId { get; set; }
        public int AddressId { get; set; }
    }
}
