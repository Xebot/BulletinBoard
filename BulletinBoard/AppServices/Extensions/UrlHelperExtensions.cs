using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppServices.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string Id, string code)
        {

            var scheme = urlHelper.ActionContext.HttpContext.Request.Scheme;
            return urlHelper.Action(
                action:"ConfirmEmail",
                controller:"user",
                values: new { userId = Id, code = code },
                protocol: scheme);
       
        }

        //public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        //{
        //    return urlHelper.Action(
        //        action: nameof(AccountController.ResetPassword),
        //        controller: "Account",
        //        values: new { userId, code },
        //        protocol: scheme);
        //}
    }
}
