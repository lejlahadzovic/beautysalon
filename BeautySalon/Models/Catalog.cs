using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautySalon.Models
{
    public class Catalog
    {
        public int Id { get; set; } 
        public string Title { get; set; }  
        public string Description { get; set; }  
        public string Type { get; set; }  
        public string? ImageFileString { get; set; }
    }
}
