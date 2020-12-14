using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Interface
{
    public interface ICaptchaFactory
    {
        ICaptcha Create(string type);
        ICaptcha Create<T>() where T:ICaptcha,new();
    }
}
