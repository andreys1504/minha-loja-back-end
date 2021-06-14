using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System;
using System.Collections.Generic;
using MensagensTipoProduto = MinhaLoja.Domain.MessagesDomain.Catalogo;

namespace MinhaLoja.Domain.Catalogo.ApplicationServices.TipoProduto.Cadastro
{
    public class CadastroTipoProdutoRequest : RequestAppService, IRequest<IResponseAppService<CadastroTipoProdutoDataResponse>>
    {
        public CadastroTipoProdutoRequest(
            string nomeTipoProduto,
            int? idTipoProdutoSuperior,
            IList<(string nome, string observacao)> caracteristicasTipoProduto,
            Guid idUsuario)
        {
            NomeTipoProduto = nomeTipoProduto.TrimString();
            IdTipoProdutoSuperior = idTipoProdutoSuperior;
            CaracteristicasTipoProduto = caracteristicasTipoProduto;
            IdUsuario = idUsuario;
        }

        public string NomeTipoProduto { get; private set; }
        public int? IdTipoProdutoSuperior { get; private set; }
        public IList<(string nome, string observacao)> CaracteristicasTipoProduto { get; private set; }
        public override Guid IdUsuario { get; }

        public override bool Validate()
        {
            AddNotifications(new Contract<Notification>()
                .IsNotNullOrWhiteSpace(this.NomeTipoProduto, nameof(this.NomeTipoProduto), MensagensTipoProduto.TipoProduto_Cadastro_NomeTipoProdutoIsNotNullOrWhiteSpace)
                .IsGreaterOrEqualsThan(this.NomeTipoProduto, 3, nameof(this.NomeTipoProduto), MensagensTipoProduto.TipoProduto_Cadastro_NomeTipoProdutoIsGreaterOrEqualsThan)
                .IsLowerOrEqualsThan(this.NomeTipoProduto, 65, nameof(this.NomeTipoProduto), MensagensTipoProduto.TipoProduto_Cadastro_NomeTipoProdutoIsLowerOrEqualsThan)
            );

            if (this.IdTipoProdutoSuperior.HasValue)
                AddNotifications(new Contract<Notification>()
                    .IsGreaterOrEqualsThan(this.IdTipoProdutoSuperior.Value, 1, nameof(this.IdTipoProdutoSuperior), MensagensTipoProduto.TipoProduto_Cadastro_IdTipoProdutoSuperiorIsGreaterOrEqualsThan)
                );

            AddNotifications(new Contract<Notification>()
                .IsNotNull(this.CaracteristicasTipoProduto, nameof(this.CaracteristicasTipoProduto), MensagensTipoProduto.TipoProduto_Cadastro_CaracteristicasTipoProdutoIsNotNull)
            );

            if (IsValid)
                AddNotifications(new Contract<Notification>()
                    .IsGreaterOrEqualsThan(this.CaracteristicasTipoProduto.Count, 1, nameof(this.CaracteristicasTipoProduto), MensagensTipoProduto.TipoProduto_Cadastro_CaracteristicasTipoProdutoCount)
                );

            return IsValid;
        }
    }
}
