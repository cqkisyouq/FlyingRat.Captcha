using FlyingRat.Captcha.Context;
using FlyingRat.Captcha.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Interface
{
    public interface IValidateManager
    {
        ValidateResult Validate(CaptchaContext context);
    }
}
