using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalon.Contracts
{
    public class UserVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [RegularExpressionAttribute(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public byte[]? Photo { get; set; }

        [Required]
        [RegularExpressionAttribute(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage ="Password not strong enough")]
        public string Password { get; set; }

        [NotMapped]
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password Not Matched")]
        public string ConfirmPassword { get; set; }
    }
}
