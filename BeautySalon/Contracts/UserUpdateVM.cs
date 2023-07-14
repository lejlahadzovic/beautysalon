using BeautySalon.Constants;
using System.ComponentModel.DataAnnotations;

namespace BeautySalon.Contracts
{
    public class UserUpdateVM
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = Messages.EMAIL_INVALID_ERROR_MESSAGE)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^(\+\d{1, 2}\s?)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$", ErrorMessage = Messages.PHONE_INVALID_ERROR_MESSAGE)]
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public byte[]? Photo { get; set; }
    }
}
