using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Context;
using FlyingRat.Captcha.Validator;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlyingRat.Captcha.Interface
{
    public interface ICaptchaManager
    {
        ValueTask<CaptchaImage> Captcha<T>(BaseCaptchaOptions options=null) where T:ICaptcha,new();
        ValueTask<CaptchaImage> Captcha(string type, BaseCaptchaOptions options = null);
        ValueTask<ValidateResult> Validate<T>(CaptchaValidateContext context, BaseCaptchaOptions options = null) where T:ICaptcha,new();
        ValueTask<ValidateResult> Validate(string captcha,CaptchaValidateContext context, BaseCaptchaOptions options = null);
    }
}
