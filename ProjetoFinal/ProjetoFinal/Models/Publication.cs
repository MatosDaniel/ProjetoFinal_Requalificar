namespace ProjetoFinal.Models
{
    //Creating the Publication class and its attributes and properties
    public class Publication
    {
        public int IdPub { get; set; }
        public string Text { get; set; }

        //Property that relates the User class to the Publication class
        public User User { get; set; }
        public DateTime Time { get; set; } = DateTime.Now; 
        public string Username { get; set; }
        public int Likes { get; set; }
    }
}
