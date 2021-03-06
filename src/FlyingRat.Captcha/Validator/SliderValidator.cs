﻿using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Context;
using FlyingRat.Captcha.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace FlyingRat.Captcha.Validator
{
    public class SliderValidator : BaseValidator<SliderCaptcha>
    {
        private readonly SliderOptions _options;

        public override BaseCaptchaOptions Options => _options;

        public SliderValidator(IOptions<SliderOptions> options)
        {
            _options = options?.Value;
        }
        public SliderValidator()
        {

        }
        public override  ValueTask<ValidateResult> Validate(CaptchaValidateContext context, BaseCaptchaOptions options)
        {
            var model = context.GetResult();
            model.IsValidate = true;
            model.Count++;
            model.Succeed = false;
            
            if (context.Validate?.Points?.Count != 1) return new ValueTask<ValidateResult>(model);
            var targetPoint = context.Validate.Points[0];
            var vPoint = context.Source.Points[0];

            var x = Math.Abs(vPoint.X - targetPoint.X);
            var y = Math.Abs(vPoint.Y - targetPoint.Y);
            var range = options?.Offset ?? _options.Offset;
            if (x >= 0 && x <= range && y >= 0 && y <= range)
            {
                model.Succeed = true;
                model.AllowValidate = false;
            }

            return new ValueTask<ValidateResult>(model);
        }
    }
}
