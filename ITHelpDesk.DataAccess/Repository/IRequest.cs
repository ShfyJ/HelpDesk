using ITHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITHelpDesk.DataAccess.Repository
{
    public interface IRequest
    {
       
        List<Request> GetNotifications(string flag, string rStatus);
    }
}
