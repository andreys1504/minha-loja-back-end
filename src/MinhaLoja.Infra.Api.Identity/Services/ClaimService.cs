using MinhaLoja.Core.Infra.Identity.Services;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace MinhaLoja.Infra.Api.Identity.Services
{
    public class ClaimService : IClaimService
    {
        public string GetUserData(ClaimsPrincipal user)
        {
            return user?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.UserData)?.Value;
        }

        public string GetUserId(ClaimsPrincipal user)
        {
            return user?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public string GetSellerId(ClaimsPrincipal user)
        {
            return user?.Claims.FirstOrDefault(claim => claim.Type == "SellerId")?.Value;
        }

        public IEnumerable<Claim> GetClaims(string tokenJwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = tokenHandler.ReadJwtToken(tokenJwt.Replace("Bearer", "").Trim());

            return jwtSecurityToken.Claims;
        }
    }
}
