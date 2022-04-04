namespace ProjetoFinal.Models
{
    //Model that combines the User and Publication Models
    public class ProfileViewModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public string Gender { get; set; }
        public int Mobile { get; set; }
        public string ProfileImage { get; set; } = Path.GetFileName("/images/default.jpg");
        public IEnumerable<Publication> Publications { get; set; }
        public int TotalPostByUser { get; set; }
    }
}
