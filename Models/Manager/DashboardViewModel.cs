﻿using SakeFigureShop.Domains;

namespace SakeFigureShop.Models.Manager
{
    public class DashboardViewModel
    {
        public double TotalRevenue;
        public long TotalOrders;
        public long TotalUser;
        public long TotalUnconfirmOrders;

        public IEnumerable<Order> ListUnconfirmOrders { get; set; } = new List<Order>();
        public IEnumerable<Order> ListConfirmedOrders { get; set; } = new List<Order>();
    }
}
