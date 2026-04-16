using Google.Apis.Auth;

namespace AuthService.Auth
{
    public interface IGoogleTokenValidator
    {
        Task<GoogleJsonWebSignature.Payload?> ValidateAsync(string idToken);
    }

    public class GoogleTokenValidator : IGoogleTokenValidator
    {
        private readonly IConfiguration _config;
        private readonly ILogger<GoogleTokenValidator> _logger;

        public GoogleTokenValidator(IConfiguration config, ILogger<GoogleTokenValidator> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<GoogleJsonWebSignature.Payload?> ValidateAsync(string idToken)
        {
            try
            {
                var payload          = await GoogleJsonWebSignature.ValidateAsync(idToken);
                var expectedClientId = _config["Google:ClientId"];
                bool isValid = payload.Audience switch
                {
                    string aud                => aud == expectedClientId,
                    IEnumerable<string> auds  => auds.Contains(expectedClientId),
                    _                         => false
                };

                if (!isValid)
                {
                    _logger.LogWarning("Audience mismatch. Expected: {Expected}, Got: {Actual}",
                        expectedClientId, payload.Audience);
                    return null;
                }

                _logger.LogInformation("Google token validated for {Email}", payload.Email);
                return payload;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Google token validation failed: {Message}", ex.Message);
                return null;
            }
        }
    }
}
