using FlyingRat.Captcha.Configuration;
using System.Threading.Tasks;

namespace FlyingRat.Captcha.Interface
{
    public interface ICaptcha
    {
        ValueTask<CaptchaImage> Captcha(BaseCaptchaOptions options=null);
    }
}
