using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Configuration
{
    public class BaseCaptchaOptions
    {
        public int Col { get; set; } = 1;
        public int Row { get; set; } = 1;
        public int Offset { get; set; } = 1;
        public string Tips { get; set; }
    }
}
