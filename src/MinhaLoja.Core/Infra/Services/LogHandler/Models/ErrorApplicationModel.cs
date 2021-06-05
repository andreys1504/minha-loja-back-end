namespace MinhaLoja.Core.Infra.Services.LogHandler.Models
{
    public class ErrorApplicationModel
    {
        public string Application { get; set; }
        public string ExceptionTitle { get; set; }
        public string Error { get; set; }
        public string Path { get; set; }
        public string UserId { get; set; }
    }
}
