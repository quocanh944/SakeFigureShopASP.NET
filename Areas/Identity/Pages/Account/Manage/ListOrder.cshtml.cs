using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SakeFigureShop.Data;
using SakeFigureShop.Domains;
using System.ComponentModel.DataAnnotations;

namespace SakeFigureShop.Areas.Identity.Pages.Account.Manage
{
    public class ListOrderModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly ApplicationDbContext _context;

        public ListOrderModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<ChangePasswordModel> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        public IEnumerable<Order> orders;

        public async Task<IActionResult> OnGetAsync()
        {
            var user = _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            orders = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.UserId == user.Id)
                .ToList();
            
            return Page();
        }
    }
}
