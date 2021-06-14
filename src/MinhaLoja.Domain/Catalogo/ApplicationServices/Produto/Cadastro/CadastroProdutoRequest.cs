using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System;
using System.Collections.Generic;
using MensagensProduto = MinhaLoja.Domain.MessagesDomain.Catalogo;

namespace MinhaLoja.Domain.Catalogo.ApplicationServices.Produto.Cadastro
{
    public class CadastroProdutoRequest : RequestAppService, IRequest<IResponseAppService<CadastroProdutoDataResponse>>
    {
        public CadastroProdutoRequest(
            string nomeProduto,
            decimal valor,
            int idMarca,
            string descricaoProduto,
            string idExterno,
            int idTipoProduto,
            IList<(int idCaracteristicaProduto, string descricao)> caracteristicaProduto,
            int? idVendedor,
            Guid idUsuario)
        {
            NomeProduto = nomeProduto.TrimString();
            Valor = valor;
            IdMarca = idMarca;
            DescricaoProduto = descricaoProduto.TrimString();
            IdExterno = idExterno.TrimString();
            IdTipoProduto = idTipoProduto;
            CaracteristicaProduto = caracteristicaProduto;
            IdVendedor = idVendedor;
            IdUsuario = idUsuario;
        }

        public string NomeProduto { get; private set; }
        public decimal Valor { get; private set; }
        public int IdMarca { get; private set; }
        public string DescricaoProduto { get; private set; }
        public string IdExterno { get; private set; }
        public int IdTipoProduto { get; private set; }
        public int? IdVendedor { get; private set; }
        public IList<(int idCaracteristicaProduto, string descricao)> CaracteristicaProduto { get; private set; }
        public override Guid IdUsuario { get; }

        public override bool Validate()
        {
            AddNotifications(new Contract<Notification>()
                .IsNotNullOrWhiteSpace(this.NomeProduto, nameof(this.NomeProduto), MensagensProduto.Produto_Cadastro_NomeProdutoIsNotNullOrWhiteSpace)
                .IsGreaterOrEqualsThan(this.NomeProduto, 15, nameof(this.NomeProduto), MensagensProduto.Produto_Cadastro_NomeProdutoIsGreaterOrEqualsThan)
                .IsLowerOrEqualsThan(this.NomeProduto, 150, nameof(this.NomeProduto), MensagensProduto.Produto_Cadastro_NomeProdutoIsLowerOrEqualsThan)
            );

            AddNotifications(new Contract<Notification>()
                .IsGreaterOrEqualsThan(this.Valor, 0.01, nameof(this.Valor), MensagensProduto.Produto_Cadastro_ValorIsGreaterOrEqualsThan)
            );

            AddNotifications(new Contract<Notification>()
                .IsGreaterOrEqualsThan(this.IdMarca, 1, nameof(this.IdMarca), MensagensProduto.Produto_Cadastro_IdMarcaIsGreaterOrEqualsThan)
            );

            AddNotifications(new Contract<Notification>()
                .IsNotNullOrWhiteSpace(this.DescricaoProduto, nameof(this.DescricaoProduto), MensagensProduto.Produto_Cadastro_DescricaoProdutoIsNotNullOrWhiteSpace)
                .IsGreaterOrEqualsThan(this.DescricaoProduto, 30, nameof(this.DescricaoProduto), MensagensProduto.Produto_Cadastro_DescricaoProdutoIsGreaterOrEqualsThan)
                .IsLowerOrEqualsThan(this.DescricaoProduto, 100000, nameof(this.DescricaoProduto), MensagensProduto.Produto_Cadastro_DescricaoProdutoIsLowerOrEqualsThan)
            );

            AddNotifications(new Contract<Notification>()
                .IsLowerOrEqualsThan(this.IdExterno, 500, nameof(this.IdExterno), MensagensProduto.Produto_Cadastro_IdExternoIsLowerOrEqualsThan)
            );

            AddNotifications(new Contract<Notification>()
                .IsGreaterOrEqualsThan(this.IdTipoProduto, 1, nameof(this.IdTipoProduto), MensagensProduto.Produto_Cadastro_IdTipoProdutoIsGreaterOrEqualsThan)
            );

            AddNotifications(new Contract<Notification>()
                .IsNotNull(this.CaracteristicaProduto, nameof(CaracteristicaProduto), MensagensProduto.Produto_Cadastro_CaracteristicaProdutoIsNotNull)
            );

            if(this.IdVendedor.HasValue)
                AddNotifications(new Contract<Notification>()
                    .IsGreaterOrEqualsThan(this.IdVendedor.Value, 1, nameof(this.IdVendedor), MensagensProduto.Produto_Cadastro_IdVendedorIsGreaterOrEqualsThan)
                );

            if (IsValid)
                AddNotifications(new Contract<Notification>()
                    .IsGreaterOrEqualsThan(this.CaracteristicaProduto.Count, 1, nameof(CaracteristicaProduto), MensagensProduto.Produto_Cadastro_CaracteristicaProdutoIsGreaterOrEqualsThan)
                );

            return IsValid;
        }
    }
}
