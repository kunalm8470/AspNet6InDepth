namespace Api.Models
{
    public class Auth0Configuration
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public int RetryCount { get; set; }
    }
}
