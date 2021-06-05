using MediatR;
using MinhaLoja.Core.Domain.Events;
using System.Collections.Generic;

namespace MinhaLoja.Domain.Catalogo.Events.Produto.Cadastro
{
    public class ProdutoCadastradoEvent : DomainEvent, INotification
    {
        public ProdutoCadastradoEvent(
            int idProduto, 
            string nomeProduto, 
            decimal valor, 
            int idMarca, 
            string descricaoProduto, 
            string idExterno, 
            int idTipoProduto, 
            int? idVendedor, 
            IEnumerable<(int idCaracteristicaProduto, string descricao)> caracteristicaProduto)
        {
            IdProduto = idProduto;
            NomeProduto = nomeProduto;
            Valor = valor;
            IdMarca = idMarca;
            DescricaoProduto = descricaoProduto;
            IdExterno = idExterno;
            IdTipoProduto = idTipoProduto;
            IdVendedor = idVendedor;
            CaracteristicaProduto = caracteristicaProduto;
        }

        public int IdProduto { get; private set; }
        public string NomeProduto { get; private set; }
        public decimal Valor { get; private set; }
        public int IdMarca { get; private set; }
        public string DescricaoProduto { get; private set; }
        public string IdExterno { get; private set; }
        public int IdTipoProduto { get; private set; }
        public int? IdVendedor { get; private set; }
        public IEnumerable<(int idCaracteristicaProduto, string descricao)> CaracteristicaProduto { get; private set; }

    }
}
