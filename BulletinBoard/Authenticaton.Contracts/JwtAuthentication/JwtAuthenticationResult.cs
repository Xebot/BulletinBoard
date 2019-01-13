using System;
using System.Collections.Generic;
using System.Text;

namespace Authenticaton.Contracts.JwtAuthentication
{
    public class JwtAuthenticationResult
    {
        public JwtAuthenticationToken Token { get; }
        public bool IsSucced { get; }
        public string[] Errors { get; }

        private JwtAuthenticationResult(JwtAuthenticationToken token, bool isSucceed, string[] errors)
        {
            Token = token;
            IsSucced = isSucceed;
            Errors = errors;
        }
        public static JwtAuthenticationResult Failed(params string[] errors) => new JwtAuthenticationResult(token: null, errors: errors, isSucceed: false);
        public static JwtAuthenticationResult Succeed(JwtAuthenticationToken token) => new JwtAuthenticationResult(token: token, errors: Array.Empty<string>(), isSucceed: true);
    }
}
