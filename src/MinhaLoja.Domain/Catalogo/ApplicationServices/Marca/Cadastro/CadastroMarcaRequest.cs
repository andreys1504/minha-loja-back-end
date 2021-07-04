using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System;
using MarcaMensagens = MinhaLoja.Domain.MessagesDomain.Catalogo;

namespace MinhaLoja.Domain.Catalogo.ApplicationServices.Marca.Cadastro
{
    public class CadastroMarcaRequest : RequestAppService, IRequest<IResponseAppService<CadastroMarcaDataResponse>>
    {
        public CadastroMarcaRequest(
            string nomeMarca,
            Guid idUsuario)
        {
            NomeMarca = nomeMarca.TrimString();
            IdUsuarioEnvioRequest = idUsuario;
        }

        public string NomeMarca { get; private set; }
        public override Guid IdUsuarioEnvioRequest { get; }

        public override bool Validate()
        {
            AddNotifications(new Contract<Notification>()
                .IsNotNullOrWhiteSpace(this.NomeMarca, nameof(this.NomeMarca), MarcaMensagens.Marca_Cadastro_NomeMarcaIsNotNullOrWhiteSpace)
                .IsGreaterOrEqualsThan(this.NomeMarca, 2, nameof(this.NomeMarca), MarcaMensagens.Marca_Cadastro_NomeMarcaIsGreaterOrEqualsThan)
                .IsLowerOrEqualsThan(this.NomeMarca, 40, nameof(this.NomeMarca), MarcaMensagens.Marca_Cadastro_NomeMarcaIsLowerOrEqualsThan)
            );

            return IsValid;
        }
    }
}
