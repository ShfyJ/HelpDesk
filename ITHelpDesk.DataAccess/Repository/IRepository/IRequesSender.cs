using ITHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITHelpDesk
{
    public interface IRequesSender
    {
        IEnumerable<Request> GetAll();
        Request GetById(int id);
        void Add(Request request);
        string getSender(int id);
        string getTitle(int id);
        string getDescription(int id);
        Address GetAddress(int id);

    }
}
