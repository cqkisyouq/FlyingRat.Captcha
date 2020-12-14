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

        public abstract BaseCaptchaOptions Options { get; }

        public virtual bool AllowValidate(CaptchaValidateContext context, BaseCaptchaOptions options = null)
        {
            var result = context.GetResult();
            result.AllowValidate = options==null? result.Count<Options?.Validate_Max: result.Count < options.Validate_Max;
            return result.AllowValidate;
        }
        public abstract ValidateResult Validate(CaptchaValidateContext context, BaseCaptchaOptions options = null);
    }
}
