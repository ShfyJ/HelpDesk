using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITHelpDesk.Models;
using ITHelpDesk.Models.ViewModels;

using System.Drawing.Printing;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Storage;
using System.IO.Pipelines;
using HelpDesk.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ITHelpDesk.Utility;

namespace ITHelpDesk.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = SD.Role_Manager)]
    public class RequestManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> UserManager;


        public RequestManagerController(ApplicationDbContext context, UserManager<IdentityUser> userManager)//, IWorker worker)
        {
            // _worker = worker;
            _context = context;
            UserManager = userManager;
        }


        // GET: Requests
        public async Task<IActionResult> Index(string searchBy, string search)
        {
            var mFlag = _context.Managers.ToList().FirstOrDefault(u => u.UserId == UserManager.GetUserId(User)).Flag;

            var uNG_HELPDESKContext = _context.Request.Include(r => r.Address).Include(r => r.Manager).ThenInclude(u => u.User).Include(r => r.Requestmaker).ThenInclude(u => u.User).Include(r => r.Worker).
                ThenInclude(u => u.User).Where(r => r.Address.Flag == mFlag).OrderByDescending(r => r.RequestId);//.Where(r=> (r.Address.Flag=="F" && r.ManagerId==1) || (r.Address.Flag == "S" && r.ManagerId == 2));

            if (searchBy == "WorkerName" && search != null)
            {
                return View(await uNG_HELPDESKContext.Where(r => r.Worker.User.FName.ToLower().StartsWith(search.ToLower())).ToListAsync());
                //return View(await uNG_HELPDESKContext.Where(r => r.WorkerId == search).ToListAsync());
            }
            //if (searchBy == "WorkerName" &&  searchBy == null) { return View(await uNG_HELPDESKContext.ToListAsync()); }
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
            ViewData["AddressId"] = new SelectList(_context.Workers, "WorkerId", "WorkerId");
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestId,RName,RDescription,AddressId")] Request request)
        {
            if (ModelState.IsValid)
            {
                request.RDateTime = DateTime.Now;
                request.RStatus = "Blue";
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

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var m = _context.Managers.ToList().FirstOrDefault(u => u.UserId == UserManager.GetUserId(User)).ManagerId;
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


            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in _context.Workers.Include(w => w.Request).Include(w => w.User).Where(w => w.State == "working" && w.ManagerId == m))
            {
                SelectListItem selectListItem = new SelectListItem
                {
                    Text = item.User.FName + " #" + item.Queue,
                    Value = item.WorkerId.ToString(),

                };
                SelectListItem sel = selectListItem;
                items.Add(sel);
            }
            ViewBag.WorkerId = items;
            //ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "User.Fname", request.WorkerId);
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestId,RName,Room,RDescription,RStatus,RWeight,RDateTime,RequestmakerId,AddressId,ManagerId,WorkerId")] Request request)
        {


            if (id != request.RequestId)
            {

                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var m = _context.Managers.ToList().FirstOrDefault(u => u.UserId == UserManager.GetUserId(User)).ManagerId;

                    //_context.AddAsync(request);
                    request.ManagerId = m;
                    request.RStatus = "yellow";
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
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "Full", request.AddressId);
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
                .Include(r => r.Worker).ThenInclude(r => r.User)
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
//    [Area("Manager")]
//    [Authorize(Roles = SD.Role_Manager)]
//    public class RequestManagerController : Controller
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly UserManager<IdentityUser> UserManager;

//        //private IWorker _worker;

//        public RequestManagerController(ApplicationDbContext context, UserManager<IdentityUser> userManager)//, IWorker worker)
//        {
//           // _worker = worker;
//            _context = context;
//            UserManager = userManager;
//        }


//        // GET: Requests
//        public async Task<IActionResult> Index(string searchBy, string search)
//        {
//            var mFlag = _context.Managers.ToList().FirstOrDefault(u => u.UserId == UserManager.GetUserId(User)).Flag;

//            var uNG_HELPDESKContext = _context.Request.Include(r => r.Address).Include(r => r.Manager).ThenInclude(u => u.User).Include(r => r.Requestmaker).ThenInclude(u => u.User).Include(r => r.Worker).
//                ThenInclude(u=>u.User).Where(r => r.Address.Flag == mFlag).OrderByDescending(r => r.RequestId);//.Where(r=> (r.Address.Flag=="F" && r.ManagerId==1) || (r.Address.Flag == "S" && r.ManagerId == 2));

//            if (searchBy == "WorkerName" && search != null)
//            {
//                return View(await uNG_HELPDESKContext.Where(r=> r.Worker.User.FName.ToLower().StartsWith(search.ToLower())).ToListAsync());
//                //return View(await uNG_HELPDESKContext.Where(r => r.WorkerId == search).ToListAsync());
//            }
//            //if (searchBy == "WorkerName" &&  searchBy == null) { return View(await uNG_HELPDESKContext.ToListAsync()); }
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
//            //ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "Full");


//            // ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId");
//            // ViewData["RequestmakerId"] = new SelectList(_context.RequestMakers, "RequestmakerId", "RequestmakerId");
//            ViewData["AddressId"] = new SelectList(_context.Workers, "WorkerId", "WorkerId");
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
//                request.RDateTime = DateTime.Now;
//                request.RStatus = "Blue";
//                _context.Add(request);
//                await _context.SaveChangesAsync();
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
//            var m = _context.Managers.ToList().FirstOrDefault(u => u.UserId == UserManager.GetUserId(User)).ManagerId;
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
//            //ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "AddressId", request.AddressId);
//            //ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId", request.ManagerId);
//            //ViewData["RequestmakerId"] = new SelectList(_context.RequestMakers, "RequestmakerId", "RequestmakerId", request.RequestmakerId);


//            List<SelectListItem> items = new List<SelectListItem>();
//            foreach (var item in _context.Workers.Include(w => w.Request).Include(w => w.User).Where(w => w.State == "working" && w.ManagerId == m))
//            //foreach(var item in _context.Workers.Include(w=>w.User))
//            {
//                SelectListItem selectListItem = new SelectListItem
//                {
//                    Text = item.User.FName + " #" + item.Queue,
//                    Value = item.WorkerId.ToString(),

//                };
//                SelectListItem sel = selectListItem;
//                items.Add(sel);
//            }
//            ViewBag.WorkerId = items;
//            //ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "User.Fname", request.WorkerId);
//            return View(request);
//        }

//        // POST: Requests/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("RequestId,RName,RDescription,RStatus,RWeight,RDateTime,RequestmakerId,AddressId,ManagerId,WorkerId")] Request request)
//        {
//           // var workers = _worker.GetWorkers();
//            //var workerModel = workers.Select(w => new WorkerDetailModel
//            //{   
//             //   R_id = w.RequestId,
//              //  id = w.Worker.WorkerId,
//               // FirstName = w.Worker.User.Fname,
//                //LastName = w.Worker.User.Lname,
//                //score = w.Worker.Score,
//                //State = w.Worker.State,

//            //}).ToList();

//        //    var Wmodel = new WorkerIndexModel()
//         //   {
//          //      Workers = workerModel
//           // };

//            if (id != request.RequestId)
//            {

//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {

//                    var m = _context.Managers.ToList().FirstOrDefault(u => u.UserId == UserManager.GetUserId(User)).ManagerId;

//                    //_context.AddAsync(request);
//                    request.ManagerId = m;
//                    request.RStatus = "yellow";
//                    _context.Update(request);
//                    await _context.SaveChangesAsync();

//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!RequestExists(request.RequestId))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "Full", request.AddressId);
//            //ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "AddressId", request.AddressId);
//            //ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId", request.ManagerId);
//            //ViewData["RequestmakerId"] = new SelectList(_context.RequestMakers, "RequestmakerId", "RequestmakerId", request.RequestmakerId);
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
//                .Include(r => r.Worker).ThenInclude(r => r.User)
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
