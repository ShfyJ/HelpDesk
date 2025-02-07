using HelpDesk.DataAccess.Data;
using ITHelpDesk.DataAccess.Repository.IRepository;
using ITHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITHelpDesk.DataAccess.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        private readonly ApplicationDbContext _db;

        public AddressRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Address address)
        {
            var objFromDb = _db.Address.FirstOrDefault(s => s.AddressId == address.AddressId);
            
            if(objFromDb != null)
            {
                objFromDb.Block = address.Block;
                objFromDb.Building = address.Building;
                objFromDb.Flag = address.Flag;
                
            }
           
           
        }
    }
}
