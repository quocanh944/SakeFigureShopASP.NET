namespace SakeFigureShop.Domains
{
    public class Media
    {
        public long Id { get; set; }
        public MediaType MediaType { get; set; }
        public string Url { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
    }
    public enum MediaType
    {
        Video,
        Image
    }
}
