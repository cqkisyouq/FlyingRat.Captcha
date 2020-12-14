using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Context;
using FlyingRat.Captcha.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlyingRat.Captcha.Validator
{
    public abstract class BaseValidateHandler<T> : ICaptchaHandler where T:ICaptcha
    {
        public string Type => typeof(T).Name;

        public virtual Task Validating(CaptchaValidateContext context, BaseCaptchaOptions options = null)
        {
            return Task.CompletedTask;
        }

        public virtual Task Validated(CaptchaValidateContext context, BaseCaptchaOptions options = null)
        {
            return Task.CompletedTask;
        }
        public virtual Task Creating(CaptchaContext context)
        {
            return Task.CompletedTask;
        }
        public virtual Task Created(CaptchaContext context)
        {
            return Task.CompletedTask;
        }
    }
}
