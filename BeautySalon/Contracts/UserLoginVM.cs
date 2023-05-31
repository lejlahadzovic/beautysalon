using BeautySalon.Constants;
using System.ComponentModel.DataAnnotations;

namespace BeautySalon.Contracts
{
    public class UserLoginVM
    {
        [Required]
        [RegularExpressionAttribute(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = Messages.EMAIL_INVALID_ERROR_MESSAGE)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }
    }
}
