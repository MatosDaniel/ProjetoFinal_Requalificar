using System.ComponentModel.DataAnnotations;

namespace ProjetoFinal.Models
{
    //Publication model that uses UserId instead of the user
    public class PostViewModel
    {
        public int IdPub { get; set; }

        [StringLength(140, ErrorMessage = "The maximum characters each Gluglu can have is 140")]
        public string Text { get; set; }
        public int UserId { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
