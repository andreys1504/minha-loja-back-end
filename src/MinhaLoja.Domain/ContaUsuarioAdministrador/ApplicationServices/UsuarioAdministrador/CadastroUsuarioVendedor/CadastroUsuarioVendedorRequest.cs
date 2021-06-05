using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System;
using MensagensVendedor = MinhaLoja.Domain.MessagesDomain.ContaUsuarioAdministrador;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.CadastroUsuarioVendedor
{
    public class CadastroUsuarioVendedorRequest : RequestService, IRequest<IResponseService<string>>
    {
        public CadastroUsuarioVendedorRequest(
            string nome,
            string senha,
            string email,
            string cnpj)
        {
            Nome = nome.TrimString();
            Senha = senha.TrimString();
            Email = email.TrimString();
            Cnpj = cnpj.TrimString().GetNumbers();
        }

        public string Nome { get; private set; }
        public string Senha { get; private set; }
        public string Email { get; private set; }
        public string Cnpj { get; private set; }
        public override Guid IdUsuario { get; }

        public override bool Validate()
        {
            AddNotifications(new Contract<Notification>()
                .IsNotNullOrWhiteSpace(this.Nome, nameof(this.Nome), MensagensVendedor.Vendedor_Cadastro_NomeIsNotNullOrWhiteSpace)
                .IsGreaterOrEqualsThan(this.Nome, 3, nameof(this.Nome), MensagensVendedor.Vendedor_Cadastro_NomeIsGreaterOrEqualsThan)
                .IsLowerOrEqualsThan(this.Nome, 45, nameof(this.Nome), MensagensVendedor.Vendedor_Cadastro_NomeIsLowerOrEqualsThan)
            );

            AddNotifications(new Contract<Notification>()
                .IsNotNullOrWhiteSpace(this.Senha, nameof(this.Senha), MensagensVendedor.Vendedor_Cadastro_SenhaIsNotNullOrWhiteSpace)
                .IsGreaterOrEqualsThan(this.Senha, 8, nameof(this.Senha), MensagensVendedor.Vendedor_Cadastro_SenhaIsGreaterOrEqualsThan)
                .IsLowerOrEqualsThan(this.Senha, 20, nameof(this.Senha), MensagensVendedor.Vendedor_Cadastro_SenhaIsLowerOrEqualsThan)
            );

            AddNotifications(new Contract<Notification>()
                .IsNotNullOrWhiteSpace(this.Email, nameof(this.Email), MensagensVendedor.Vendedor_Cadastro_EmailIsNotNullOrWhiteSpace)
                .IsEmail(this.Email, nameof(this.Email), MensagensVendedor.Vendedor_Cadastro_EmailIsEmail)
                .IsLowerOrEqualsThan(this.Email, 45, nameof(this.Email), MensagensVendedor.Vendedor_Cadastro_EmailIsLowerOrEqualsThan)
            );

            AddNotifications(new Contract<Notification>()
                .IsNotNullOrWhiteSpace(this.Cnpj, nameof(this.Cnpj), MensagensVendedor.Vendedor_Cadastro_CnpjIsNotNullOrWhiteSpace)
                .IsCnpj(this.Cnpj, nameof(this.Cnpj), MensagensVendedor.Vendedor_Cadastro_CnpjIsCnpj)
            );

            return IsValid;
        }
    }
}
