using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Context;
using FlyingRat.Captcha.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Validator
{
    public class PointValidator : BaseValidator<PointCaptcha>
    {
        private readonly PointOptions _options;

        public override BaseCaptchaOptions Options => _options;

        public PointValidator(IOptions<PointOptions> options)
        {
            _options = options?.Value;
        }
        public PointValidator() { }
        public override ValidateResult Validate(CaptchaValidateContext context, BaseCaptchaOptions options)
        {
            var model = context.GetResult();
            model.IsValidate = true;
            model.Count++;
            model.Succeed = false;
            var sourcePoints = context.Source.Points;
            var points = context.Validate.Points;
            if (points?.Count == 0 || points.Count != sourcePoints?.Count) return model;

            var offset =options?.Offset ?? _options.Offset;
            model.Succeed = true;
            for (int i = 0; i < points.Count; i++)
            {
                var pitem = points[i];
                var oldp = sourcePoints[i];
                if (pitem.X >= oldp.X - offset //验证左边范围
                       && pitem.X <= oldp.X + oldp.Width + offset //验证 右边范围
                       && pitem.Y >= oldp.Y - offset  //验证高度上的范围
                       && pitem.Y <= oldp.Y + oldp.Height + offset //验证高度 下的范围
                       )
                {
                    continue;
                }
                model.Succeed = false;
                break;
            }
            return model;
        }
    }
}
