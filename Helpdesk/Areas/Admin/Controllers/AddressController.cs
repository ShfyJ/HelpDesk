using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ITHelpDesk.DataAccess.Repository.IRepository;
using ITHelpDesk.Models;
using ITHelpDesk.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITHelpDesk.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class AddressController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Console.WriteLine("Address id in edit:" + id);
            Address address = new Address();
            if(id == null)
            {
                //this is for create
                return View(address);
            }
            //this is for edit
            address = _unitOfWork.Address.Get(id.GetValueOrDefault());
            if(address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Address address)
        {
            Console.WriteLine("Address id in update:" + address.AddressId);
            if (ModelState.IsValid)
            {
                Console.WriteLine("Address id:" + address.AddressId);
               // address.AddressId = 1;
                if (address.AddressId == 0) 
                {                 
                    _unitOfWork.Address.Add(address);
                }
                else
                {
                  
                  _unitOfWork.Address.Update(address);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            
            return View(address);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allobj = _unitOfWork.Address.GetAll();
            return Json(new { data = allobj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Address.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Address.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion
    }
}
