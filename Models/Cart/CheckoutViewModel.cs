using SakeFigureShop.Domains;

namespace SakeFigureShop.Models.Cart
{
    public class CheckoutViewModel
    {
        public Order Order { get; set; }
        public IEnumerable<CartItem> cartItems { get; set; } = new List<CartItem>();
        public string Email { get; set; }
    }
}
