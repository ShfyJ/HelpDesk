using HelpDesk.DataAccess.Data;
using ITHelpDesk.DataAccess.Repository.IRepository;
using ITHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITHelpDesk.DataAccess.Repository
{
    public class RejectedRepository : Repository<Rejected>, IRejectedRepository
    {
        private readonly ApplicationDbContext _db;

        public RejectedRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
