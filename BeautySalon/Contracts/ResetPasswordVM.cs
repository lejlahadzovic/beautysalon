using BeautySalon.Constants;
using System.ComponentModel.DataAnnotations;

namespace BeautySalon.Contracts
{
    public class ResetPasswordVM
    {
        [Required]
        [RegularExpressionAttribute(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage = Messages.PASSWORD_ERROR_MESSAGE)]
        public string NewPassword { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = Messages.CONFRIM_PASSWORD_ERROR_MESSAGE)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ResetCode { get; set; }
    }
}
