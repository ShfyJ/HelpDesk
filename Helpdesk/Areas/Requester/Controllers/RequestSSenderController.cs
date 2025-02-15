﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITHelpDesk.Models;
using HelpDesk.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using ITHelpDesk.Utility;
using System.Runtime.InteropServices;

namespace ITHelpDesk.Controllers
{
    [Area("Requester")]
    [Authorize(Roles = SD.Role_Requester)]
    public class RequestSSenderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> SignInManager;
        private readonly UserManager<IdentityUser> UserManager;
        
        public RequestSSenderController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            SignInManager = signInManager;
            UserManager = userManager;

        }


        // GET: Requests

        public async Task<IActionResult> Index()
        {
            var uNG_HELPDESKContext = _context.Request.Include(t => t.Tasks).Include(r => r.Address).Include(r => r.Manager).ThenInclude(u => u.User).
                           Include(r => r.Requestmaker).Include(r => r.Worker).ThenInclude(u => u.User).Where(r => r.Requestmaker.UserId == UserManager.GetUserId(User)).OrderByDescending(r => r.RequestId)
                           .Select(p => new Request
                           {
                               RequestId = p.RequestId,
                               RName = p.RName,
                               RDescription = p.RDescription,
                               RStatus = p.RStatus,
                               RWeight = p.RWeight,
                               RDateTime = p.RDateTime,
                               review = p.review,
                               Worker_Comment = p.Worker_Comment,
                               JavascriptToRun = p.JavascriptToRun,
                               Room = p.Room,
                               AddressId = p.AddressId,
                    //date = p.date,
                               TaskId = p.TaskId,
                               Tasks = p.Tasks,
                               Address = p.Address,
                               Requestmaker = p.Requestmaker,
                               Manager = p.Manager,
                               Worker = p.Worker,
                               RequestmakerId = p.RequestmakerId,
                               ManagerId = p.ManagerId,
                               WorkerId = p.WorkerId,
                               Completed_at = p.Completed_at,
                               YourOrder = _context.Request.Include(w => w.Worker).ThenInclude(w => w.Request).Where(r => r.WorkerId == p.WorkerId && (DateTime.Compare(p.RDateTime.Date, r.RDateTime.Date) > 0) && (p.RStatus == "yellow" || p.RStatus == "Taken") && r.RStatus == p.RStatus).Count() + 1,
                           });
            return View(await uNG_HELPDESKContext.ToListAsync());
        }

        // GET: Requests/Details/5
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

        // GET: Requests/Create
        public IActionResult Create()
        {
            var AddressID = _context.User.Include(i => i.Address).ToList().FirstOrDefault(u => u.Id == UserManager.GetUserId(User)).Address.Full;
          //  string a = _context.Address.ToList().FirstOrDefault(i => i.AddressId == AddressID).Full;
            //ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "Full");
            List<SelectListItem> address = new List<SelectListItem>();
            foreach (var item in _context.Address)
            {
                SelectListItem sel = new SelectListItem
                {
                    Text = item.Full,
                    Value = item.AddressId.ToString(),
                    Selected = item.Full == AddressID,
                };
                SelectListItem s = sel;
                address.Add(s);
            }
            ViewData["AddressId"] = address;

            

            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId");
            ViewData["RequestmakerId"] = new SelectList(_context.RequestMakers, "RequestmakerId", "RequestmakerId");
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "WorkerId");
            
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestId,RName,RDescription,AddressId, Room, ManagerId")] Request request)
        {
            if (ModelState.IsValid)
            {
                var requestMakerList = _context.RequestMakers.ToList();
                // var managerList = _context.Managers.ToList();

                request.RequestmakerId = requestMakerList.FirstOrDefault(u => u.UserId == UserManager.GetUserId(User)).RequestmakerId;
                request.RDateTime = DateTime.Now;
                request.RStatus = "blue";
                _context.Add(request);

                await _context.SaveChangesAsync();


                //request.ManagerId = managerList.FirstOrDefault(u => u.Flag == request.Address.Flag).ManagerId;
                //_context.Add(request);

                //await _context.SaveChangesAsync();
               
                
                
                return RedirectToAction(nameof(Index));
            }

            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "AddressId", request.AddressId);
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId", request.ManagerId);
            ViewData["RequestmakerId"] = new SelectList(_context.RequestMakers, "RequestmakerId", "RequestmakerId", request.RequestmakerId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "WorkerId", request.WorkerId);

           

            return View(request);
        }

        // GET: Requests/Edit/5
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
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "Full", request.AddressId);
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId", request.ManagerId);
            ViewData["RequestmakerId"] = new SelectList(_context.RequestMakers, "RequestmakerId", "RequestmakerId", request.RequestmakerId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "WorkerId", request.WorkerId);
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(bool Done, int id, [Bind("RequestId,Worker_Comment,review,RName,RDescription,Room,RStatus,RWeight,RDateTime,TaskId,RequestmakerId,AddressId,ManagerId,WorkerId")] Request request)
        {
            if (id != request.RequestId)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    if (Done == true)
                    {
                        request.RStatus = "green";

                        _context.Update(request);
                        await _context.SaveChangesAsync();
                    }
                    else if (Done == false && request.WorkerId == null)
                    {
                        request.RStatus = "blue";
                        _context.Update(request);
                        await _context.SaveChangesAsync();
                    }
                    else if (Done == false && request.WorkerId != null && request.Worker_Comment == null)
                    {
                        if (request.RStatus == "Taken") {
                            request.RStatus = "Taken";
                        }
                        else 
                        {
                            request.RStatus = "yellow";
                        }
                            
                        _context.Update(request);
                        await _context.SaveChangesAsync();
                    }
                    else if (Done == false && request.WorkerId != null && request.Worker_Comment != null)
                    {
                        
                        request.RStatus = "Taken";
                        _context.Update(request);
                        await _context.SaveChangesAsync();
                    }
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

        // GET: Requests/Delete/5
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

        // POST: Requests/Delete/5
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
    }
}






//namespace ITHelpDesk.Controllers
//{
//    [Area("Requester")]
//    [Authorize(Roles = SD.Role_Requester)]
//    public class RequestSSenderController : Controller
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly SignInManager<IdentityUser> SignInManager;
//        private readonly UserManager<IdentityUser> UserManager;

//        public RequestSSenderController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
//        {
//            _context = context;
//            SignInManager = signInManager;
//            UserManager = userManager;

//        }


//        // GET: Requests

//        public async Task<IActionResult> Index()
//        {

//            var uNG_HELPDESKContext = _context.Request.Include(r => r.Address).Include(r => r.Manager).
//                Include(r => r.Requestmaker).Include(r => r.Worker).Where(r => r.Requestmaker.UserId == UserManager.GetUserId(User));
//            return View(await uNG_HELPDESKContext.ToListAsync());
//        }

//        // GET: Requests/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var request = await _context.Request
//                .Include(r => r.Address)
//                .Include(r => r.Manager)
//                .Include(r => r.Requestmaker)
//                .Include(r => r.Worker)
//                .FirstOrDefaultAsync(m => m.RequestId == id);
//            if (request == null)
//            {
//                return NotFound();
//            }

//            return View(request);
//        }

//        // GET: Requests/Create
//        public IActionResult Create()
//        {
//            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "Full");


//            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId");
//           ViewData["RequestmakerId"] = new SelectList(_context.RequestMakers, "RequestmakerId", "RequestmakerId");
//            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "WorkerId");
//            return View();
//        }

//        // POST: Requests/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.     
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("RequestId,RName,RDescription,AddressId")] Request request)
//        {
//            if (ModelState.IsValid)
//            {
//                var requestMakerList= _context.RequestMakers.ToList();

//               // var managerList = _context.Managers.ToList();

//                request.RequestmakerId = requestMakerList.FirstOrDefault(u => u.UserId == UserManager.GetUserId(User)).RequestmakerId;
//                request.RDateTime = DateTime.Now;
//                request.RStatus = "blue";
//                _context.Add(request);

//                await _context.SaveChangesAsync();
//                //request.ManagerId = managerList.FirstOrDefault(u => u.Flag == request.Address.Flag).ManagerId;
//                //_context.Add(request);

//                //await _context.SaveChangesAsync();

//                return RedirectToAction(nameof(Index));
//            }

//            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "AddressId", request.AddressId);
//            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId", request.ManagerId);
//            ViewData["RequestmakerId"] = new SelectList(_context.RequestMakers, "RequestmakerId", "RequestmakerId", request.RequestmakerId);
//            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "WorkerId", request.WorkerId);
//            return View(request);
//        }

//        // GET: Requests/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var request = await _context.Request.FindAsync(id);
//            if (request == null)
//            {
//                return NotFound();
//            }
//            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "Full", request.AddressId);
//            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId", request.ManagerId);
//            ViewData["RequestmakerId"] = new SelectList(_context.RequestMakers, "RequestmakerId", "RequestmakerId", request.RequestmakerId);
//            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "WorkerId", request.WorkerId);
//            return View(request);
//        }

//        // POST: Requests/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(bool Done, int id, [Bind("RequestId,RName,RDescription,RStatus,RWeight,RDateTime,RequestmakerId,AddressId,ManagerId,WorkerId")] Request request)
//        {
//            if (id != request.RequestId)
//            {
//                return NotFound();
//            }


//            if (ModelState.IsValid)
//                        {
//                            try
//                            {
//                                if (Done == true)
//                                {
//                                    request.RStatus = "green";

//                                    _context.Update(request);
//                                    await _context.SaveChangesAsync();
//                                }
//                                else if (Done == false && request.WorkerId == null)
//                                {
//                                    request.RStatus = "blue";
//                                    _context.Update(request);
//                                    await _context.SaveChangesAsync();
//                                }
//                                else if (Done == false && request.WorkerId != null)
//                                {
//                                    request.RStatus = "yellow";
//                                    _context.Update(request);
//                                    await _context.SaveChangesAsync();
//                                }
//                                _context.Update(request);
//                                await _context.SaveChangesAsync();
//                            }
//                            catch (DbUpdateConcurrencyException)
//                            {
//                                if (!RequestExists(request.RequestId))
//                                {
//                                    return NotFound();
//                                }
//                                else
//                                {
//                                    throw;
//                                }
//                            }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "AddressId", request.AddressId);
//            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId", request.ManagerId);
//            ViewData["RequestmakerId"] = new SelectList(_context.RequestMakers, "RequestmakerId", "RequestmakerId", request.RequestmakerId);
//            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "WorkerId", request.WorkerId);
//            return View(request);
//        }

//        // GET: Requests/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var request = await _context.Request
//                .Include(r => r.Address)
//                .Include(r => r.Manager)
//                .Include(r => r.Requestmaker)
//                .Include(r => r.Worker)
//                .FirstOrDefaultAsync(m => m.RequestId == id);
//            if (request == null)
//            {
//                return NotFound();
//            }

//            return View(request);
//        }

//        // POST: Requests/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var request = await _context.Request.FindAsync(id);
//            _context.Request.Remove(request);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool RequestExists(int id)
//        {
//            return _context.Request.Any(e => e.RequestId == id);
//        }
//    }
//}
