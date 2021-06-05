using System.Threading.Tasks;

namespace MinhaLoja.Core.Infra.Data
{
    public interface IApplicationTransaction
    {
        Task<int> CommitAsync();

        Task DisposeAsync();
    }
}
