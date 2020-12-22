using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Context;
using FlyingRat.Captcha.Validator;
using System.Threading.Tasks;

namespace FlyingRat.Captcha.Interface
{
    public interface IValidator
    {
        string Type { get; }
        BaseCaptchaOptions Options {get;}
        bool AllowValidate(CaptchaValidateContext context, BaseCaptchaOptions options = null);
        ValueTask<ValidateResult> Validate(CaptchaValidateContext context, BaseCaptchaOptions options = null);
    }
}
