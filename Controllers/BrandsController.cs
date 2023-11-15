using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SakeFigureShop.Data;
using SakeFigureShop.Domains;

namespace SakeFigureShop.Controllers
{
    public class BrandsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationHandler _authService;
        private readonly UserManager<User> _userManager;
        private static string Message = string.Empty;


        public BrandsController(
            ApplicationDbContext context,
            IAuthorizationHandler authService,
            UserManager<User> userManager)
        {
            _context = context;
            _authService = authService;
            _userManager = userManager;
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
            return _context.Brands != null ?
                        View(await _context.Brands.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Brands'  is null.");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brand);
                await _context.SaveChangesAsync();
                Message = "Thêm thành công hãng: " + brand.Name;
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name")] Brand brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                    Message = "Sửa thành công hãng: " + brand.Name;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.Id))
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
            return View(brand);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Brands == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Brands'  is null.");
            }
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
            }

            Message = "Xóa thành công hãng: " + brand?.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(long id)
        {
            return (_context.Brands?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
