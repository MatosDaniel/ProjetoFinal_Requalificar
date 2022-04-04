using System.ComponentModel.DataAnnotations;

namespace ProjetoFinal.Models
{
    //Creating the User class and its attributes and properties
    public class User 
    {
        public int UserId { get; set; }
        public string Username { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(25, ErrorMessage ="Password must have a minimum of 6 characters and a maximum of 25", MinimumLength = 6)]
        public string Password { get; set; }
        public string Gender { get; set; }

        [RegularExpression(@"^([0-9]{9})$", ErrorMessage = "Invalid Phone Number.")]
        public int Mobile { get; set; }
        public string ProfileImage { get; set; } = Path.GetFileName("/images/default.jpg");
    }
}
