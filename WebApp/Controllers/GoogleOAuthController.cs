using ApplicationCore.Entities;
using ApplicationCore.Helpers;
using ApplicationCore.Interfaces;
using Azure.Core;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebApp.Controllers
{
    /// <summary>
    /// Controller for handling Google OAuth operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleOAuthController : Controller
    {
        private readonly IOAuthService _oAuthService;
        private readonly IGoogleCalendarService _googleCalendarService;
        private readonly IConfiguration _configuration;
        private const string RedirectUrl = "https://localhost:7159/api/GoogleOAuth/Code"; // Action wich auth server calls with "Authorization Code" after authentification
        //private const string GoogleCallendarScope = "https://www.googleapis.com/auth/calendar"; // View and edit all your calendars
        //private const string GoogleCallendarScope = "https://www.googleapis.com/auth/calendar.events"; // View and edit events on all your calendars
        private string GoogleCallendarScope = CalendarService.Scope.CalendarEvents;
        private const string PkceSessionKey = "codeVerifier";  // Key for save "Proof key/Code Verifier" value in UserSession
        private const string RememberMeSessionKey = "rememberMe";  // Key for save "Remember me" value in UserSession

        public GoogleOAuthController(IOAuthService oAuthService,
                                     IGoogleCalendarService googleCalendarService,
                                     IConfiguration configuration)
        {

            _oAuthService = oAuthService;
            _googleCalendarService = googleCalendarService;
            _configuration = configuration;
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

            var clientId = _configuration["ClientCredentials:ClientId"];

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
            var clientId = _configuration["ClientCredentials:ClientId"];
            var clientSecret = _configuration["ClientCredentials:ClientSecret"];

            var tokenResult = await _oAuthService.ExchangeCodeOnTokenAsync(code, codeVerifier, RedirectUrl, clientId, clientSecret);

            if (tokenResult == null)
            {
                return BadRequest("Failed to exchange code for token.");
            }

            bool rememberMe;
            bool.TryParse(HttpContext.Session.GetString(RememberMeSessionKey), out rememberMe);

            if (rememberMe)
            {
                // Save tokens in long-term storage (e.g., HTTP-only cookies or secure storage)
                // For example, saving in HTTP-only cookies
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Adjust according to your needs
                    SameSite = SameSiteMode.Strict, // Adjust according to your needs
                };

                Response.Cookies.Append("AccessToken", tokenResult.AccessToken, cookieOptions);
                Response.Cookies.Append("RefreshToken", tokenResult.RefreshToken, cookieOptions);
            }
            else
            {
                // Save tokens in user session
                // For example, using ASP.NET Core sessions
                HttpContext.Session.SetString("AccessToken", tokenResult.AccessToken);
                HttpContext.Session.SetString("RefreshToken", tokenResult.RefreshToken);
            }

            return Ok();
        }

        /// <summary>
        /// Refreshes the access token using the refresh token.
        /// </summary>
        /// <param name="redirectUrl">Optional URL to redirect to after token refresh.</param>
        /// <returns>HTTP status indicating the result of token refresh.</returns>
        [HttpPost]
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

            var clientId = _configuration["ClientCredentials:ClientId"];
            var clientSecret = _configuration["ClientCredentials:ClientSecret"];

            var newTokenResponse = await _oAuthService.RefreshTokenAsync(refreshToken, clientId, clientSecret);
            if (newTokenResponse != null)
            {
                if (rememberMe)
                {
                    // Storing tokens in long-term storage (for example, HTTP-only cookies or secure storage)
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true, // Customize according to your needs
                        SameSite = SameSiteMode.Strict, // Customize according to your needs
                    };

                    Response.Cookies.Append("AccessToken", newTokenResponse.AccessToken, cookieOptions);
                }
                else
                {
                    // Saving tokens in the user session
                    // For example, using ASP.NET Core sessions
                    HttpContext.Session.SetString("AccessToken", newTokenResponse.AccessToken);
                }
            }
            if (!redirectUrl.IsNullOrEmpty())
                return Redirect(redirectUrl);
            return Ok();
        }

        /// <summary>
        /// Creates a new event in the Google Calendar.
        /// </summary>
        /// <param name="summary">Summary of the event.</param>
        /// <param name="description">Description of the event.</param>
        /// <param name="startDateTime">Start date and time of the event.</param>
        /// <param name="endDateTime">End date and time of the event.</param>
        /// <param name="calendarId">ID of the calendar to add the event to.</param>
        /// <param name="timeZone">Time zone of the event.</param>
        /// <returns>HTTP status indicating the result of event creation.</returns>
        [HttpPost("CreateEvent")]
        public async Task<IActionResult> CreateEvent(string summary = "Lecture Schedule",
                                              string description = "Lecture for the upcoming semester.",
                                              DateTime? startDateTime = null,
                                              DateTime? endDateTime = null,
                                              string calendarId = "primary",
                                              string timeZone = "America/Los_Angeles")
        {
            startDateTime ??= new DateTime(2024, 9, 1, 10, 0, 0);
            endDateTime ??= new DateTime(2024, 9, 1, 12, 0, 0);

            string accessToken;

            // Checking whether the Access token is present in HTTP-only cookies
            accessToken = Request.Cookies["AccessToken"];

            // If the Access token is not found in the cookies, we will try to get it from the session
            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = HttpContext.Session.GetString("AccessToken");
            }

            // Access token check
            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest("Access token is missing.");
            }

            // Calling the service method to create an event in Google Calendar using the received Access token
            await _googleCalendarService.CreateEventAsync(accessToken, calendarId, summary, description, startDateTime.Value, endDateTime.Value, timeZone);

            return Ok("Event created successfully.");
        }


    }
}
