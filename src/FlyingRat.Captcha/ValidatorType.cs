using FlyingRat.Captcha.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha
{
    public static class ValidatorType
    {
        public static string Slider { get; private set; } = typeof(SliderCaptcha).Name;
        public static string Point { get; private set; } = typeof(PointCaptcha).Name;
    }
}
