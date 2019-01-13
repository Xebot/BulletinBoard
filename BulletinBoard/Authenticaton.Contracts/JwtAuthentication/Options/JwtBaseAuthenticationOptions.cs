namespace Authenticaton.Contracts.JwtAuthentication.Options
{
    public class JwtBaseAuthenticationOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Secret {get;set;}
    }
}
