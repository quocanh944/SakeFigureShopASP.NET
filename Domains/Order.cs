using System.ComponentModel.DataAnnotations;

namespace SakeFigureShop.Domains
{
    public enum StatusType
    {
        NotConfirmed,
        Confirmed,
        OnDelivery,
        Deliveried
    }
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Fullname{ get; set; }
        public string? Information { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public StatusType status { get; set; } = StatusType.NotConfirmed;

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
