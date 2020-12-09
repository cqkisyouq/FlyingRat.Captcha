using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Interface
{
    public interface ICaptchaValidatorFactory
    {
        IValidator Create(string type);
        IValidator Create<T>() where T : ICaptcha, new();
    }
}
