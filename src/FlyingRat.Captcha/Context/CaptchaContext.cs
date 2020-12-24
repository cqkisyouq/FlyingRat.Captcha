using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Model;

namespace FlyingRat.Captcha.Context
{
    public class CaptchaContext
    {
        public CaptchaContext() { }
        public CaptchaContext(CaptchaImage captcha=null, BaseCaptchaOptions options=null)
        {
            this.Captcha = captcha;
            this.Options = options;
        }
        public CaptchaImage Captcha { get; set; }
        public BaseCaptchaOptions Options { get; set; }
    }
}
