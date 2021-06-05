using System.Collections.Generic;
using System.Security.Claims;

namespace MinhaLoja.Core.Infra.Identity.Services
{
    public interface IIdentityService
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
        
        string GetUserData(ClaimsPrincipal user);

        string GetUserId(ClaimsPrincipal user);

        string GetSellerId(ClaimsPrincipal user);
    }
}
