namespace ProjetoFinal.Models
{
    public class PostViewModel
    {
        public int IdPub { get; set; }
        public string Text { get; set; }
        public string? Img { get; set; }
        public int UserId { get; set; }  //Propriedade que relaciona a classe User com a classe Publication
        public DateTime Time { get; set; } = DateTime.Now;

        //public int Likes { get; set; }
    }
}
