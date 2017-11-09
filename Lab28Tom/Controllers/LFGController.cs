using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab28Tom.Models;
using Microsoft.AspNetCore.Authorization;

namespace Lab28Tom.Controllers
{
    //[Authorize(Policy = "Admin Only")]
    [Authorize(Policy = "Minimum Power")]
    public class LFGController : Controller
    {
        private readonly Lab28TomContext _context;

        public LFGController(Lab28TomContext context)
        {
            _context = context;
        }

        // GET: LFG
        public async Task<IActionResult> Index()
        {
            return View(await _context.LFG.ToListAsync());
        }

        // GET: LFG/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lFG = await _context.LFG
                .SingleOrDefaultAsync(m => m.ID == id);
            if (lFG == null)
            {
                return NotFound();
            }

            return View(lFG);
        }

        // GET: LFG/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        // POST: LFG/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Gamertag,DestinyClass,Request")] LFG lFG)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lFG);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lFG);
        }

        // GET: LFG/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lFG = await _context.LFG.SingleOrDefaultAsync(m => m.ID == id);
            if (lFG == null)
            {
                return NotFound();
            }
            return View(lFG);
        }

        // POST: LFG/Edit/5
        [Authorize(Policy = "Admin Only")]
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Gamertag,DestinyClass,Request")] LFG lFG)
        {
            if (id != lFG.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lFG);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LFGExists(lFG.ID))
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
            return View(lFG);
        }

        // GET: LFG/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lFG = await _context.LFG
                .SingleOrDefaultAsync(m => m.ID == id);
            if (lFG == null)
            {
                return NotFound();
            }

            return View(lFG);
        }

        // POST: LFG/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lFG = await _context.LFG.SingleOrDefaultAsync(m => m.ID == id);
            _context.LFG.Remove(lFG);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LFGExists(int id)
        {
            return _context.LFG.Any(e => e.ID == id);
        }
    }
}
