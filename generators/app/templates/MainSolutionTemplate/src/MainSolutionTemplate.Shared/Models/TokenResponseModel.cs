
using Newtonsoft.Json;

namespace MainSolutionTemplate.Shared.Models
{
    public class TokenResponseModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        
        public string ClientId { get; set; }
        
        public string UserName { get; set; }
        
        public string DisplayName { get; set; }
        
        public string Permissions { get; set; }

    }
}