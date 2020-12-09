using FlyingRat.Captcha.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlyingRat.Captcha
{
    public class DefaultCaptchaFactory : ICaptchaFactory
    {
        private readonly Dictionary<string, ICaptcha> _validatorDic;
        private readonly IEnumerable<ICaptcha> _handlers;
        public DefaultCaptchaFactory(IEnumerable<ICaptcha> captchas)
        {
            _handlers = captchas;
            _validatorDic = _handlers.ToDictionary(x => x.GetType().Name, x => x);
        }

         public ICaptcha Create(string type)
        {
            _validatorDic.TryGetValue(type, out ICaptcha captcha);
            return captcha;
        }

        ICaptcha ICaptchaFactory.Create<T>()
        {
            var type = typeof(T).Name;
            return Create(type);
        }
    }
}
