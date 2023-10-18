using System.ComponentModel.DataAnnotations;

namespace SakeFigureShop.Domains
{
    public class Product
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int Quantity { get; set; }
        public string ThumbnailImageUrl { get; set; }
        public long? BrandId { get; set; }
        public Brand? Brand { get; set; }
        public long? FilmId { get; set; }
        public Film? Film { get; set; }
        public ICollection<Media> Medias { get; set; }
    }
}
