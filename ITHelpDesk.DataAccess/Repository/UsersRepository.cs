using HelpDesk.DataAccess.Data;
using ITHelpDesk.DataAccess.Repository.IRepository;
using ITHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHelpDesk.DataAccess.Repository
{
    public class UsersRepository : Repository<Users>, IUsersRepository
    {
        private readonly ApplicationDbContext _db;
        public UsersRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Users user)
        {
            var objFromDb = _db.User.FirstOrDefault(s => s.Id == user.Id);

            if (objFromDb != null)
            {
                objFromDb.FName = user.FName;
                

            }

         
        }
    }
}
