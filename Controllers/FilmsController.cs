using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SakeFigureShop.Data;
using SakeFigureShop.Domains;

namespace SakeFigureShop.Controllers
{
    public class FilmsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static string Message = string.Empty;

        public FilmsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            ViewData["Message"] = null;
            if (!string.IsNullOrEmpty(Message))
            {
                ViewData["Message"] = Message;
                Message = string.Empty;
            }
            return _context.Films != null ?
                        View(await _context.Films.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Films'  is null.");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Film film)
        {
            if (ModelState.IsValid)
            {
                _context.Add(film);
                await _context.SaveChangesAsync();
                Message = "Thêm thành công anime: " + film.Name;
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Films == null)
            {
                return NotFound();
            }

            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name")] Film film)
        {
            if (id != film.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(film);
                    await _context.SaveChangesAsync();
                    Message = "Sửa thành công anime: " + film.Name;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.Id))
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
            return View(film);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Films == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Films == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Films'  is null.");
            }
            var film = await _context.Films.FindAsync(id);
            if (film != null)
            {
                _context.Films.Remove(film);
            }
            Message = "Đã xóa thành công anime: " + film?.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(long id)
        {
            return (_context.Films?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
