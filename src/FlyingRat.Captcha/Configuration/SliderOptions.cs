using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Configuration
{
    public class SliderOptions:BaseCaptchaOptions
    {
        public override string Tips { get; set; } = "向右拖动滑块填充拼图";
    }
}
