using MinhaLoja.Core.Domain.Entities.AggregateRootBase;
using MinhaLoja.Core.Settings;
using System.Security.Cryptography;
using System.Text;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.Entities
{
    public class UsuarioAdministrador : AggregateRoot
    {
        protected UsuarioAdministrador() : base(null)
        {
        }

        public UsuarioAdministrador(
            string nome,
            string usernameEmail,
            string senha,
            GlobalSettings globalSettings) : base(null)
        {
            Nome = nome.TrimString();
            Username = usernameEmail.TrimString();
            if (globalSettings != null)
                Senha = CriptografarSenha(globalSettings, senha);
            UsuarioMaster = true;
        }

        public string Nome { get; private set; }
        public string Username { get; private set; }
        public string Senha { get; private set; }
        public bool UsuarioMaster { get; private set; }

        public Vendedor Vendedor { get; private set; }

        public static string CriptografarSenha(
            GlobalSettings globalSettings,
            string senha)
        {
            senha = globalSettings.SecretApplications.TrimString() + senha.TrimString();

            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(senha));

            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                builder.Append(bytes[i].ToString("x2"));

            return builder.ToString();
        }

        public void VincularVendedor(Vendedor vendedor)
        {
            Vendedor = vendedor;
            UsuarioMaster = false;

            AddNotifications(vendedor.Notifications);
        }
    }
}
