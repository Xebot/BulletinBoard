using System;


namespace Authenticaton.Contracts.JwtAuthentication.Options
{
    public class JwtServerAuthenticationOptions : JwtBaseAuthenticationOptions
    {
        public TimeSpan Lifetime { get; set; }
    }
}
