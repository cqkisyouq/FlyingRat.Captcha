using FlyingRat.Captcha.Context;
using FlyingRat.Captcha.Interface;
using FlyingRat.Captcha.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha
{
    public static class ValidateHandlerExtension
    {
        public static void Handing<T>(this IEnumerable<ICaptchaHandler> handlers,Action<ICaptchaHandler,T> action,T context)
        {
            if (handlers == null || action == null) return;
            foreach (var item in handlers)
            {
               action(item, context);
            }
        }
    }
}
