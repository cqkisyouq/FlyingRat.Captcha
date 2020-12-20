using FlyingRat.Captcha.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.ViewModel
{
    public class CaptchaVerifyModel
    {
        public string TK { get; set; }
        public List<CaptchaPoint> Points { get; set; }
        public string Code { get; set; }
    }
}
