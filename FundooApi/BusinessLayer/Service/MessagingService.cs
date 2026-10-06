using System.Net.Http.Json;
using ModelLayer;

namespace BusinessLayer.Service
{
    public class MessagingService
    {
        private readonly HttpClient _httpClient;

        public MessagingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string body)
        {
            var request = new
            {
                ToEmail = toEmail,
                Subject = subject,
                Body = body
            };

            await _httpClient.PostAsJsonAsync(
                "https://localhost:7002/api/Email/send",
                request);
        }
    }
}