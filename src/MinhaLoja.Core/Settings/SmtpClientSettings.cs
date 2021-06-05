namespace MinhaLoja.Core.Settings
{
    public class SmtpClientSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailSupport { get; set; }
        public string EmailNotifications { get; set; }
    }
}
