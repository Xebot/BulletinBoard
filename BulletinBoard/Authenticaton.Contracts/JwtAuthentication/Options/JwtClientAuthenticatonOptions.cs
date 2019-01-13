namespace Authenticaton.Contracts.JwtAuthentication.Options
{
    public class JwtClientAuthenticatonOptions:JwtBaseAuthenticationOptions
    {
        public string AuthenticationEndpoint { get; set; }
    }
}
