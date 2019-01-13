using System;

namespace Authenticaton.Contracts
{
    public class AuthenticationResult
    {
        public static AuthenticationResult Succeed { get; } = new AuthenticationResult(errors: Array.Empty<string>(), isSucceed: true);

        public bool IsSucceed { get; }
        public string[] Errors { get; }

        private AuthenticationResult(bool isSucceed, string[] errors)
        {
            IsSucceed = isSucceed;
            Errors = errors;
        }
        public static AuthenticationResult Failed(params string[] errors) => new AuthenticationResult(errors: errors, isSucceed: false);
    }
}
