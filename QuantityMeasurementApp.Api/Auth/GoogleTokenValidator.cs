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
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = [_config["Google:ClientId"]!]
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                return payload;
            }
            catch (InvalidJwtException ex)
            {
                _logger.LogWarning("Google token validation failed: {Message}", ex.Message);
                return null;
            }
        }
    }
}
