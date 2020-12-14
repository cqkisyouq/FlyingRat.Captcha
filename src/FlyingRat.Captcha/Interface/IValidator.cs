using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Context;
using FlyingRat.Captcha.Validator;

namespace FlyingRat.Captcha.Interface
{
    public interface IValidator
    {
        string Type { get; }
        BaseCaptchaOptions Options {get;}
        bool AllowValidate(CaptchaValidateContext context, BaseCaptchaOptions options = null);
        ValidateResult Validate(CaptchaValidateContext context, BaseCaptchaOptions options = null);
    }
}
