using System.ComponentModel.DataAnnotations;
using System.Drawing;

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

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Product p = (Product) obj;
                return (Name == p.Name) 
                    && (Description == p.Description) 
                    && (Price == p.Price) 
                    && (BrandId == p.BrandId) 
                    && (FilmId == p.FilmId);
            }
        }
    }
}
