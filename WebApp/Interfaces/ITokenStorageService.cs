namespace WebApp.Interfaces
{
    public interface ITokenStorageService
    {
        void StoreTokens(string accessToken, bool rememberMe, HttpContext context, string? refreshToken = null);
        (string AccessToken, string RefreshToken) RetrieveToken(bool rememberMe, HttpContext context);
    }
}
