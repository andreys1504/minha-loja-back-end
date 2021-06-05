using MinhaLoja.Core.Infra.Services.LogHandler.Models;
using System.Threading.Tasks;

namespace MinhaLoja.Core.Infra.Services.LogHandler
{
    public interface ILogErrorHandler
    {
        Task SendAsync(ErrorApplicationModel error);
    }
}
