
namespace WebMVC.Models
{
    public class UserViewModel
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string UserSName { get; set; }

        /// <summary>
        /// E-mail пользователя
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Регион пользователя
        /// </summary>        
        public string UserRegion { get; set; }

        /// <summary>
        /// Адрес пользователя
        /// </summary>
        public string UserAdress { get; set; }

        /// <summary>
        /// Телефон пользователя
        /// </summary>
        public string UserTel { get; set; }

        /// <summary>
        /// Роль пользователя (Админ или простой пользователь)
        /// </summary>
        public string UserRole { get; set; }

        public string FIO { get; set; }
    }
}
