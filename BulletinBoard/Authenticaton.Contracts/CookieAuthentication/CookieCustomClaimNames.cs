using System;
using System.Collections.Generic;
using System.Text;

namespace Authenticaton.Contracts.CookieAuthentication
{
    public class CookieCustomClaimNames
    {
        public const string AuthToken = "cookie/auth_token";
        public const string UserId = "cookie/user_id";
        public const string UserName = "cookie/username";
        public const string Email = "cookie/email";
        public const string Phone = "cookie/phone";
        public const string FIO = "cookie/FIO";
    }
}
