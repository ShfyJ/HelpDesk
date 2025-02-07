using System;
using System.Collections.Generic;
using System.Text;

namespace ITHelpDesk.DataAccess.Repository.Dto
{
    public class UsersDto

    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
    }
}
