using MinhaLoja.Core.Settings;

namespace MinhaLoja.Tests.Fakes.Settings
{
    public class GlobalSettingsFake : GlobalSettings
    {
        public GlobalSettingsFake()
        {
            SecretApplications = "a1a0bf9d-0036-4fad-b8d1-0315edc383ad-MinhaLoja";
        }
    }
}
