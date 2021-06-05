using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MinhaLoja.Domain.Catalogo.Queries
{
    public static class TipoProdutoQueries
    {
        public static Expression<Func<Entities.TipoProduto, bool>> TipoProdutoValido()
        {
            return tipoProduto => tipoProduto.Ativo;
        }

        public static Expression<Func<Entities.TipoProduto, bool>> 
            TipoProdutoExistenteSistema(string nomeTipoProduto, int? idTipoProdutoSuperior)
        {
            return tipoProduto => tipoProduto.Nome.ToUpper() == nomeTipoProduto.Trim().ToUpper()
                            && tipoProduto.TipoProdutoSuperiorId == idTipoProdutoSuperior;
        }

        public static Expression<Func<Entities.TipoProdutoCaracteristica, bool>> ValidarCaracteristicaCadastroProduto(
            Guid codigoGrupoTipoProduto,
            int numeroOrdemHierarquiaGrupo)
        {
            return caracteristicaTpProduto => caracteristicaTpProduto.TipoProduto.CodigoGrupoTipoProduto == codigoGrupoTipoProduto
                                            && caracteristicaTpProduto.TipoProduto.NumeroOrdemHierarquiaGrupo <= numeroOrdemHierarquiaGrupo;
        }

        public static bool CaracteristicasIguaisExistentes(IEnumerable<string> nomesCaracteristicas)
        {
            return nomesCaracteristicas
                .Select(caracteristica => caracteristica.ToUpper())
                .Distinct()
                .Count() != nomesCaracteristicas.Count();
        }

        public static bool CaracteristicasIguaisExistentes(
            IEnumerable<string> nomesCaracteristicasExistentes,
            IEnumerable<string> nomesCaracteristicasComparacao)
        {
            bool existe = false;
            foreach (string caracteristicaExistente in nomesCaracteristicasExistentes)
                if(nomesCaracteristicasComparacao.Any(comparacao => comparacao.ToUpper() == caracteristicaExistente.ToUpper()))
                {
                    existe = true;
                    break;
                }

            return existe;
        }
    }
}
