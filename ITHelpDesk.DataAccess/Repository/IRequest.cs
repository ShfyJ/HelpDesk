using ITHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITHelpDesk.DataAccess.Repository
{
    public interface IRequest
    {

        List<Request> GetRequester(int id, string rStatus);
        List<Request> GetEmployee(int id, string rStatus);
        List<Request> GetNotifications(string flag, string rStatus);
        List<Request> GetEvents();
        List<Request> GetAll();
    }
}
