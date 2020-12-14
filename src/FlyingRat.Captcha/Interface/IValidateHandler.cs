using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Context;
using FlyingRat.Captcha.Validator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlyingRat.Captcha.Interface
{
    public interface ICaptchaHandler
    {
        string Type { get; }
        Task Creating(CaptchaContext context);
        Task Created(CaptchaContext context);
        Task Validating(CaptchaValidateContext context, BaseCaptchaOptions options = null);
        Task Validated(CaptchaValidateContext context, BaseCaptchaOptions options = null);
    }
}
