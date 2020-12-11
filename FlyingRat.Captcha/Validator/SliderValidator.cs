using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Context;
using FlyingRat.Captcha.Interface;
using Microsoft.Extensions.Options;
using System;

namespace FlyingRat.Captcha.Validator
{
    public class SliderValidator : BaseValidator<SliderCaptcha>
    {
        private readonly SliderOptions _options;
        public SliderValidator(IOptions<SliderOptions> options)
        {
            _options = options?.Value;
        }
        public SliderValidator()
        {

        }
        public override ValidateResult Validate(CaptchaContext context, BaseCaptchaOptions options)
        {
            var model = context.GetResult();
            model.IsValidate = true;
            model.Count++;
            model.AllowValidate = true;
            model.Succeed = false;

            if (context.Validate?.Points?.Count != 1) return model;
            var targetPoint = context.Validate.Points[0];
            var vPoint = context.Source.Points[0];

            var x = Math.Abs(vPoint.X - targetPoint.X);
            var y = Math.Abs(vPoint.Y - targetPoint.Y);
            var range = options?.Offset ?? _options.Offset;
            if (x >= 0 && x <= range && y >= 0 && y <= range)
            {
                model.Succeed = true;
            }
            return model;
        }
    }
}
