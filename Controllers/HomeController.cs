using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using SakeFigureShop.Data;
using SakeFigureShop.Models;
using SakeFigureShop.Models.Home;
using SQLitePCL;
using System.Diagnostics;
using System.Web;

namespace SakeFigureShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly int pageSize = 9;

        public HomeController(
            ApplicationDbContext applicationDbContext,
            ILogger<HomeController> logger
            )
        {
            _context = applicationDbContext;
            _logger = logger;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("Home/Film/{id}")]
        public IActionResult Film(
            long id,
            [FromQuery] List<long> brands,
            string search = "",
            int page = 1,
            string sort = "ASC",
            long minPrice = -1,
            long maxPrice = -1)
        {
            var vm = new ShopAllViewModel();
            vm.AllBrands = _context.Brands.Include(brand => brand.Products).ToList();
            vm.AllFilms = _context.Films.Include(film => film.Products).ToList();

            var filteredProds =
                sort == "ASC" ?
                _context.Products.Include(p => p.Brand).Include(p => p.Film)
                    .OrderBy(p => p.Price)
                    .Where(p => p.FilmId == id)
                    .Where(p => string.IsNullOrEmpty(search) ? true : p.Name.Contains(search))
                    .Where(p => brands.Count > 0 ? (p.BrandId != null ? brands.Contains((long)p.BrandId) : false) : true)
                    .Where(p => maxPrice != -1 ? p.Price <= maxPrice : true)
                    .Where(p => minPrice != -1 ? p.Price >= minPrice : true) :
                _context.Products.Include(p => p.Brand).Include(p => p.Film)
                    .OrderByDescending(p => p.Price)
                    .Where(p => p.FilmId == id)
                    .Where(p => string.IsNullOrEmpty(search) ? true : p.Name.Contains(search))
                    .Where(p => brands.Count > 0 ? (p.BrandId != null ? brands.Contains((long)p.BrandId) : false) : true)
                    .Where(p => maxPrice != -1 ? p.Price <= maxPrice : true)
                    .Where(p => minPrice != -1 ? p.Price >= minPrice : true);
            ViewData["Number of Pages"] = Math.Ceiling(filteredProds.Count() / (pageSize * 1.0));
            vm.AllProducts = filteredProds
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ViewData["Product Quantity"] = filteredProds.Count();
            
            ViewData["Min Price"] = minPrice;
            ViewData["Max Price"] = maxPrice;
            ViewData["Current Page"] = page;
            ViewData["Sort Type"] = sort;
            ViewData["Brands"] = brands;
            ViewData["Film Id"] = id;
            ViewData["Current Query"] = "sort=" + sort + "&minPrice=" + minPrice + "&maxPrice=" + maxPrice;
            foreach (var item in brands)
            {
                ViewData["Current Query"] += "&brands=" + item.ToString();
            }
            return View(vm);
        }

        [AllowAnonymous]
        public IActionResult ShopAll(
            [FromQuery] List<long> brands,
            int page = 1,
            string search = "",
            string sort = "ASC",
            long minPrice = -1,
            long maxPrice = -1)
        {                                 
            var vm = new ShopAllViewModel();
            vm.AllBrands = _context.Brands.Include(brand => brand.Products).ToList();
            vm.AllFilms = _context.Films.Include(film => film.Products).ToList();

            var filteredProds = 
                sort == "ASC" ? 
                _context.Products.Include(p => p.Brand).Include(p => p.Film)
                    .OrderBy(p => p.Price)
                    .Where(p => string.IsNullOrEmpty(search) ? true : p.Name.Contains(search))
                    .Where(p => brands.Count > 0 ? (p.BrandId != null ? brands.Contains((long) p.BrandId) : false) : true)
                    .Where(p => maxPrice != -1 ? p.Price <= maxPrice : true)
                    .Where(p => minPrice != -1 ? p.Price >= minPrice : true) :
                _context.Products.Include(p => p.Brand).Include(p => p.Film)
                    .OrderByDescending(p => p.Price)
                    .Where(p => string.IsNullOrEmpty(search) ? true : p.Name.Contains(search))
                    .Where(p => brands.Count > 0 ? (p.BrandId != null ? brands.Contains((long)p.BrandId) : false) : true)
                    .Where(p => maxPrice != -1 ? p.Price <= maxPrice : true)
                    .Where(p => minPrice != -1 ? p.Price >= minPrice : true);

            vm.AllProducts = filteredProds
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ViewData["Product Quantity"] = filteredProds.Count();
            
            ViewData["Number of Pages"] = Math.Ceiling(filteredProds.Count() / (pageSize * 1.0));
            ViewData["Min Price"] = minPrice;
            ViewData["Max Price"] = maxPrice;
            ViewData["Current Page"] = page;
            ViewData["Sort Type"] = sort;
            ViewData["Brands"] = brands;
            ViewData["Current Query"] = "sort=" + sort + "&minPrice=" + minPrice + "&maxPrice=" + maxPrice;
            foreach (var item in brands)
            {
                ViewData["Current Query"] += "&brands=" + item.ToString();
            }
            return View(vm);
        }

        [AllowAnonymous]
        [Route("Home/Detail/{id}")]
        public IActionResult Detail(long id)
        {
            var vm = new DetailViewModel();
            var product = _context.Products
                .Include(p => p.Medias)
                .Include(p => p.Brand)
                .Include(p => p.Film)
                .FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound("Không tìm thấy sản phẩm");
            }
            var relatedProduct = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Film)
                .Where(p => p.Id != id)
                .Where(p => p.BrandId == product.BrandId || p.FilmId == product.FilmId).ToList();
            vm.Product = product;
            vm.RelatedProducts = relatedProduct;
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}