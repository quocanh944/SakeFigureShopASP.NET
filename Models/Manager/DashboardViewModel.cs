using SakeFigureShop.Domains;

namespace SakeFigureShop.Models.Manager
{
    public class DashboardViewModel
    {
        public double TotalRevenue;
        public long TotalOrders;
        public long TotalUser;
        public long TotalUnconfirmOrders;
        public long TotalProducts;
        public long TotalAnimes;
        public long TotalBrands;

        public IEnumerable<Order> ListUnconfirmOrders { get; set; } = new List<Order>();
        public IEnumerable<Order> ListConfirmedOrders { get; set; } = new List<Order>();
    }
}
