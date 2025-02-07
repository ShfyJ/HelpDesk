using HelpDesk.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;
using ITHelpDesk.Models;
using System.Linq;
using ITHelpDesk.Utility;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ITHelpDesk.DataAccess.Repository
{
    public class PythonBotService : IPythonBot
    {
        private readonly ApplicationDbContext _context;
        public PythonBotService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateCommand(Request cmd)
        {
            if (cmd==null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            _context.Request.Add(cmd);
        }

        public IEnumerable<Request> GetAllRequests()
        {
            return _context.Request.ToList();
        }

        public IEnumerable<Users> GetAllUsers()
        {
            List<Users> requestmakers = new List<Users>();
            var users =_context.User.ToList();
            var roles = _context.Roles.ToList();
            var UserRole = _context.UserRoles.ToList();
            foreach (var user in users)
            {

                var roleId = UserRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                if (user.Role == SD.Role_Requester)
                {
                    // Add items using Add method   
                    requestmakers.Add(user);
                }
            }
                return requestmakers;

        }

        public Request GetRequestById(int id)
        {
            return _context.Request.FirstOrDefault(r => r.RequestId == id);
        }

        public RequestMakers GetRequesterById(string id)
        {
            return _context.RequestMakers.FirstOrDefault(rq => rq.UserId == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
