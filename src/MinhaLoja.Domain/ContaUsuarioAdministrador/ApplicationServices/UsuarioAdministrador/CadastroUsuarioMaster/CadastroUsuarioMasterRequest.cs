using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System;
using MensagensUsuario = MinhaLoja.Domain.MessagesDomain.ContaUsuarioAdministrador;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.CadastroUsuarioMaster
{
    public class CadastroUsuarioMasterRequest : RequestAppService, IRequest<IResponseAppService<bool>>
    {
        public CadastroUsuarioMasterRequest(
            string nome,
            string username,
            string senha,
            Guid idUsuario)
        {
            Nome = nome.TrimString();
            Username = username.TrimString();
            Senha = senha.TrimString();
            IdUsuario = idUsuario;
        }

        public string Nome { get; private set; }
        public string Username { get; private set; }
        public string Senha { get; private set; }
        public override Guid IdUsuario { get; }

        public override bool Validate()
        {
            AddNotifications(new Contract<Notification>()
                .IsNotNullOrWhiteSpace(this.Nome, nameof(this.Nome), MensagensUsuario.UsuarioMaster_Cadastro_NomeIsNotNullOrWhiteSpace)
                .IsGreaterOrEqualsThan(this.Nome, 3, nameof(this.Nome), MensagensUsuario.UsuarioMaster_Cadastro_NomeIsGreaterOrEqualsThan)
                .IsLowerOrEqualsThan(this.Nome, 45, nameof(this.Nome), MensagensUsuario.UsuarioMaster_Cadastro_NomeIsLowerOrEqualsThan)
            );

            AddNotifications(new Contract<Notification>()
                .IsNotNullOrWhiteSpace(this.Username, nameof(this.Username), MensagensUsuario.UsuarioMaster_Cadastro_UsernameIsNotNullOrWhiteSpace)
                .IsGreaterOrEqualsThan(this.Username, 6, nameof(this.Username), MensagensUsuario.UsuarioMaster_Cadastro_UsernameIsGreaterOrEqualsThan)
                .IsLowerOrEqualsThan(this.Username, 45, nameof(this.Username), MensagensUsuario.UsuarioMaster_Cadastro_UsernameIsLowerOrEqualsThan)
            );

            AddNotifications(new Contract<Notification>()
                .IsNotNullOrWhiteSpace(this.Senha, nameof(this.Senha), MensagensUsuario.UsuarioMaster_Cadastro_SenhaIsNotNullOrWhiteSpace)
                .IsGreaterOrEqualsThan(this.Senha, 8, nameof(this.Senha), MensagensUsuario.UsuarioMaster_Cadastro_SenhaIsGreaterOrEqualsThan)
                .IsLowerOrEqualsThan(this.Senha, 20, nameof(this.Senha), MensagensUsuario.UsuarioMaster_Cadastro_SenhaIsLowerOrEqualsThan)
            );

            return IsValid;
        }
    }
}
