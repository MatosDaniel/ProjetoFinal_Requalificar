namespace ProjetoFinal.Models
{
    public class User //Criação da classe User e dos seus atributos e propriedades
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public int Mobile { get; set; }
    }
}
