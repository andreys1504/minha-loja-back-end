using MinhaLoja.Core.Infra.Data;
using System.Threading.Tasks;

namespace MinhaLoja.Tests.Fakes.Infra.Data
{
    public class ApplicationTransactionFake : IApplicationTransaction
    {
        public Task<int> CommitAsync()
        {
            return Task.FromResult(1);
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
