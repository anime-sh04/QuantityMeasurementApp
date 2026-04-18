using Google.Apis.Auth;

namespace QuantityMeasurementApp.Api.Auth
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
                // ✅ Let Google handle validation internally
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

                // ✅ Manual check (safe + flexible)
                var expectedClientId = _config["Google:ClientId"];

                bool isValidAudience = false;

                if (payload.Audience is string aud)
                {
                    isValidAudience = aud == expectedClientId;
                }
                else if (payload.Audience is IEnumerable<string> audList)
                {
                    isValidAudience = audList.Contains(expectedClientId);
                }

                if (!isValidAudience)
                {
                    _logger.LogWarning(
                        "Audience mismatch. Expected: {Expected}, Got: {Actual}",
                        expectedClientId,
                        payload.Audience
                    );
                    return null;
                }

                _logger.LogInformation(
                    "Google token validated for {Email} (sub: {Subject})",
                    payload.Email, payload.Subject);

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