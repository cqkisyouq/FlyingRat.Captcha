using FlyingRat.Captcha.Context;
using FlyingRat.Captcha.Interface;
using FlyingRat.Captcha.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha
{
    public class ValidatorManager : IValidateManager
    {
        public ValidatorManager()
        {

        }
        public ValidateResult Validate(CaptchaValidateContext context)
        {
            throw new NotImplementedException();
        }
    }
}
