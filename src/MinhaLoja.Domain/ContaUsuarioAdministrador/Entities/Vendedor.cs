using MinhaLoja.Core.Domain.Entities.AggregateRootBase;
using MinhaLoja.Core.Settings;
using MinhaLoja.Domain.Catalogo.Entities;
using System;
using System.Collections.Generic;
using MensagensVendedor = MinhaLoja.Domain.MessagesDomain.ContaUsuarioAdministrador;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.Entities
{
    public class Vendedor : AggregateRoot
    {
        protected Vendedor() : base(null)
        {
        }

        public Vendedor(
            string email,
            string cnpj,
            int idUsuario) : base(null)
        {
            Email = email.TrimString();
            Cnpj = cnpj.TrimString().GetNumbers();
            CadastroAprovado = null;
            UsuarioId = idUsuario;
            GerarNovoCodigoValidacaoEmail();
        }

        public string Email { get; private set; }
        public string Cnpj { get; private set; }
        public bool? CadastroAprovado { get; private set; }
        public bool EmailValidado { get; private set; }
        public string CodigoValidacaoEmail { get; private set; }
        public DateTime? DataMaximaCodigoValidacaoEmail { get; private set; }
        public int UsuarioId { get; private set; }

        public UsuarioAdministrador Usuario { get; private set; }
        public ICollection<Produto> Produtos { get; private set; } = new List<Produto>();

        public string GerarNovoCodigoValidacaoEmail()
        {
            EmailValidado = false;
            CodigoValidacaoEmail = $"{Guid.NewGuid()}-{Guid.NewGuid()}-{Guid.NewGuid()}";
            DataMaximaCodigoValidacaoEmail = DateTime.Now.AddDays(5);

            return CodigoValidacaoEmail;
        }

        public void SetarEmailValido()
        {
            EmailValidado = true;
            CodigoValidacaoEmail = null;
            DataMaximaCodigoValidacaoEmail = null;
        }

        public void AprovarCadastro()
        {
            CadastroAprovado = true;
        }

        public void RejeitarCadastro()
        {
            CadastroAprovado = false;
        }

        public static string CorpoMensagemValidacaoEmail(
            GlobalSettings globalSettings,
            string codigoValidacaoEmail)
        {
            string href = $"{globalSettings.UrlApiAdminLoja}/{globalSettings.URLValidateEmailUserAdministrator}/{codigoValidacaoEmail}";
            return $@"
                <p>
                    <a href='{href}' target='_blank'>Validar E-mail</a>
                </p>
            ";
        }

        public static string CorpoMensagemAprovacaoCadastro()
        {
            return $@"
                <p>{MensagensVendedor.Vendedor_Aprovar_MensagemEmail01}</p>
                <br />
                <p>{MensagensVendedor.Vendedor_Aprovar_MensagemEmail02}</p>
            ";
        }

        public static string CorpoMensagemRejeicaoCadastro()
        {
            return $@"
                <p>{MensagensVendedor.Vendedor_Rejeitar_MensagemEmail01}</p>
                <br />
                <p>{MensagensVendedor.Vendedor_Rejeitar_MensagemEmail02}</p>
            ";
        }
    }
}
