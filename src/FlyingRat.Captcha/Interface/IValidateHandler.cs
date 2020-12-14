using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Context;
using FlyingRat.Captcha.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Interface
{
    public interface IValidateHandler
    {
        void Validating(CaptchaContext context, BaseCaptchaOptions options = null);
        void Validated(CaptchaContext context, BaseCaptchaOptions options = null);
    }
}
