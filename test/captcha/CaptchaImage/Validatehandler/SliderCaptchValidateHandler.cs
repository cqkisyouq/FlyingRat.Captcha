using FlyingRat.Captcha;
using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Context;
using FlyingRat.Captcha.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Captcha.CaptchaImage.Validatehandler
{
    public class SliderCaptchValidateHandler : BaseValidateHandler<SliderCaptcha>
    {
        public override Task Validated(CaptchaValidateContext context, BaseCaptchaOptions options = null)
        {
            return Task.CompletedTask;
        }

        public override Task Validating(CaptchaValidateContext context, BaseCaptchaOptions options = null)
        {
            return Task.CompletedTask;
        }
    }
}
