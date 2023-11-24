using System.ComponentModel.DataAnnotations;

namespace CeilingCalc.Autorization
{
    public class LoginModel
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }

    }
}