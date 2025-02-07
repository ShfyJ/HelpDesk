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
using System.IO;
using Microsoft.AspNetCore.Http;

namespace ITHelpDesk.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = SD.Role_Employee)]
    public class WorkersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> UserManager;
        public WorkersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            UserManager = userManager;
        }

        // GET: Employee/Workers
        public async Task<IActionResult> Index()
        {
            var uNG_HELPDESKContext = _context.Workers.Include(w => w.Manager).ThenInclude(w => w.User).Include(w => w.User).Where(w=> w.UserId == UserManager.GetUserId(User));
            return View(await uNG_HELPDESKContext.ToListAsync());
        }

        // GET: Employee/Workers/Details/5
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

        // GET: Employee/Workers/Create
        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId");
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Employee/Workers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkerId,Score,NOfWorks,State,Flag,Queue,Id,ManagerId")] Workers workers)
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

        // GET: Employee/Workers/Edit/5
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

        // POST: Employee/Workers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

     [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkerId,Score,NOfWorks,State,Flag,Queue,Id,ManagerId")] Workers workers)
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
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", workers.UserId);
            return View(workers);
        }

        // GET: Employee/Workers/Delete/5
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

        // POST: Employee/Workers/Delete/5
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


        [HttpPost]
        public async Task<IActionResult> Upload_Config(IFormFile file, int? WorkerId, [Bind("WorkerId,Score,NOfWorks,State,Flag,Queue,UserId,ManagerId")] Workers workers)
        {
            string name = _context.Workers.Include(w => w.User).FirstOrDefault(w => w.WorkerId == WorkerId).User.Fullname;
            var w = _context.Workers.FirstOrDefault(w => w.WorkerId == WorkerId);
            string f = w.FileUrl;

            if (WorkerId == null)
            {
                return NotFound();
            }
            if (file == null || file.Length == 0)
                return Content("File is not selected");
            string type = Path.GetExtension(file.FileName);
            if ((type != ".docx") && (type != ".doc") && (type != ".pdf"))
                return Content("Notogri fayl turi tanlandi");

            try
            {
                workers = w;
                workers.FileUrl = type;
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

            string prevFile = name + f;
            if (System.IO.File.Exists(Path.Combine("wwwroot/files", prevFile)))
            {
                // If file found, delete it    
                System.IO.File.Delete(Path.Combine("wwwroot/files", prevFile));
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", name + type);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Download_Config(int WorkerId)
        {
            string file;
            string name = _context.Workers.Include(w => w.User).FirstOrDefault(w => w.WorkerId == WorkerId).User.Fullname;
            string type = _context.Workers.FirstOrDefault(w => w.WorkerId == WorkerId).FileUrl;
            file = name + type;
            if (file == null)
                return Content("There is no such file");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", file);
            var memory = new MemoryStream();
            try
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, GetContentType(path), Path.GetFileName(path));
            }
            catch
            {
                return Content("Error occured while downloading");
            }

        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain" },
                {".pdf", "application/pdf" },
                {".doc", "application/vnd.ms-word" },
                {".docx", "application/vnd.ms-word" }
            };
        }


    }
}
