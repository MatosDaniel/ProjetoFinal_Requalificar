namespace ProjetoFinal.Models
{
    //Model for the error page
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string? MessageError { get; set; }
    }
}