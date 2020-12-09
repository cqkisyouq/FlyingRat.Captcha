using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Context;
using FlyingRat.Captcha.Validator;

namespace FlyingRat.Captcha.Interface
{
    public interface IValidator
    {
        string Type { get; }
        ValidateResult Validate(CaptchaContext context, BaseCaptchaOptions options = null);
    }
}
