using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProjetoFinal.Models
{
    public class User //Criação da classe User e dos seus atributos e propriedades
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(25, ErrorMessage ="Password must have a minimum of 6 characters and a maximum of 25", MinimumLength = 6)]
        public string Password { get; set; }

        public string Gender { get; set; }

        [RegularExpression(@"^([0-9]{9})$", ErrorMessage = "Invalid Phone Number.")]
        public int Mobile { get; set; }
    }
}
