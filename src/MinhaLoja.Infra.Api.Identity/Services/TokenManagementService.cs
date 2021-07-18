using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MinhaLoja.Core.Infra.Identity.Services;
using MinhaLoja.Core.Settings;
using NetDevPack.Security.JwtSigningCredentials.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MinhaLoja.Infra.Api.Identity.Services
{
    public class TokenManagementService : ITokenManagementService
    {
        private readonly GlobalSettings _globalSettings;
        private readonly IJsonWebKeySetService _jsonWebKeySetService;

        public TokenManagementService(
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

            var tokenHandler = new JsonWebTokenHandler();

            string currentIssuer = $"{requestScheme}://{requestHost}";
            if (string.IsNullOrWhiteSpace(_globalSettings.Identity.Issuer) == false)
            {
                currentIssuer = _globalSettings.Identity.Issuer;
            }

            SigningCredentials signingCredentials = _jsonWebKeySetService.GetCurrentSigningCredentials();
            string token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = currentIssuer,
                Audience = "TESTE", //TODO
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = signingCredentials
            });

            return new
            {
                access_token = token,
                token_type = "Bearer",
                expires_in = expiresTotalSeconds
            };
        }

        public bool ValidateToken(
            string token,
            string requestScheme,
            string requestHost)
        {
            var tokenHandler = new JsonWebTokenHandler();
            var currentIssuer = $"{requestScheme}://{requestHost}";
            SigningCredentials signingCredentials = _jsonWebKeySetService.GetCurrentSigningCredentials();

            TokenValidationResult result = tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidIssuer = currentIssuer,
                    ValidAudience = "TESTE", //TODO
                    IssuerSigningKey = signingCredentials.Key
                });

            return result.IsValid;
        }
    }
}
