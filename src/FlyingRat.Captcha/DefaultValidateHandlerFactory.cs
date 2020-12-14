using FlyingRat.Captcha.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingRat.Captcha
{
    public class DefaultValidateHandlerFactory : IValidateHandlerFactory
    {
        private readonly Dictionary<string, IEnumerable<ICaptchaHandler>> _validatorDic;
        private readonly IEnumerable<ICaptchaHandler> _handlers;
        public DefaultValidateHandlerFactory(IEnumerable<ICaptchaHandler> handlers)
        {
            _handlers = handlers;
            _validatorDic = _handlers.GroupBy(x=>x.Type).ToDictionary(x => x.Key,x=>x.AsEnumerable());
        }
        
        public IEnumerable<ICaptchaHandler> Create(string type)
        {
            _validatorDic.TryGetValue(type, out IEnumerable<ICaptchaHandler> captcha);
            return captcha;
        }

        IEnumerable<ICaptchaHandler> IValidateHandlerFactory.Create<T>()
        {
            var type = _handlers.Where(x => x.GetType() == typeof(T)).FirstOrDefault()?.Type;
            return Create(type);
        }
    }
}
