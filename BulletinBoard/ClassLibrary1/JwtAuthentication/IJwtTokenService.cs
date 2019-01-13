using Authenticaton.Contracts.JwtAuthentication;
using BulletinDomain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;


namespace Authentication.AppServices.JwtAuthentication
{
    /// <summary>
    /// Сервис для работы с Jwt токенами
    /// </summary>
    public interface IJwtTokenService
    {
        ///<summary>
        ///Создает JWT токен
        /// </summary>
        JwtAuthenticationToken CreateToken(User user, TimeSpan lifeTime, IList<string> roles);

        ///<summary>
        ///Получает заявки из токенов
        /// </summary>
        Claim[] GetClaims(string authToken);
    }
}
