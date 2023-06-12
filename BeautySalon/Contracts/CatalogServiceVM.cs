using BeautySalon.Models;
using System.Diagnostics;

namespace BeautySalon.Contracts
{
    public class CatalogServiceVM
    {
        public string Title { get; set; }

        public int CatalogId { get; set; }

        public List<ServiceVM> Services { get; set; }
    }
}
