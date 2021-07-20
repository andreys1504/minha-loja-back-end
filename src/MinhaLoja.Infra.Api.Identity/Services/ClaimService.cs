using MinhaLoja.Core.Infra.Identity.Services;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace MinhaLoja.Infra.Api.Identity.Services
{
    public class ClaimService : IClaimService
    {
        public string GetUserId(ClaimsPrincipal user)
        {
            return GetValueClaim(user, ClaimTypes.NameIdentifier);
        }

        public string GetUserData(ClaimsPrincipal user)
        {
            return GetValueClaim(user, ClaimTypes.Actor);
        }

        public string GetSellerId(ClaimsPrincipal user)
        {
            return GetValueClaim(user, "SellerId");
        }

        public IEnumerable<Claim> GetClaims(string tokenJwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = tokenHandler.ReadJwtToken(tokenJwt.Replace("Bearer", "").Trim());

            return jwtSecurityToken.Claims;
        }


        private string GetValueClaim(
            ClaimsPrincipal user,
            string claimType)
        {
            if (user == null)
            {
                return "";
            }

            var claim = user.Claims.FirstOrDefault(claim => claim.Type == claimType);
            if (claim == null)
            {
                return "";
            }

            return claim.Value;
        }
    }
}
