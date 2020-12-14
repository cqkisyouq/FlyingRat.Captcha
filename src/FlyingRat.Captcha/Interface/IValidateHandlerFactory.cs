using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Interface
{
    public interface IValidateHandlerFactory
    {
        IEnumerable<ICaptchaHandler> Create(string type);
        IEnumerable<ICaptchaHandler> Create<T>() where T : ICaptcha, new();
    }
}
