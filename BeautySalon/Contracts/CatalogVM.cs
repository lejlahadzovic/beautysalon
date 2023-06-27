using BeautySalon.Constants;
using BeautySalon.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeautySalon.Contracts
{
    public class CatalogVM
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = Messages.TITLE_ERROR_MESSAGE)]
        [StringLength(50)]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = Messages.DESCRIPTION_ERROR_MESSAGE)]
        public string Description { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = Messages.TYPE_ERROR_MESSAGE)]
        [StringLength(50)]
        public string Type { get; set; }
        public byte[]? Photo { get; set; }
        public string? ImageFileString { get; set; }
    }
}
