using FSab.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace FSab.Services
{
    public class SMSNotifyer : ISMSNotifyer
    {
        private readonly IConfiguration _configuration;

        public SMSNotifyer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendSMSAsync(string message)
        {
            var apiKey = _configuration["SMSNotifyer:ApiKey"];

            Console.WriteLine("SMS skickas: {message}");
            Console.WriteLine($"API Key hämtad: {string.IsNullOrEmpty(apiKey)}");

            return Task.CompletedTask;
        }
    }
}
