using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OktaOpeIDConnectDemo.Models;
using System.Net.Http.Headers;

namespace OktaOpeIDConnectDemo.Services
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<OktaTokenSettings> _options;  
        public TokenService(IOptions<OktaTokenSettings> options)
        {
            _options = options;
        }
        public async Task<OktaResponse> GetToken(string username, string password)
        {
            var token = new OktaResponse();
            var client = new HttpClient();
            var client_id = _options.Value.ClientId;
            var client_secret = _options.Value.ClientSecret;
            var clientCreds = System.Text.Encoding.UTF8.GetBytes($"{client_id}:{client_secret}");
            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Basic",System.Convert.ToBase64String(clientCreds));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
           

            var postMessage = new Dictionary<string, string>();
            postMessage.Add("grant_type", "password");
            postMessage.Add("username", username);
            postMessage.Add("password", password);
            postMessage.Add("scope", "openid");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.Value.OktaDomain}/oauth2/default/v1/token")
            {
                Content = new FormUrlEncodedContent(postMessage)
            };

            var response = client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonSerializerSetting = new JsonSerializerSettings();
                var json = await response.Content.ReadAsStringAsync();

                token = JsonConvert.DeserializeObject<OktaResponse>(json, jsonSerializerSetting);
                token.ExpiresAt = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
            }
            else
            {
                var error = await response.Content?.ReadAsStringAsync(); 
                throw new ApplicationException(error);
            }
            return token;
        }
    }
}
