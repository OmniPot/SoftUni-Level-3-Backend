namespace Battleships.WebServices.Models
{
    using Newtonsoft.Json;

    public class LoginUserData
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        public string UserName { get; set; }
    }
}