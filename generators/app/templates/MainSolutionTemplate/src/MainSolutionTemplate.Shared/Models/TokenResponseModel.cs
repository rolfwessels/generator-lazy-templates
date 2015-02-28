using RestSharp.Deserializers;

namespace MainSolutionTemplate.Shared.Models
{
    public class TokenResponseModel
    {
        [DeserializeAs(Name = "access_token")]
        public string AccessToken { get; set; }

        [DeserializeAs(Name = "token_type")]
        public string TokenType { get; set; }

        [DeserializeAs(Name = "expires_in")]
        public int ExpiresIn { get; set; }

        [DeserializeAs(Name = "client_id")]
        public string ClientId { get; set; }

        [DeserializeAs(Name = "userName")]
        public string UserName { get; set; }

        [DeserializeAs(Name = "displayName")]
        public string DisplayName { get; set; }

        [DeserializeAs(Name = "permissions")]
        public string Permissions { get; set; }

    }
}