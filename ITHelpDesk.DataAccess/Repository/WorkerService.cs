using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.DataAccess.Data;
using ITHelpDesk;
using ITHelpDesk.Models;
using Microsoft.EntityFrameworkCore;

namespace ITHelpDesk
{
    class WorkerService : IWorker
    {
        private ApplicationDbContext _context;
        public WorkerService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Workers Get(int id)
        {
            return GetAll().FirstOrDefault(w => w.WorkerId == id);
        
        }
        //public Request GetR_W(int id)
        //{
         //   return GetWorkers().FirstOrDefault(w => w.RequestId == id);
       // }

        public IEnumerable<Workers> GetAll()
        {
            return _context.Workers.Include(w => w.User);
        }

        //public IEnumerable<Request> GetWorkers()
        //{
         //   return _context.Request.Include(w => w.Worker).ThenInclude(w => w.User);
        //}
        public IEnumerable<Users> GetUsers(int id)
        {
            var userID = Get(id).
                User.Id;
            return _context.User.Where(w => w.Id == userID);
        }
    }
}
