using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolGradeWebApp.Data;

namespace SchoolGradeWebApp.Controllers
{
    public class GradeDetailsController : Controller
    {
        private readonly SchoolDataBase _context;

        public GradeDetailsController(SchoolDataBase context)
        {
            _context = context;
        }

        // GET: GradeDetails
        public async Task<IActionResult> Index()
        {
            var schoolDataBase = _context.GradeDetails.Include(g => g.Grade);
            return View(await schoolDataBase.ToListAsync());
        }

        // GET: GradeDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradeDetail = await _context.GradeDetails
                .Include(g => g.Grade)
                .FirstOrDefaultAsync(m => m.GradeDetailID == id);
            if (gradeDetail == null)
            {
                return NotFound();
            }

            return View(gradeDetail);
        }

        // GET: GradeDetails/Create
        public IActionResult Create()
        {
            ViewData["GradeID"] = new SelectList(_context.Grades, "GradeID", "GradeID");
            return View();
        }

        // POST: GradeDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GradeDetailID,GradeID,Mark,Date,Description")] GradeDetail gradeDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gradeDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GradeID"] = new SelectList(_context.Grades, "GradeID", "GradeID", gradeDetail.GradeID);
            return View(gradeDetail);
        }

        // GET: GradeDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradeDetail = await _context.GradeDetails.FindAsync(id);
            if (gradeDetail == null)
            {
                return NotFound();
            }
            ViewData["GradeID"] = new SelectList(_context.Grades, "GradeID", "GradeID", gradeDetail.GradeID);
            return View(gradeDetail);
        }

        // POST: GradeDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GradeDetailID,GradeID,Mark,Date,Description")] GradeDetail gradeDetail)
        {
            if (id != gradeDetail.GradeDetailID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gradeDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradeDetailExists(gradeDetail.GradeDetailID))
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
            ViewData["GradeID"] = new SelectList(_context.Grades, "GradeID", "GradeID", gradeDetail.GradeID);
            return View(gradeDetail);
        }

        // GET: GradeDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradeDetail = await _context.GradeDetails
                .Include(g => g.Grade)
                .FirstOrDefaultAsync(m => m.GradeDetailID == id);
            if (gradeDetail == null)
            {
                return NotFound();
            }

            return View(gradeDetail);
        }

        // POST: GradeDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gradeDetail = await _context.GradeDetails.FindAsync(id);
            _context.GradeDetails.Remove(gradeDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GradeDetailExists(int id)
        {
            return _context.GradeDetails.Any(e => e.GradeDetailID == id);
        }
    }
}
