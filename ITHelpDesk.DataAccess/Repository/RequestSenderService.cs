using HelpDesk.DataAccess.Data;
using ITHelpDesk.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITHelpDesk
{
    public class RequestSenderService : IRequesSender
    {
        private ApplicationDbContext _context;
        public RequestSenderService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Request request)
        {
            _context.Add(request);
            _context.SaveChanges();
        }

        public Address GetAddress(int id)
        {
            var address_id = _context.Request.Include(request => request.AddressId)
                 .FirstOrDefault(request => request.AddressId == id)
                 .Address.AddressId;
            return (Address)_context.Address
                .Where(ad => ad.AddressId == address_id);

        }

        public IEnumerable<Request> GetAll()
        {
            return _context.Request.Include(address => address.AddressId);
        }

        public Request GetById(int id)
        {
            return GetAll()
                .FirstOrDefault(request => request.RequestId==id);
        }

        public string getDescription(int id)
        {
            throw new NotImplementedException();
        }

        public string getSender(int id)
        {
            throw new NotImplementedException();
        }

        public string getTitle(int id)
        {
            throw new NotImplementedException();
        }
    }
}
