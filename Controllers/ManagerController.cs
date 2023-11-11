using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SakeFigureShop.Data;
using SakeFigureShop.Domains;
using SakeFigureShop.Models.Manager;

namespace SakeFigureShop.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ManagerController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var vm = new DashboardViewModel();

            var allOrders = _context.Orders.Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product);

            var totalOrders = allOrders.Count();
            var listUnconfirmOrders = allOrders.Where(o => o.status == StatusType.NotConfirmed).ToList();
            var totalUnconfirmOrders = listUnconfirmOrders.Count();
            var totalDeliveriedOrder = allOrders.Where(o => o.status == StatusType.Deliveried).ToList();
            var listConfirmedOrders = allOrders.Where(o => o.status != StatusType.NotConfirmed).ToList();

            double totalRevenue = 0;

            foreach (var item in totalDeliveriedOrder)
            {
                totalRevenue += item.OrderDetails.Sum(od => od.Quantity * od.Product.Price);
            }

            vm.TotalOrders = totalOrders;
            vm.TotalUnconfirmOrders = totalUnconfirmOrders;
            vm.TotalRevenue = totalRevenue;
            vm.ListUnconfirmOrders = listUnconfirmOrders;
            vm.ListConfirmedOrders = listConfirmedOrders;
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult OrderDetail(int id)
        {
            var o = _context.Orders.Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product).Where(o => o.Id == id).FirstOrDefault();
            return View(o);
        }
    }
}
