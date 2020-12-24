using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Model;
using System.Threading.Tasks;

namespace FlyingRat.Captcha.Interface
{
    public interface ICaptcha
    {
        string Name { get; }
        CaptchaType Type { get; }
        ValueTask<CaptchaImage> Captcha(BaseCaptchaOptions options=null);
    }
}
