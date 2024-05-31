using ApplicationCore.Helpers;
using ApplicationCore.Interfaces;
using Google.Apis.Calendar.v3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApp.Interfaces;

namespace WebApp.Controllers
{
    /// <summary>
    /// Controller for handling Google OAuth operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleOAuthController : ControllerBase
    {
        private readonly IOAuthService _oAuthService;
        private readonly IGoogleCalendarService _googleCalendarService;
        private readonly IConfiguration _configuration;
        private readonly ITokenStorageService _tokenStorageService;
        private string RedirectUrl { get; init; } // Action wich auth server calls with "Authorization Code" after authentification
        //private const string GoogleCallendarScope = "https://www.googleapis.com/auth/calendar"; // View and edit all your calendars
        //private const string GoogleCallendarScope = "https://www.googleapis.com/auth/calendar.events"; // View and edit events on all your calendars
        private const string GoogleCallendarScope = CalendarService.ScopeConstants.CalendarEvents;
        private const string PkceSessionKey = "codeVerifier";  // Key for save "Proof key/Code Verifier" value in UserSession
        private const string RememberMeSessionKey = "rememberMe";  // Key for save "Remember me" value in UserSession

        public GoogleOAuthController(IOAuthService oAuthService,
                                     IConfiguration configuration,
                                     ITokenStorageService tokenStorageService,
                                     IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                RedirectUrl = "https://university-web.azurewebsites.net/api/GoogleOAuth/Code";
            }
            else if (env.IsDevelopment())
            {
                RedirectUrl = "https://localhost:7159/api/GoogleOAuth/Code";
            }

            _oAuthService = oAuthService;
            _configuration = configuration;
            _tokenStorageService = tokenStorageService;
        }

        /// <summary>
        /// Redirects to OAuth server for authorization.
        /// </summary>
        /// <param name="rememberMe">Flag indicating whether to remember the user's authorization.</param>
        /// <returns>Redirects to the OAuth server.</returns>
        [HttpGet("RedirectOnOAuthServer")]
        public IActionResult RedirectOnOAuthServer([FromQuery]bool rememberMe = false)
        {
            // PCKE.
            var codeVerifier = Guid.NewGuid().ToString();
            var codeChellange = Sha256Helper.ComputeHash(codeVerifier);

            HttpContext.Session.SetString(PkceSessionKey, codeVerifier);
            HttpContext.Session.SetString(RememberMeSessionKey, rememberMe.ToString());

            var clientId = _configuration["Google:ClientId"];

            var url = _oAuthService.GenerateOAuthRequestUrl(GoogleCallendarScope, RedirectUrl, codeChellange, clientId);
            return Redirect(url);
        }

        /// <summary>
        /// Handles the code returned from OAuth server.
        /// </summary>
        /// <param name="code">The authorization code.</param>
        /// <returns>HTTP status indicating the result of code exchange.</returns>
        [HttpGet("Code")]
        public async Task<IActionResult> Code(string code)
        {
            var codeVerifier = HttpContext.Session.GetString(PkceSessionKey);

            // Attention: the update token is provided only at the first user authorization!
            var clientId = _configuration["Google:ClientId"];
            var clientSecret = _configuration["Google:ClientSecret"];

            var tokenResult = await _oAuthService.ExchangeCodeOnTokenAsync(code, codeVerifier, RedirectUrl, clientId, clientSecret);

            if (tokenResult == null)
            {
                return BadRequest("Failed to exchange code for token.");
            }

            bool rememberMe;
            bool.TryParse(HttpContext.Session.GetString(RememberMeSessionKey), out rememberMe);

            _tokenStorageService.StoreTokens(tokenResult.AccessToken, rememberMe, HttpContext, tokenResult.RefreshToken);

            return Ok();
        }

        /// <summary>
        /// Refreshes the access token using the refresh token.
        /// </summary>
        /// <param name="redirectUrl">Optional URL to redirect to after token refresh.</param>
        /// <returns>HTTP status indicating the result of token refresh.</returns>
        //[Authorize]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromQuery] string? redirectUrl = null)
        {
            bool rememberMe = true;
            // Checking whether the Refresh token is present in HTTP-only cookies
            var refreshToken = Request.Cookies["RefreshToken"];

            // If the Refresh token is not found in the cookies, we will try to get it from the session
            if (string.IsNullOrEmpty(refreshToken))
            {
                rememberMe = false;
                refreshToken = HttpContext.Session.GetString("RefreshToken");
            }

            // Refresh token check
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized();
            }

            var clientId = _configuration["Google:ClientId"];
            var clientSecret = _configuration["Google:ClientSecret"];

            var newTokenResponse = await _oAuthService.RefreshTokenAsync(refreshToken, clientId, clientSecret);
            if (newTokenResponse != null)
                _tokenStorageService.StoreTokens(newTokenResponse.AccessToken, rememberMe, HttpContext);

            if (!redirectUrl.IsNullOrEmpty())
                return Redirect(redirectUrl);
            return Ok();
        }
    }
}
