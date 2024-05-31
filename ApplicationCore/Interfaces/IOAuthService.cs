using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IOAuthService
    {
        public string GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChellange, string ClientId);
        public Task<TokenResult> ExchangeCodeOnTokenAsync(string code, string codeVerifier, string redirectUrl, string ClientId, string ClientSecret);
        public Task<TokenResult> RefreshTokenAsync(string refreshToken, string ClientId, string ClientSecret);
    }
}
