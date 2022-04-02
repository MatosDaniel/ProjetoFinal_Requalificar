using System.ComponentModel.DataAnnotations;

namespace ProjetoFinal.Models
{
    public class PostViewModel
    {
        public int IdPub { get; set; }

        [StringLength(140, ErrorMessage = "The maximum characters each Gluglu can have is 140")]
        public string Text { get; set; }
        public int UserId { get; set; }  //Propriedade que relaciona a classe User com a classe Publication
        public DateTime Time { get; set; } = DateTime.Now;

        //public int Likes { get; set; }
    }
}
