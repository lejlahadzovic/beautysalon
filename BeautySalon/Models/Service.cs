using System.ComponentModel.DataAnnotations.Schema;

namespace BeautySalon.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public int CatalogId { get; set; }
        public Catalog Catalog { get; set; }

    }
}
