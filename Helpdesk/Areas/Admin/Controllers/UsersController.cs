using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.DataAccess.Data;
using ITHelpDesk.DataAccess.Repository;
using ITHelpDesk.DataAccess.Repository.IRepository;
using ITHelpDesk.Models;
using ITHelpDesk.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITHelpDesk.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;

        //public UsersController(IUnitOfWork unitOfWork)
        //{
           
        //    _unitOfWork = unitOfWork;
        //}
        public UsersController(ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            return View();
        }

        

        #region API CALLS
        
        [HttpGet]
        public IActionResult GetAll()
        {
            //var allobj = _unitOfWork.Address.GetAll();
            //return Json(new { data = allobj });
            var userList = _db.User.Include(u => u.Address).ToList();
           //var userList = _db.User.ToList();

            Console.WriteLine("Address is fetched");
            userList.ForEach(item => Console.Write(item + ","));
            foreach (var user in userList)
            {
                Console.WriteLine(user.Address.Building);
            }
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                if (user.Address == null)
                {
                    user.Address = new Address()
                    {
                        Building = "",
                        Block ="",
                        Flag =""
                    };
                }
            }
            var jsonlist = Json(new { data = userList }).Value;
            Console.WriteLine(jsonlist);
            return Json(new { data = userList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            Console.WriteLine("we are here");
            var objFromDb = _db.Users.FirstOrDefault(u => u.Id == id);
            if(objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }
            if(objFromDb.LockoutEnd!=null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Operation is Successfull" });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.User.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.User.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion
    }
}
