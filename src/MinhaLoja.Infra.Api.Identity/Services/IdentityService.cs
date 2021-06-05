using Microsoft.IdentityModel.Tokens;
using MinhaLoja.Core.Infra.Identity.Services;
using MinhaLoja.Core.Settings;
using NetDevPack.Security.JwtSigningCredentials.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace MinhaLoja.Infra.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly GlobalSettings _globalSettings;
        private readonly IJsonWebKeySetService _jsonWebKeySetService;

        public IdentityService(
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
                new Claim("Id", userId),
                new Claim("SellerId", sellerId ?? ""),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.UserData, userData)
            };

            if (permissions?.Count > 0)
                if (roles is false)
                    permissions.ToList().ForEach(permission => claims.Add(new Claim(permission, "true")));
                else
                    claims.Add(new Claim(ClaimTypes.Role, permissions[0]));

            var dateTimeNow = DateTime.UtcNow;
            var tokenHandler = new JwtSecurityTokenHandler();
            string currentIssuer = $"{requestScheme}://{requestHost}";
            SigningCredentials signingCredentials = _jsonWebKeySetService.GetCurrentSigningCredentials();
            SecurityToken securityToken = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = currentIssuer,
                Subject = new ClaimsIdentity(claims),
                Expires = _globalSettings.Identity.IsHours
                           ? dateTimeNow.AddHours(1)
                           : dateTimeNow.AddDays(_globalSettings.Identity.ExpiresToken),
                SigningCredentials = signingCredentials
            });
            string token = tokenHandler.WriteToken(securityToken);

            return new
            {
                access_token = token,
                token_type = "Bearer",
                expires_in = (_globalSettings.Identity.IsHours
                    ? TimeSpan.FromHours(_globalSettings.Identity.ExpiresToken)
                    : TimeSpan.FromDays(_globalSettings.Identity.ExpiresToken)
                ).TotalSeconds
            };
        }

        public string GetUserData(ClaimsPrincipal user)
        {
            return user?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.UserData)?.Value;
        }

        public string GetUserId(ClaimsPrincipal user)
        {
            return user?.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
        }

        public string GetSellerId(ClaimsPrincipal user)
        {
            return user?.Claims.FirstOrDefault(claim => claim.Type == "SellerId")?.Value;
        }
    }
}
