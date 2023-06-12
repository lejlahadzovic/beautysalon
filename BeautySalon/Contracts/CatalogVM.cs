using BeautySalon.Models;

namespace BeautySalon.Contracts
{
    public class CatalogVM
    {
        public int Id { get; set; }
        public string Title { get; set; } 
        public string Description { get; set; }
        public string Type { get; set; }
        public byte[]? Photo { get; set; }
    }
}
