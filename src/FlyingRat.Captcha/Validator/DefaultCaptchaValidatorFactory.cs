using FlyingRat.Captcha.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace FlyingRat.Captcha.Validator
{
    public class DefaultCaptchaValidatorFactory: ICaptchaValidatorFactory
    {
        private readonly Dictionary<string, IValidator> _validatorDic;
        private readonly IEnumerable<IValidator> _handlers;
        public DefaultCaptchaValidatorFactory(
            IEnumerable<IValidator> validators
            )
        {
            _handlers = validators;
            _validatorDic = _handlers.ToDictionary(x =>x.Type, x => x);
        }

        public IValidator Create(string type)
        {
            _validatorDic.TryGetValue(type, out IValidator validator);
            return validator;
        }

        public IValidator Create<T>() where T : ICaptcha, new()
        {
            var type = typeof(T).Name;
            return Create(type);
        }
    }
}
