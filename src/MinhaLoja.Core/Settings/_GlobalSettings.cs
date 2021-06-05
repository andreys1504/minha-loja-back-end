using System;

namespace MinhaLoja.Core.Settings
{
    public class GlobalSettings
    {
        public Environments Environment { get; set; } = Environments.Development;
        public string DatabaseConnectionString { get; set; }
        public string DatabaseSecondaryConnectionString { get; set; }
        public string DatabaseCacheConnectionString { get; set; }
        public string SecretToken { get; set; }
        public string CurrentApplication { get; set; }
        public string SecretApplications { get; set; }
        public bool TriggerEmails { get; set; }
        public string UrlApiAdminLoja { get; set; }
        public string URLValidateEmailUserAdministrator { get; set; }
        public bool PublishEventsInBus { get; set; }
        public bool SendLogErrorToStorage { get; set; }
        public StorageSettings Storage { get; set; }
        public SmtpClientSettings SmtpClient { get; set; }
        public IdentitySettings Identity { get; set; }
        public ServiceBusSettings ServiceBus { get; set; }

        public void SetEnvironment(string environmentName)
        {
            Environment = (Environments)Enum.Parse(typeof(Environments), environmentName);
        }
    }
}
