using FlyingRat.Captcha.Validator;
using FlyingRat.Captcha.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Extensions
{
    public static class CaptchaVerifyExtension
    {
        public static ValidateModel ToValidateModel(this CaptchaVerifyModel captcha)
        {
            if (captcha == null) return new ValidateModel();
            var model = new ValidateModel();
            model.Code = captcha.Code;
            model.Points = captcha.Points;
            return model;
        }
    }
}
