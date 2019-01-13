using Authenticaton.Contracts.Basic;
using Authenticaton.Contracts.JwtAuthentication;
using System.Threading.Tasks;
using WebApi.Contracts.DTO;

namespace Authentication.AppServices.JwtAuthentication
{
    /// <summary>
    /// Сервис JWT аутентификации
    /// </summary>
    public interface IJwtAuthenticationService
    {
        Task<JwtAuthenticationResult> AuthenticateAsync(BasicAuthenticationRequest request);
        Task<JwtAuthenticationResult> AuthenticateExternalAsync(string UserEmail);
        
    }
}
