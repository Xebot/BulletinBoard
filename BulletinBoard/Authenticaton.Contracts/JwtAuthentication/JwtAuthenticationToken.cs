using System;
using System.Collections.Generic;
using System.Text;

namespace Authenticaton.Contracts.JwtAuthentication
{
    public class JwtAuthenticationToken
    {
        public string AuthToken { get; set; }
        public DateTime? Expires { get; set; }
        public Guid UserId { get; set; }
    }
}
