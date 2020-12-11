using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Configuration
{
    public class PointOptions:BaseCaptchaOptions
    {
        public override string Tips { get; set; } = "请依次点击";
    }
}
