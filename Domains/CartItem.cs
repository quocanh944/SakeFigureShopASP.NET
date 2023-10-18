using System.ComponentModel.DataAnnotations;

namespace SakeFigureShop.Domains
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int quantity { get; set; }
        public Product Product { get; set; }
        public long ProductId { get; set; }
        
        public User User { get; set; }
        public string UserId { get; set; }
    }
}
