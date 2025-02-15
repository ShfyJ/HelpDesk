﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITHelpDesk.Models;
using HelpDesk.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using ITHelpDesk.Utility;
using Microsoft.AspNetCore.Identity;
using ITHelpDesk.DataAccess.Repository.IRepository;

namespace ITHelpDesk.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = SD.Role_Employee)]

    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> UserManager;
        private readonly IUnitOfWork _unitOfWork;
        public RequestsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork)
        {
            UserManager = userManager;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        // GET: Employee/Requests
        public async Task<IActionResult> Index()
        {
            Console.WriteLine(UserManager.GetUserId(User));
            var uNG_HELPDESKContext = _context.Request.Include(r=>r.Tasks).Include(r => r.Address).Include(r => r.Manager)
                .ThenInclude(r => r.User).Include(r => r.Requestmaker).ThenInclude(r => r.User)
                .Include(r => r.Worker).ThenInclude(r => r.User).Where(r => r.Worker.UserId == UserManager.GetUserId(User)).OrderByDescending(r =>r.RequestId);
            ViewData["score"] = _context.Workers.FirstOrDefault(w => w.UserId == UserManager.GetUserId(User)).Score;
            return View(await uNG_HELPDESKContext.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Request
                .Include(r => r.Address)
                .Include(r => r.Manager)
                .Include(r => r.Requestmaker)
                .Include(r => r.Worker)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Employee/Requests/Create
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "AddressId");
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId");
            ViewData["RequestmakerId"] = new SelectList(_context.RequestMakers, "RequestmakerId", "RequestmakerId");
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "WorkerId");
            return View();
        }

        // POST: Employee/Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestId,RName,RDescription,RStatus,RWeight,RDateTime,RequestmakerId,AddressId,ManagerId,WorkerId")] Request request)
        {
            if (ModelState.IsValid)
            {
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "AddressId", request.AddressId);
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId", request.ManagerId);
            ViewData["RequestmakerId"] = new SelectList(_context.RequestMakers, "RequestmakerId", "RequestmakerId", request.RequestmakerId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "WorkerId", request.WorkerId);
            return View(request);
        }

        // GET: Employee/Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Request.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "AddressId", request.AddressId);
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId", request.ManagerId);
            ViewData["RequestmakerId"] = new SelectList(_context.RequestMakers, "RequestmakerId", "RequestmakerId", request.RequestmakerId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "WorkerId", request.WorkerId);
            return View(request);
        }

        // POST: Employee/Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestId, Worker_Comment,Room,review,RName,RDescription,RStatus,RWeight,RDateTime,TaskId,RequestmakerId,AddressId,ManagerId,WorkerId")] Request request)
        {
            if (id != request.RequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.RequestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "AddressId", request.AddressId);
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId", request.ManagerId);
            ViewData["RequestmakerId"] = new SelectList(_context.RequestMakers, "RequestmakerId", "RequestmakerId", request.RequestmakerId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "WorkerId", request.WorkerId);
            return View(request);
        }

        // GET: Employee/Requests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Request
                .Include(r => r.Address)
                .Include(r => r.Manager)
                .Include(r => r.Requestmaker)
                .Include(r => r.Worker)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: Employee/Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.Request.FindAsync(id);
            _context.Request.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestExists(int id)
        {
            return _context.Request.Any(e => e.RequestId == id);
        }

        #region API CALLS

        [HttpPost]
        public IActionResult Accept(int id)
        {
            var objFromDb = _unitOfWork.Request.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while accepting" });
            }

            objFromDb.RStatus = "Taken";
            _unitOfWork.Save();
            return Json(new { success = true, message = "Иш сизга юклатилди!" });

        }

        [HttpPost]
        public IActionResult Deny(int id)
        {
            Rejected rejected = new Rejected();

            var objFromDb = _unitOfWork.Request.Get(id);

            rejected.WorkerId = objFromDb.WorkerId;
            rejected.RequestId = objFromDb.RequestId;

            _unitOfWork.Rejected.Add(rejected);

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Рад етишда хатолик юз берди!" });
            }

            objFromDb.RStatus = "red";
            _unitOfWork.Save();
            return Json(new { success = true, message = "Рад етилди!" });

        }

        #endregion
    }
}
