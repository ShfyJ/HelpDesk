using HelpDesk.DataAccess.Data;
using ITHelpDesk.DataAccess.Repository.IRepository;
using ITHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITHelpDesk.DataAccess.Repository
{
    public class RequestRepository : Repository<Request> , IRequestRepository
    {
        private readonly ApplicationDbContext _db;

        public RequestRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
