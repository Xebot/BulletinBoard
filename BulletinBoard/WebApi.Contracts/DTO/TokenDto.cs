using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Contracts.DTO
{
    public class TokenDto
    {
        public string access_token;
        public string expires_in;
        public string user_id;
        public string email;
        public string userName;
    }
}
