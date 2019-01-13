using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Contracts.DTO.Base;

namespace WebApi.Contracts.DTO
{
    public class UserDto : EntityDto<Guid>
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
        /// ФИО пользователя
        /// </summary>
        public string FIO { get; set; }

        /// <summary>
        /// E-mail пользователя
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Регион пользователя
        /// </summary>        
        public string UserRegion { get; set; }
        /// <summary>
        /// Регион пользователя
        /// </summary>        
        public int RegionId { get; set; }

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
        public IList<string> UserRole { get; set; }

        /// <summary>
        /// Статус пользователя. Забанен или нет?
        /// </summary>
        public string UserStatus { get; set; }
    }
}
