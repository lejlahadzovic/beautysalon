using System.ComponentModel.DataAnnotations;
namespace BeautySalon.Contracts
{
    public class ForgotPasswordVM
    {
        [Required]
        public string Email { get; set; }
    }
}
