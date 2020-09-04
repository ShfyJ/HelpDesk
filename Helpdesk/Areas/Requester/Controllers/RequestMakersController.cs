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

namespace ITHelpDesk.Areas.Requester.Controllers
{
    [Area("Requester")]
    [Authorize(Roles = SD.Role_Requester)]
    public class RequestMakersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestMakersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Requester/RequestMakers
        public async Task<IActionResult> Index()
        {
            var uNG_HELPDESKContext = _context.RequestMakers.Include(r => r.User);
            return View(await uNG_HELPDESKContext.ToListAsync());
        }

        // GET: Requester/RequestMakers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestMakers = await _context.RequestMakers
                .Include(r => r.User).ThenInclude(u=>u.FName)
                .FirstOrDefaultAsync(m => m.RequestmakerId == id);
            if (requestMakers == null)
            {
                return NotFound();
            }

            return View(requestMakers);

        }

        // GET: Requester/RequestMakers/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Requester/RequestMakers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestmakerId,UserId")] RequestMakers requestMakers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requestMakers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", requestMakers.UserId);
            return View(requestMakers);
        }

        // GET: Requester/RequestMakers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestMakers = await _context.RequestMakers.FindAsync(id);
            if (requestMakers == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", requestMakers.UserId);
            return View(requestMakers);
        }

        // POST: Requester/RequestMakers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestmakerId,Id")] RequestMakers requestMakers)
        {
            if (id != requestMakers.RequestmakerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requestMakers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestMakersExists(requestMakers.RequestmakerId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", requestMakers.UserId);
            return View(requestMakers);
        }

        // GET: Requester/RequestMakers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestMakers = await _context.RequestMakers
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RequestmakerId == id);
            if (requestMakers == null)
            {
                return NotFound();
            }

            return View(requestMakers);
        }

        // POST: Requester/RequestMakers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requestMakers = await _context.RequestMakers.FindAsync(id);
            _context.RequestMakers.Remove(requestMakers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestMakersExists(int id)
        {
            return _context.RequestMakers.Any(e => e.RequestmakerId == id);
        }
    }
}
