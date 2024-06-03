using Microsoft.IdentityModel.Tokens;
using WebApp.Interfaces;

namespace WebApp.Services
{
    public class TokenStorageService : ITokenStorageService
    {
        private readonly IConfiguration _configuration;

        public TokenStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void StoreTokens(string accessToken, bool rememberMe, HttpContext context, string? refreshToken = null)
        {
            if (rememberMe)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                };
                context.Response.Cookies.Append("AccessToken", accessToken, cookieOptions);
                if (!refreshToken.IsNullOrEmpty()) 
                    context.Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
            }
            else
            {
                context.Session.SetString("AccessToken", accessToken);
                if (!refreshToken.IsNullOrEmpty())
                    context.Session.SetString("RefreshToken", refreshToken);
            }
        }
        public (string AccessToken, string RefreshToken) RetrieveToken(bool rememberMe, HttpContext context)
        {
            if (rememberMe)
            {
                var accessToken = context.Request.Cookies["AccessToken"];
                var refreshToken = context.Request.Cookies["RefreshToken"];
                return (accessToken, refreshToken);
            }
            else
            {
                var accessToken = context.Session.GetString("AccessToken");
                var refreshToken = context.Session.GetString("RefreshToken");
                return (accessToken, refreshToken);
            }
        }
    }
}
