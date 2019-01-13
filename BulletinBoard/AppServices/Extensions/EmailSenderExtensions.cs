using AppServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AppServices.Extensions
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Подтвердите Ваш email",
                $"Пожалуйста, подтвердите свой email пройдя по : <a href='{HtmlEncoder.Default.Encode(link)}'>ссылке</a>");
        }
    }
}
