using System.Collections.Generic;
using System.Security.Claims;

namespace MinhaLoja.Core.Infra.Identity.Services
{
    public interface IClaimService
    {
        string GetUserData(ClaimsPrincipal user);

        string GetUserId(ClaimsPrincipal user);

        string GetSellerId(ClaimsPrincipal user);

        IEnumerable<Claim> GetClaims(string tokenJwt);
    }
}
