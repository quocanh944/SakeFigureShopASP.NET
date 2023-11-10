using SakeFigureShop.Domains;

namespace SakeFigureShop.Models.Cart
{
    public class CartViewModel
    {
        public IEnumerable<CartItem> cartItems { get; set; } = new List<CartItem>();
    }
}
