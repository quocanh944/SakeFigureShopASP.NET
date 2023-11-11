using System.ComponentModel.DataAnnotations;

namespace SakeFigureShop.Domains
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
