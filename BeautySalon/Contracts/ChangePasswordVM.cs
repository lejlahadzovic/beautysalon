using BeautySalon.Constants;
using System.ComponentModel.DataAnnotations;

namespace BeautySalon.Contracts
{
    public class ChangePasswordVM
    {
        [Required]
        [RegularExpressionAttribute(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage = Messages.PASSWORD_ERROR_MESSAGE)]
        public string OldPassword { get; set; }

        [Required]
        [RegularExpressionAttribute(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage = Messages.PASSWORD_ERROR_MESSAGE)]
        public string NewPassword { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = Messages.CONFRIM_PASSWORD_ERROR_MESSAGE)]
        public string ConfirmPassword { get; set; }
    }
}
