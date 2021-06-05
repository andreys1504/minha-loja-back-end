using MinhaLoja.Core.Infra.Data;
using MinhaLoja.Infra.Data.DataSources.DatabaseMain;
using System.Threading.Tasks;

namespace MinhaLoja.Infra.Data.DataSources
{
    public class ApplicationTransaction : IApplicationTransaction
    {
        private readonly MinhaLojaContext _context;

        public ApplicationTransaction(MinhaLojaContext context)
        {
            _context = context;
        }

        public async Task<int> CommitAsync()
        {
            if (_context != null)
                return await _context.SaveChangesAsync();

            return 0;
        }

        public async Task DisposeAsync()
        {
            if (_context != null)
                await _context.DisposeAsync();
        }
    }
}
