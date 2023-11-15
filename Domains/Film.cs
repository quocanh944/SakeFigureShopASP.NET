using System.ComponentModel.DataAnnotations;

namespace SakeFigureShop.Domains
{
    public class Film
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên anime.")]
        public string Name { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
