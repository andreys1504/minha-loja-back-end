using Microsoft.IdentityModel.Tokens;
using MinhaLoja.Core.Infra.Identity.Services;
using MinhaLoja.Core.Settings;
using NetDevPack.Security.JwtSigningCredentials.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace MinhaLoja.Infra.Api.Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly GlobalSettings _globalSettings;
        private readonly IJsonWebKeySetService _jsonWebKeySetService;

        public TokenService(
            GlobalSettings globalSettings,
            IJsonWebKeySetService jsonWebKeySetService)
        {
            _globalSettings = globalSettings;
            _jsonWebKeySetService = jsonWebKeySetService;
        }

        public object GenerateToken(
            string requestScheme,
            string requestHost,
            string userId,
            string sellerId,
            string username,
            string userData,
            IList<string> permissions,
            bool roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("SellerId", sellerId ?? ""),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.UserData, userData)
            };

            if (permissions?.Count > 0)
                if (roles == false)
                    permissions.ToList().ForEach(permission => claims.Add(new Claim(permission, "true")));
                else
                    claims.Add(new Claim(ClaimTypes.Role, permissions[0]));

            var dateTimeNow = DateTime.UtcNow;
            DateTime expires;
            double expiresTotalSeconds;
            if (_globalSettings.Identity.IsHours)
            {
                expires = dateTimeNow.AddHours(_globalSettings.Identity.ExpiresToken);
                expiresTotalSeconds = TimeSpan.FromHours(_globalSettings.Identity.ExpiresToken).TotalSeconds;
            }
            else
            {
                expires = dateTimeNow.AddDays(_globalSettings.Identity.ExpiresToken);
                expiresTotalSeconds = TimeSpan.FromDays(_globalSettings.Identity.ExpiresToken).TotalSeconds;
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            string currentIssuer = $"{requestScheme}://{requestHost}";
            if (string.IsNullOrWhiteSpace(_globalSettings.Identity.Issuer) == false)
            {
                currentIssuer = _globalSettings.Identity.Issuer;
            }

            SigningCredentials signingCredentials = _jsonWebKeySetService.GetCurrentSigningCredentials();
            SecurityToken securityToken = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = currentIssuer,
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = signingCredentials
            });
            string token = tokenHandler.WriteToken(securityToken);

            return new
            {
                access_token = token,
                token_type = "Bearer",
                expires_in = expiresTotalSeconds
            };
        }

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
