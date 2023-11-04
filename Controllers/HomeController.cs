using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SakeFigureShop.Data;
using SakeFigureShop.Models;
using SakeFigureShop.Models.Home;
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
        public IActionResult ShopAll(int page = 1, string sort = "ASC", long minPrice = -1, long maxPrice = -1)
        {                                 
            var vm = new ShopAllViewModel();
            vm.AllBrands = _context.Brands.Include(brand => brand.Products).ToList();
            vm.AllFilms = _context.Films.Include(film => film.Products).ToList();

            var filteredProds = 
                sort == "ASC" ? 
                _context.Products
                    .OrderBy(p => p.Price)
                    .Where(p => maxPrice != -1 ? p.Price <= maxPrice : true)
                    .Where(p => minPrice != -1 ? p.Price >= minPrice : true) :
                _context.Products
                    .OrderByDescending(p => p.Price)
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
            ViewData["Current Query"] = "sort=" + sort + "&minPrice=" + minPrice + "&maxPrice=" + maxPrice;
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}