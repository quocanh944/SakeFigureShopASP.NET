using SakeFigureShop.Domains;

namespace SakeFigureShop.Models.Home
{
    public class ShopAllViewModel
    {
        public IEnumerable<Brand> AllBrands { get; set; } = new List<Brand>();
        public IEnumerable<Film> AllFilms { get; set; } = new List<Film>(); 
        public IEnumerable<Product> AllProducts { get; set; } = new List<Product>();
    }

    public class DetailViewModel
    {
        public IEnumerable<Product> RelatedProducts { get; set; } = new List<Product>();
        public Product Product { get; set; }
    }
}
