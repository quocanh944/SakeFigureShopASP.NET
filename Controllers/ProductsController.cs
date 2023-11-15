using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SakeFigureShop.Data;
using SakeFigureShop.Domains;
using SakeFigureShop.Services.Interfaces;

namespace SakeFigureShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStorageService _firebaseStorageService;
        private static string Message = string.Empty;

        public ProductsController(ApplicationDbContext context, IStorageService firebaseStorageService)
        {
            _context = context;
            _firebaseStorageService = firebaseStorageService;
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
            var applicationDbContext = _context.Products.Include(p => p.Brand).Include(p => p.Film);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Medias)
                .Include(p => p.Brand)
                .Include(p => p.Film)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,Description,Price,Quantity,BrandId,FilmId")] Product product,
            [Required(ErrorMessage = "Vui lòng chọn ảnh thumbnail")] IFormFile ThumbnailImageUrl,
            [AllowNull] List<IFormFile> Medias)
        {
            if (ModelState.IsValid)
            { 
                var urlThumbnail = await _firebaseStorageService.UploadFileToStorage(ThumbnailImageUrl.OpenReadStream(), ThumbnailImageUrl.FileName);
                product.ThumbnailImageUrl = urlThumbnail;
                if (Medias != null && Medias.Count > 0)
                {
                    List<Media> mediasProduct = new();
                    foreach(var media in Medias)
                    {
                        var urlMedia = await _firebaseStorageService.UploadFileToStorage(media.OpenReadStream(), media.FileName);
                        mediasProduct.Add(new Media() { MediaType = MediaType.Image, Product = product, Url = urlMedia });
                    }
                    product.Medias = mediasProduct;
                }
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = product.Id });
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Name", product.FilmId);
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Name", product.FilmId);
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id,
            [Bind("Id,Name,Description,Price,Quantity,BrandId,FilmId")] Product product,
            [AllowNull] IFormFile ThumbnailImageUrl,
            [AllowNull] List<IFormFile> Medias)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ThumbnailImageUrl != null && ThumbnailImageUrl.Length > 0) { 
                        var urlThumbnail = await _firebaseStorageService.UploadFileToStorage(ThumbnailImageUrl.OpenReadStream(), ThumbnailImageUrl.FileName);
                        product.ThumbnailImageUrl = urlThumbnail;
                    }
                    if (Medias != null && Medias.Count > 0)
                    {
                        List<Media> mediasProduct = new();
                        foreach (var media in Medias)
                        {
                            var urlMedia = await _firebaseStorageService.UploadFileToStorage(media.OpenReadStream(), media.FileName);
                            mediasProduct.Add(new Media() { MediaType = MediaType.Image, Product = product, Url = urlMedia });
                        }
                        product.Medias = mediasProduct;
                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new {id = product.Id});
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Name", product.FilmId);
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Medias)
                .Include(p => p.Brand)
                .Include(p => p.Film)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            Message = "Đã xóa thành công sản phẩm: " + product?.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(long id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
