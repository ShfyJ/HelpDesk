using System;
using System.Collections.Generic;
using System.Text;

namespace ITHelpDesk.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
       
        ISP_Call SP_Call { get; }
        IUsersRepository User { get; }
        IWorkerRepository Worker { get; }

        IAddressRepository Address { get; }
        IManagerRepository Manager { get; }

        IRequesterRepository Requester { get; }

        IRequestRepository Request { get; }

        IRejectedRepository Rejected { get; }
        void Save();
    }
}
