namespace OktaOpeIDConnectDemo.Models
{
    public class OktaTokenSettings
    {
        public string OktaDomain { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthorizationServerId { get; set; }
        public string Audience { get; set; }

    }
}
