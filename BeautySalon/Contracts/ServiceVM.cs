using BeautySalon.Models;

namespace BeautySalon.Contracts
{
    public class ServiceVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int CatalogId { get; set; }
        public Catalog Catalog { get; set; }
    }
}
