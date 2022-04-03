using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProjetoFinal.Models
{
    public class User //Criação da classe User e dos seus atributos e propriedades
    {
        public int UserId { get; set; }

        // [Index(IsUnique = true)] //https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/data-annotations
                                 //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs0592?f1url=%3FappId%3Droslyn%26k%3Dk(CS0592)
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
