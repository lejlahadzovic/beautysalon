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

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public byte[]? Photo { get; set; }

        [Required]
        [RegularExpressionAttribute(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage = Messages.PASSWORD_ERROR_MESSAGE)]
        public string Password { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = Messages.CONFRIM_PASSWORD_ERROR_MESSAGE)]
        public string ConfirmPassword { get; set; }
    }
}
