namespace MinhaLoja.Core.Settings
{
    public class IdentitySettings
    {
        public int ExpiresToken { get; set; }
        public bool IsHours { get; set; }
        public string JwksUri { get; set; }
    }
}
