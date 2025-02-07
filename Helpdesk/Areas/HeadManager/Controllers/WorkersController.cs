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
using Microsoft.AspNetCore.Hosting;
using ITHelpDesk.DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.IO;

namespace ITHelpDesk.Areas.HeadManager.Controllers
{
    [Area("HeadManager")]
    [Authorize(Roles = SD.Role_HeadManager)]
    public class WorkersController : Controller
    {
        private readonly IWebHostEnvironment _iweb;
        private readonly ApplicationDbContext _context;
        private readonly IRequest _notiService;
        List<Request> events = new List<Request>();
        public WorkersController(IRequest notiService, ApplicationDbContext context, IWebHostEnvironment iweb)
        {
            _iweb = iweb;
            _notiService = notiService;
            _context = context;
        }

        // GET: HeadManager/Workers
        public async Task<IActionResult> Index()
        {
            var uNG_HELPDESKContext = _context.Workers.Include(w => w.User).Include(w => w.Manager).ThenInclude(w => w.User).Include(w => w.Request).OrderByDescending(w => w.Score);
            return View(await uNG_HELPDESKContext.ToListAsync());
        }

        public IActionResult Reports()
        {
            return View();
        }

        // GET: HeadManager/Workers/Details/5
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

        //Employee Contacts Action
        public IActionResult Contacts()
        {
            dynamic model = new ExpandoObject();
            model.Workers = _context.Workers.Include(r => r.Manager).Include(r => r.Request).Include(r => r.User).ThenInclude(u => u.Address).ToList().OrderByDescending(u => u.Score);
            model.Requests = _context.Request.ToList();
            return View(model);
        }

        // GET: HeadManager/Workers/Create
        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: HeadManager/Workers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkerId,Score,NOfWorks,State,Flag,Queue,UserId,ManagerId")] Workers workers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_context.Managers, "ManagerId", "ManagerId", workers.ManagerId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", workers.UserId);
            return View(workers);
        }

        // GET: HeadManager/Workers/Edit/5
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", workers.UserId);
            return View(workers);
        }

        // POST: HeadManager/Workers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkerId,Score,NOfWorks,State,Flag,Queue,UserId,ManagerId")] Workers workers)
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", workers.UserId);
            return View(workers);
        }

        // GET: HeadManager/Workers/Delete/5
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

        // POST: HeadManager/Workers/Delete/5
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

        [HttpGet]
        public IActionResult GetEvents()
        {
            var events = _context.Request.Include(w => w.Worker).ThenInclude(u => u.User).ToList();
            return Json(events);

        }

        [HttpPost]
        public void WorkersScoreParameters(DateTime start, DateTime end)
        {
            Dictionary<int, Workers> worker = new Dictionary<int, Workers>();
            var Id = _context.Workers.Include(u => u.User).ToList();
            DateTime st = Convert.ToDateTime("01.09.2020 0:00:00");
            if (end.ToString() == "01.01.0001 0:00:00")
            {
                end = DateTime.Now;
            }
            if(start.ToString() == "01.01.0001 0:00:00")
            {
                start =st;
            }
            foreach (Workers w in Id)
            {
                int? score = _context.Request.Include(w => w.Worker).Where(r => DateTime.Compare(r.RDateTime.Date, start.Date) >= 0 && DateTime.Compare(r.RDateTime.Date, end.Date) <= 0 && r.RStatus == "green" && r.WorkerId == w.WorkerId).Sum(i => i.RWeight);
                int green = _context.Request.Include(w => w.Worker).Where(r => DateTime.Compare(r.RDateTime.Date, start.Date) >= 0 && DateTime.Compare(r.RDateTime.Date, end.Date) <= 0 && r.RStatus == "green" && r.WorkerId == w.WorkerId).Count();
                //int yellow = _context.Request.Include(w => w.Worker).Where(r => DateTime.Compare(r.RDateTime.Date, start.Date) >= 0 && DateTime.Compare(r.RDateTime.Date, end.Date) <= 0 && r.RStatus == "yellow" && r.WorkerId == w.WorkerId).Count();
                int red = _context.Request.Include(w => w.Worker).Where(r => DateTime.Compare(r.RDateTime.Date, start.Date) >= 0 && DateTime.Compare(r.RDateTime.Date, end.Date) <= 0 && r.RStatus == "red" && r.WorkerId == w.WorkerId).Count();
                int rejecteds = _context.Rejecteds.Where(wr => wr.Worker.WorkerId == w.WorkerId).Count();
                worker.Add(w.WorkerId, new Workers() { WorkerId = w.WorkerId, Flag = w.User.Fullname, Queue = rejecteds, NOfWorks = green, Score = score, });
                Console.WriteLine(worker);


            }

            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            //Sheet.Cells["A1"].Value = "Period";
            Sheet.Cells["A2"].Value = "Bajaruvchi";
            Sheet.Row(2).Style.Font.Bold = true;
            Sheet.Row(1).Style.Font.Size = 12;
            Sheet.Row(1).Style.Font.Italic = true;
            Sheet.Cells["A1"].Value = start.Date.ToString("dd-MM-yyyy") + " dan " + end.Date.ToString("dd-MM-yyyy") + " gacha";
            Sheet.Cells["B2"].Value = "Bajarilgan ishlar";
            Sheet.Cells["C2"].Value = "Rad etilganlar";
            Sheet.Cells["D2"].Value = "Umumiy ball";
            // Sheet.Cells["D1"].Value = "Period";
            int row = 3;
            foreach (var item in worker)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.Value.Flag;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.Value.NOfWorks;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.Value.Queue;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.Value.Score;
                row++;
            }
            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment: filename=" + "Report.xlsx");
            Response.Body.WriteAsync(Ep.GetAsByteArray());
            Response.Body.DisposeAsync();

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
                return Content("Файл танланмади");
            string type = Path.GetExtension(file.FileName);
            if ((type != ".docx") && (type != ".doc") && (type != ".pdf"))
                return View("~/Views/Shared/_UnsupportedMediatype.cshtml");

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
            return RedirectToAction("Contacts");
        }

        [HttpGet]
        public async Task<IActionResult> Download_Config(int WorkerId)
        {
            string file;
            string name = _context.Workers.Include(w => w.User).FirstOrDefault(w => w.WorkerId == WorkerId).User.Fullname;
            string type = _context.Workers.FirstOrDefault(w => w.WorkerId == WorkerId).FileUrl;
            file = name + type;
            if (file == null)
                return Content("Файл мавжуд эмас");
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
                return View("~/Views/Shared/_NotFound.cshtml", WorkerId);
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
