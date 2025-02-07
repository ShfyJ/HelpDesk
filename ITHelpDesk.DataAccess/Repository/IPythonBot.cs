using System;
using System.Collections.Generic;
using System.Text;
using ITHelpDesk.Models;
using System.Linq;
using ITHelpDesk.DataAccess.Repository.Dto;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ITHelpDesk.DataAccess.Repository
{
    public interface IPythonBot
    {
        bool SaveChanges();
        IEnumerable<Users> GetAllUsers();
        RequestMakers GetRequesterById(string id);
        IEnumerable<Request> GetAllRequests();
        Request GetRequestById(int id);
        void CreateCommand(Request cmd);
        //object GetRequesterById(RequestMakerDto id);
    }
}
