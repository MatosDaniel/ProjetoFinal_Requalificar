namespace ProjetoFinal.Models
{
    public class Publication //Criação da classe Publication e dos seus atributos e propriedades
    {
        public int IdPub { get; set; }
        public string Text { get; set; }
        public string? Img { get; set; }
        public virtual User User { get; set; }  //Propriedade que relaciona a classe User com a classe Publication
        public DateTime Time { get; set; } = DateTime.Now; 

        //public int Likes { get; set; }
    }
}
