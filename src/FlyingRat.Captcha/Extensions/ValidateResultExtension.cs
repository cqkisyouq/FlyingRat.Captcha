using FlyingRat.Captcha.Validator;
using FlyingRat.Captcha.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Extensions
{
    public static class ValidateResultExtension
    {
        public static ValidateViewModel ToValidateModel(this ValidateResult captcha)
        {
            if (captcha == null) return new ValidateViewModel();
            var model = new ValidateViewModel();
            model.Succeed = captcha.Succeed;
            model.Token = captcha.Token;
            model.Refresh = !captcha.AllowValidate;
            return model;
        }
    }
}
