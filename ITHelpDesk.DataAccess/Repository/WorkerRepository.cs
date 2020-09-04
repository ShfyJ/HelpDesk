using HelpDesk.DataAccess.Data;
using ITHelpDesk.DataAccess.Repository.IRepository;
using ITHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITHelpDesk.DataAccess.Repository
{
    public class WorkerRepository : Repository<Workers>, IWorkerRepository
    {
        private readonly ApplicationDbContext _db;

        public WorkerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Workers worker)
        {
            var objFromDb = _db.Workers.FirstOrDefault(w => w.WorkerId == worker.WorkerId);
            //here update goes if necessary
            _db.SaveChanges();
        } 
    }
}
