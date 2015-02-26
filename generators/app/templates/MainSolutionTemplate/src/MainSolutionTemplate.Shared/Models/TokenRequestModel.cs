using RestSharp.Serializers;

namespace MainSolutionTemplate.Shared.Models
{
    public class TokenRequestModel
    {
        public TokenRequestModel()
        {
            GrantType = "password";
        }

        [SerializeAs(Name = "username")]
        public string UserName { get; set; }

        [SerializeAs(Name = "password")]
        public string Password { get; set; }

        [SerializeAs(Name = "client_id")]
        public string client_id { get; set; }

        [SerializeAs(Name = "grant_type")]
        public string GrantType { get; set; }
    }
}