using System.ComponentModel.DataAnnotations;

namespace BeautySalon.Models
{
    public class User
    {
       public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public byte[]? Photo { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordHash { get; set; }


    }
}
