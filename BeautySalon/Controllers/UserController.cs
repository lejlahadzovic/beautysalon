using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace BeautySalon.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public UserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View("User");
        }

        [HttpPost]
        public ActionResult Register(UserVM newUser)
        {
            User user = new User();
            _dbContext.Add(user);

            user.FirstName = newUser.FirstName;
            user.LastName = newUser.LastName;
            user.Email = newUser.Email;
            user.PhoneNumber = newUser.PhoneNumber;
            user.Gender = newUser.Gender;
            user.BirthDate = newUser.BirthDate;
            user.PasswordSalt = GenerateSalt();
            user.PasswordHash = GenerateHash(user.PasswordSalt, newUser.Password);
            _dbContext.SaveChanges();
            return Ok();
        }

        public static string GenerateSalt()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            var byteArray = new byte[16];
            provider.GetBytes(byteArray);


            return Convert.ToBase64String(byteArray);
        }
        public static string GenerateHash(string salt, string password)
        {
            byte[] src = Convert.FromBase64String(salt);
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] dst = new byte[src.Length + bytes.Length];

            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }

    }
}
