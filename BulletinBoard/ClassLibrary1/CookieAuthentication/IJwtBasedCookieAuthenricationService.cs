using System.Threading.Tasks;
using Authenticaton.Contracts;
using Authenticaton.Contracts.Basic;
using WebApi.Contracts.DTO;

namespace Authentication.AppServices.CookieAuthentication
{
    /// <summary>
    /// Сервис аутентификации через куки, основываясь на внешней аутентификации через JWT токен
    /// </summary>
    public interface IJwtBasedCookieAuthenricationService
    {
        ///<summary>
        ///Выполняет аутентификацию
        /// </summary>
        Task<AuthenticationResult> SignInAsync(BasicAuthenticationRequest request);
        /// <summary>
        /// Выполняет вход пользователя который вошел через соц сети
        /// </summary>
       
        Task<AuthenticationResult> SignInExternalAsync(string UserEmail);
        ///<summary>
        ///Выполняет выход
        /// </summary>
        Task SignOutAsync();
    }
}
