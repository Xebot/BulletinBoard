namespace Authenticaton.Contracts.Basic
{
    public class BasicAuthenticationRequest
    {
        ///<summary>
        ///Логин пользователя
        ///</summary>
        public string Email { get; set; }

        ///<summary>
        ///Пароль пользователя
        /// </summary>
        public string Password { get; set; }
    }
}
