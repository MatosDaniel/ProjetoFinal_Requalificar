namespace ProjetoFinal.Models
{
    public class Publication
    {
        public int IdPub { get; set; }
        public string Text { get; set; }
        public string? Img { get; set; }
        public User User { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
        public int Likes { get; set; }
    }
}
