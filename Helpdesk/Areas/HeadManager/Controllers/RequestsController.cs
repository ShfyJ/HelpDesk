using System;
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
using System.Dynamic;
using ITHelpDesk.DataAccess.Repository.IRepository;

namespace ITHelpDesk.Areas.HeadManager.Controllers
{
    [Area("HeadManager")]
    [Authorize(Roles = SD.Role_HeadManager)]
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public RequestsController(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
       
        // GET: HeadManager/Requests
        public async Task<IActionResult> Index(int year, int month)
        {
            List<SelectListItem> y = new List<SelectListItem>();
            foreach(var i in _context.Request.Select(i => i.RDateTime.Year).Distinct())
            {
                SelectListItem sel = new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                };
                SelectListItem s = sel;
                y.Add(s);
                
            }
            ViewData["y"] = y;
            string n="Все";
            List<SelectListItem> m = new List<SelectListItem>();
            foreach (var i in _context.Request.Select(i => i.RDateTime.Month).Distinct())
            { 
                if (i == 1)
                    n = "Январь";
                else if (i == 2)
                    n = "Февраль";
                else if (i == 3)
                    n = "Март";
                else if (i == 4)
                    n = "Апрель";
                else if (i == 5)
                    n = "Май";
                else if (i == 6)
                    n = "Июнь";
                else if (i == 7)
                    n = "Июль";
                else if (i == 8)
                    n = "Август";
                else if (i == 9)
                    n = "Сентябрь";
                else if (i == 10)
                    n = "Октябрь";
                else if (i == 11)
                    n = "Ноябрь";
                else if (i == 12)
                    n = "Декабрь";
                else n = "Все";
                SelectListItem sel = new SelectListItem
                {
                    Text = n,
                    Value = i.ToString(),
                };
                SelectListItem s = sel;
                m.Add(s);

            }
            ViewData["m"] = m;

            var uNG_HELPDESKContext = _context.Request.Include(r => r.Tasks).Include(r => r.Address).Include(r => r.Manager).Include(r => r.Requestmaker).ThenInclude(r => r.User).Include(r => r.Worker).ThenInclude(r => r.User).OrderByDescending(r => r.RequestId);
           
            
            
            if (year == 0)
            {
                var year1 = uNG_HELPDESKContext.Where(i => i.RDateTime.Year == DateTime.Now.Year);
                uNG_HELPDESKContext = (IOrderedQueryable<Request>)year1;
            }

            else if (year != 123 && year != 0)
            {
               var year1 =  uNG_HELPDESKContext.Where(i => i.RDateTime.Year == year);
               uNG_HELPDESKContext = (IOrderedQueryable<Request>)year1;
            }
            if(month == 0)
            {
                var month1 = uNG_HELPDESKContext.Where(i => i.RDateTime.Month == DateTime.Now.Month);
                uNG_HELPDESKContext = (IOrderedQueryable<Request>)month1;
            }
            else if (month != 13 && month != 0)
            {
               var month1 = uNG_HELPDESKContext.Where(i => i.RDateTime.Month == month);
                uNG_HELPDESKContext = (IOrderedQueryable<Request>)month1;

            }
            ViewData["ydy"] = year;
            ViewData["mdm"] =month;
            return View(await uNG_HELPDESKContext.ToListAsync());
        }

        public IActionResult Dashboard()
        {
            
            dynamic model = new ExpandoObject();
            model.Workers = _context.Workers.Include(u => u.Manager).Include(u => u.User).ThenInclude(a => a.Address).ToList();
            
            model.Requests = _context.Request.Include(u => u.Requestmaker).ThenInclude(r => r.User).ToList();
            model.Users = _context.Users.ToList();           

            return View(model);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetRequests()
        {

            var requests = _context.Request.ToList();

            foreach (var request in requests)
            {
                request.RName = null;
                request.RDescription = null;
                request.RWeight = null;
                request.RequestmakerId = null;
                request.Address = null;
                request.ManagerId = null;
                request.WorkerId = null;
                request.Completed_at = null;
                request.Room = null;
                
                            }
            
            return Json(new { data = requests });

        }

        [HttpGet]
        public IActionResult GetRejectedRequests()
        {

            var rRequests = _context.Rejecteds.Include(r => r.Request).ToList();
            if (rRequests != null)
            {
                foreach (var rRequest in rRequests.Where(r => r.RequestId != null))
                {
                    rRequest.rDateTime = rRequest.Request.RDateTime;
                }
            }

            return Json(new { data = rRequests });

        }

        #endregion

        // GET: HeadManager/Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Request
                .Include(r => r.Address)
                .Include(r => r.Manager)
                .ThenInclude(r => r.User)
                .Include(r => r.Requestmaker)
                .ThenInclude(r => r.User)
                .Include(r => r.Worker)
                .Include(r => r.Tasks)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: HeadManager/Requests/Create
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "AddressId");
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId");
            ViewData["RequestmakerId"] = new SelectList(_context.RequestMakers, "RequestmakerId", "RequestmakerId");
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "WorkerId");
            return View();
        }

        // POST: HeadManager/Requests/Create
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

        // GET: HeadManager/Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //var m = _context.Managers.ToList().FirstOrDefault(u => u.UserId == UserManager.GetUserId(User)).ManagerId;
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
            foreach (var item in _context.Workers.Include(w => w.Request).Include(w => w.User).Where(w => w.State == "working"))
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


            List<SelectListItem> tasks0 = new List<SelectListItem>();
            List<SelectListItem> tasks = new List<SelectListItem>();
            var Taskkk = _context.Tasks.OrderBy(x => x.type).ThenBy(w => w.child);
            foreach (var item in Taskkk)
            {
                //var group = new SelectListGroup { Name = item.type};
                SelectListItem selectListItem = new SelectListItem
                {
                    Text = item.child,
                    Value = item.TaskId.ToString(),
                    Group = new SelectListGroup { Name = item.type }
                };
                //SelectListItem task = selectListItem;
                tasks0.Add(selectListItem);
            }
            foreach (var t in tasks0.GroupBy(x => x.Group.Name))
            {
                foreach (var i in t)
                {
                    i.Group = t.First().Group;
                    tasks.Add(i);
                }

            }
            ViewBag.TaskId = tasks;


            return View(request);
        }

        // POST: HeadManager/Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestId,RName,Worker_Comment,review,Room,RDescription,RStatus,RWeight,RDateTime,RequestmakerId,AddressId,ManagerId,WorkerId, TaskId")] Request request)
        {
            if (id != request.RequestId)
            {

                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (request.TaskId != null && request.TaskId != 0)
                    {
                        var weight = _context.Tasks.ToList().FirstOrDefault(u => u.TaskId == request.TaskId).weight;

                        request.RWeight = (byte?)weight;
                        if (request.RStatus == "green")
                        {
                            request.RStatus = "green";
                        }
                        else
                        {
                            request.RStatus = "Taken";
                        }
                    }
                    else 
                    {
                        var address = _context.Address.FirstOrDefault(a => a.AddressId == request.AddressId).Flag;
                        var m = _context.Managers.FirstOrDefault(a => a.Flag == address);
                        var worker = request.WorkerId;
                        request.ManagerId = m.ManagerId;
                        request.RStatus = "yellow";
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
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "Full", request.AddressId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "WorkerId", request.WorkerId);
            return View(request);
        }

        // GET: HeadManager/Requests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Request
               .Include(r => r.Address)
               .Include(r => r.Manager)
               .ThenInclude(r => r.User)
               .Include(r => r.Requestmaker)
               .ThenInclude(r => r.User)
               .Include(r => r.Worker)
               .Include(r => r.Tasks)
               .FirstOrDefaultAsync(m => m.RequestId == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: HeadManager/Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.Request.FindAsync(id);
            var rejected = _context.Rejecteds.FirstOrDefault(w => w.RequestId == id);
            if (rejected != null)
            {
                _context.Rejecteds.Remove(rejected);
            }
            _context.Request.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Close(int id)
        {
            Request request = new Request();
            request = await _context.Request.FirstOrDefaultAsync(r => r.RequestId == id);
            request.RStatus = "green";
            if(id != request.RequestId)
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
            return View(request);
        }

        public async Task<IActionResult> UnClose(int id)
        {
            Request request = new Request();
            request = await _context.Request.FirstOrDefaultAsync(r => r.RequestId == id);
            request.RStatus = "Taken";
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
            return View(request);
        }

        private bool RequestExists(int id)
        {
            return _context.Request.Any(e => e.RequestId == id);
        }
    }
}
