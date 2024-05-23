using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using OAuthTutorial.Helpers;

namespace ApplicationCore.Services
{
    internal class GoogleOAuthService : IOAuthService
    {
        private const string OAuthServerEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        private const string TokenServerEndpoint = "https://oauth2.googleapis.com/token";
        public string GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChellange, string ClientId)
        {
            var queryParams = new Dictionary<string, string>()
            {
                { "client_id", ClientId },
                { "redirect_uri", redirectUrl },
                { "response_type", "code" },
                { "scope", scope },
                { "code_challenge", codeChellange },
                { "code_challenge_method", "S256" },
                { "access_type", "offline" }
            };

            var url = QueryHelpers.AddQueryString(OAuthServerEndpoint, queryParams);
            return url;
        }
        public async Task<TokenResult> ExchangeCodeOnTokenAsync(string code, string codeVerifier, string redirectUrl, string ClientId, string ClientSecret)
        {
            var authParams = new Dictionary<string, string>
            {
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "code", code },
                { "code_verifier", codeVerifier },
                { "grant_type", "authorization_code" },
                { "redirect_uri", redirectUrl }
            };

            var tokenResult = await HttpClientHelper.SendPostRequest<TokenResult>(TokenServerEndpoint, authParams);
            return tokenResult;
        }


        public async Task<TokenResult> RefreshTokenAsync(string refreshToken, string ClientId, string ClientSecret)
        {
            var refreshParams = new Dictionary<string, string>
            {
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken }
            };

            var tokenResult = await HttpClientHelper.SendPostRequest<TokenResult>(TokenServerEndpoint, refreshParams);

            return tokenResult;
        }

    }
}
