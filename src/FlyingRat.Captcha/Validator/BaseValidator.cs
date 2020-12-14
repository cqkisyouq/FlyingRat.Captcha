using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Context;
using FlyingRat.Captcha.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Validator
{
    public abstract class BaseValidator<T> : IValidator where T:ICaptcha,new()
    {
        public string Type => typeof(T).Name;

        public abstract ValidateResult Validate(CaptchaContext context, BaseCaptchaOptions options = null);
    }
}
