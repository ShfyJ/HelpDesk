using HelpDesk.DataAccess.Data;
using ITHelpDesk.DataAccess.Repository.IRepository;
using ITHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITHelpDesk.DataAccess.Repository
{
    public class ManagerRepository : Repository<Manager>, IManagerRepository
    {
        private readonly ApplicationDbContext _db;

        public ManagerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
          
    }
}
