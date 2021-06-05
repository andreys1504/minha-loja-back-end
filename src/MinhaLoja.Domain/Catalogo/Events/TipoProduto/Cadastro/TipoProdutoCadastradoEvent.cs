using MediatR;
using MinhaLoja.Core.Domain.Events;
using System.Collections.Generic;

namespace MinhaLoja.Domain.Catalogo.Events.TipoProduto.Cadastro
{
    public class TipoProdutoCadastradoEvent : DomainEvent, INotification
    {
        public TipoProdutoCadastradoEvent(
            int idTipoProduto, 
            string nomeTipoProduto, 
            int? idTipoProdutoSuperior, 
            IEnumerable<(int idCaracteristica, string nome, string observacao)> caracteristicasTipoProduto)
        {
            IdTipoProduto = idTipoProduto;
            NomeTipoProduto = nomeTipoProduto;
            IdTipoProdutoSuperior = idTipoProdutoSuperior;
            CaracteristicasTipoProduto = caracteristicasTipoProduto;
        }

        public int IdTipoProduto { get; private set; }
        public string NomeTipoProduto { get; private set; }
        public int? IdTipoProdutoSuperior { get; private set; }
        public IEnumerable<(int idCaracteristica, string nome, string observacao)> CaracteristicasTipoProduto { get; private set; }
    }
}
