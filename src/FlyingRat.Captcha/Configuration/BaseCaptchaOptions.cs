using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Configuration
{
    public class BaseCaptchaOptions
    {
        public virtual int Col { get; set; } = 1;
        public virtual int Row { get; set; } = 1;
        public virtual int Offset { get; set; } = 1;
        public virtual string Tips { get; set; }
    }
}
