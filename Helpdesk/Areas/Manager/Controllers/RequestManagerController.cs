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
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ITHelpDesk.Utility;
using ITHelpDesk.DataAccess.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;

namespace ITHelpDesk.Controllers
{
    [Area("Manager")]
    [Authorize]

    public class RequestManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> UserManager;
        string connectionString = "";
        private readonly IRequest _notiService;
        List<Request> _oNotifications = new List<Request>();


        public RequestManagerController(IRequest notiService, IConfiguration configuration, ApplicationDbContext context, UserManager<IdentityUser> userManager)//, IWorker worker)
        {
            _notiService = notiService;
            connectionString = configuration.GetConnectionString("ConnectionString");
            // _worker = worker;
            _context = context;
            UserManager = userManager;
        }

        public IActionResult AllNotifications()
        {
            return View();
        }

        [Authorize(Roles = "Manager, HeadManager, Employee, Requester")]
        //  [Authorize(Roles = SD.Role_Employee)]
        // [Authorize(Roles = SD.Role_Requester)]
        [HttpGet]
        public JsonResult GetNotifications()
        {
            string RStatus = "";
            var m = "";
            string Flag = "";
            int id;

            if (User.IsInRole(SD.Role_HeadManager))
            {
                _oNotifications = new List<Request>();
                _oNotifications = _notiService.GetAll();
            }
            else if (User.IsInRole(SD.Role_Employee))
            {
                RStatus = "yellow";
                id = _context.Workers.ToList().FirstOrDefault(u => u.UserId == UserManager.GetUserId(User)).WorkerId;
                _oNotifications = new List<Request>();
                _oNotifications = _notiService.GetEmployee(id, RStatus);
            }
            else if (User.IsInRole(SD.Role_Requester))
            {
                RStatus = "Taken";
                id = _context.RequestMakers.ToList().FirstOrDefault(u => u.UserId == UserManager.GetUserId(User)).RequestmakerId;
                _oNotifications = new List<Request>();
                _oNotifications = _notiService.GetRequester(id, RStatus);
            }
            else
            {
                RStatus = "blue";
                m = _context.Managers.ToList().FirstOrDefault(u => u.UserId == UserManager.GetUserId(User)).Flag;
                Flag = m;
                _oNotifications = new List<Request>();
                _oNotifications = _notiService.GetNotifications(Flag, RStatus);
            }



            return Json(_oNotifications);
        }

        public IActionResult Dashboard()
        {
            var mFlag = _context.Managers.ToList().FirstOrDefault(u => u.UserId == UserManager.GetUserId(User)).Flag;
            dynamic model = new ExpandoObject();
            
            model.Workers = _context.Workers.Include(u => u.Manager).Where(u => u.Manager.UserId == UserManager.GetUserId(User)).ToList();
            model.Requests = _context.Request.Include(u => u.Address).Include(u => u.Requestmaker).ThenInclude(r => r.User).Where(r => r.Address.Flag == mFlag).ToList();
            model.Users = _context.Users.ToList();
            //model.flag = _context.Managers.FirstOrDefault(m => m.UserId == UserManager.GetUserId(User)).Flag;

            return View(model);
        }


        // GET: Requests

        public async Task<IActionResult> Index(string searchBy, string search, int year, int month)
        {
            var mFlag = _context.Managers.ToList().FirstOrDefault(u => u.UserId == UserManager.GetUserId(User)).Flag;

            var uNG_HELPDESKContext = _context.Request.Include(r => r.Tasks).Include(r => r.Address).Include(r => r.Manager).ThenInclude(u => u.User).Include(r => r.Requestmaker).ThenInclude(u => u.User).Include(r => r.Worker).
                ThenInclude(u => u.User).Where(r => r.Address.Flag == mFlag).OrderByDescending(r => r.RequestId);//.Where(r=> (r.Address.Flag=="F" && r.ManagerId==1) || (r.Address.Flag == "S" && r.ManagerId == 2));

            var blue = _context.Request.Include(r => r.Address).Include(r => r.Manager).ThenInclude(u => u.User).Include(r => r.Requestmaker).ThenInclude(u => u.User).Include(r => r.Worker).
                ThenInclude(u => u.User).Where(r => r.Address.Flag == mFlag && r.RStatus == "blue").OrderByDescending(r => r.RequestId);

            ViewBag.Count = blue.Count();

            List<SelectListItem> y = new List<SelectListItem>();
            foreach (var i in _context.Request.Select(i => i.RDateTime.Year).Distinct())
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
            string n = "Все";
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


            if (searchBy == "WorkerName" && search != null)
            {
                return View(await uNG_HELPDESKContext.Where(r => r.Worker.User.FName.ToLower().StartsWith(search.ToLower())).ToListAsync());
                //return View(await uNG_HELPDESKContext.Where(r => r.WorkerId == search).ToListAsync());
            }

            if (year == 0)
            {
                var year1 = uNG_HELPDESKContext.Where(i => i.RDateTime.Year == DateTime.Now.Year);
                uNG_HELPDESKContext = (IOrderedQueryable<Request>)year1;
            }

            else if(year != 123 && year !=0)
            {
                var year1 = uNG_HELPDESKContext.Where(i => i.RDateTime.Year == year);
                uNG_HELPDESKContext = (IOrderedQueryable<Request>)year1;
            }
            if (month == 0)
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
            ViewData["mdm"] = month;


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
                .Include(r => r.Tasks)
                .Include(r => r.Requestmaker)
                .ThenInclude(r => r.User)
                .Include(r => r.Worker)
                .ThenInclude(r => r.User)
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
            //var request = await _context.Request.Include(a => a.Address).FirstOrDefaultAsync(m => m.RequestId == id);
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
        public async Task<IActionResult> Edit(int id, [Bind("RequestId,RName,Worker_Comment,review,Room,RDescription,RStatus,RWeight,RDateTime,RequestmakerId,TaskId,AddressId,ManagerId,WorkerId")] Request request)
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
            ViewData["TaskId"] = new SelectList(_context.Tasks, "TaskId", "child", request.TaskId);
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
