using HelpDesk.DataAccess.Data;
using ITHelpDesk.DataAccess.Repository.IRepository;
using ITHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITHelpDesk.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            SP_Call = new SP_Call(_db);
            User = new UsersRepository(_db);
            Worker = new WorkerRepository(_db);
            Address = new AddressRepository(_db);
            Manager = new ManagerRepository(_db);
            Requester = new RequesterRepository(_db);
            Request = new RequestRepository(_db);
            Rejected = new RejectedRepository(_db);
        }

        public ISP_Call SP_Call { get; private set; }

        public IUsersRepository User { get; private set; }

        public IWorkerRepository Worker { get; private set; }

        public IAddressRepository Address { get; private set; }
        public IManagerRepository Manager { get; private set; }
        public IRequesterRepository Requester { get; private set; }

        public IRequestRepository Request { get; private set; }

        public IRejectedRepository Rejected { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
