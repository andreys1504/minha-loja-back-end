using System.Collections.Generic;

namespace MinhaLoja.Core.Infra.Identity.Services
{
    public interface ITokenManagementService
    {
        object GenerateToken(
            string requestScheme,
            string requestHost,
            string userId,
            string sellerId,
            string username,
            string userData,
            IList<string> permissions,
            bool roles);

        bool ValidateToken(
            string token,
            string requestScheme,
            string requestHost);

        string CurrentUser(string token);
    }
}
