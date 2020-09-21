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
using Microsoft.AspNetCore.Identity;
using System.Dynamic;

namespace ITHelpDesk.Areas.Admin.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = SD.Role_Manager)]
    public class WorkersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> UserManager;
        public WorkersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            UserManager = userManager;
        }

        // GET: Admin/Workers
        public async Task<IActionResult> Index()
        {
            

            var Manager = _context.Managers.ToList().FirstOrDefault(u => u.UserId == UserManager.GetUserId(User)).ManagerId;

            var uNG_HELPDESKContext = _context.Workers.Include(w => w.Manager).Where(w => w.ManagerId == Manager).Include(w => w.User);
            return View(await uNG_HELPDESKContext.ToListAsync());
        
    }

        //Employee Contacts Action
        public IActionResult Contacts()
        {
            dynamic model = new ExpandoObject();
            model.Workers = _context.Workers.Include(r => r.Manager).Include(r => r.Request).Include(r => r.User).ThenInclude(u => u.Address).ToList();
            model.Requests = _context.Request.ToList();
            return View(model);
        }

        // GET: Admin/Workers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workers = await _context.Workers
                .Include(w => w.Manager)
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.WorkerId == id);
            if (workers == null)
            {
                return NotFound();
            }

            return View(workers);
        }

        // GET: Admin/Workers/Create
        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId");
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Admin/Workers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkerId,Score,NOfWorks,State,Queue,Id,ManagerId")] Workers workers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId", workers.ManagerId);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", workers.UserId);
            return View(workers);
        }

        // GET: Admin/Workers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workers = await _context.Workers.FindAsync(id);
            if (workers == null)
            {
                return NotFound();
            }
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId", workers.ManagerId);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", workers.UserId);
            return View(workers);
        }

        // POST: Admin/Workers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkerId,Score,NOfWorks,State,Queue,Id,ManagerId")] Workers workers)
        {
            if (id != workers.WorkerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkersExists(workers.WorkerId))
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
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId", workers.ManagerId);
            ViewData["UserId"] = new SelectList(_context.Users, "UerId", "UserId", workers.UserId);
            return View(workers);
        }

        // GET: Admin/Workers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workers = await _context.Workers
                .Include(w => w.Manager)
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.WorkerId == id);
            if (workers == null)
            {
                return NotFound();
            }

            return View(workers);
        }

        // POST: Admin/Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workers = await _context.Workers.FindAsync(id);
            _context.Workers.Remove(workers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkersExists(int id)
        {
            return _context.Workers.Any(e => e.WorkerId == id);
        }
    }
}
