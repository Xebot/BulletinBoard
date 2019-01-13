using System;
using System.Collections.Generic;
using System.Text;

namespace Authenticaton.Contracts.JwtAuthentication
{
    public class JwtCustomClaimNames
    {
        public const string UserId = "JWT/user_id";
        public const string UserName = "JWT/userName";
        public const string Email = "JWT/email";
        public const string Phone = "JWT/phone";
        public const string FIO = "JWT/FIO";
    }
}
