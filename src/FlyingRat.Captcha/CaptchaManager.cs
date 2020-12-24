using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Context;
using FlyingRat.Captcha.Interface;
using FlyingRat.Captcha.Model;
using FlyingRat.Captcha.Validator;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FlyingRat.Captcha
{
    public class CaptchaManager:ICaptchaManager
    {
        private readonly ICaptchaFactory _factory;
        private readonly ICaptchaValidatorFactory _validatorFactory;
        private readonly IValidateHandlerFactory _validateHandlerFactory;
        private readonly ITokenGenerate _token;
        public CaptchaManager(
            ICaptchaValidatorFactory validatorFactory,
            IValidateHandlerFactory validateHandlerFactory,
            ITokenGenerate token,
            ICaptchaFactory captchaFactory
            )
        {
            _validatorFactory = validatorFactory;
            _validateHandlerFactory = validateHandlerFactory;
            _token = token;
            _factory = captchaFactory;
        }

        public ValueTask<ValidateResult> Validate<T>(CaptchaValidateContext context, BaseCaptchaOptions options) where T : ICaptcha, new()
        {
            var type = typeof(T).Name;
            return Validate(type, context, options);
        }
        public async ValueTask<ValidateResult> Validate(string captcha, CaptchaValidateContext context, BaseCaptchaOptions options)
        {
            var validator = _validatorFactory.Create(captcha);
            if (validator == null) return ValidateResult.Failed;
            if (!validator.AllowValidate(context, options)) return context.GetResult().NotAllow();

            var handlers = _validateHandlerFactory?.Create(captcha);
            handlers?.Handing(async (handler, context) =>await handler.Validating(context,options), context);
            if (!validator.AllowValidate(context)) return context.GetResult().NotAllow();

            await validator.Validate(context,options);
            var result = context.GetResult();
            if (result.Token == null && result.Succeed) result.Token = _token.Create();
            if (!result.Succeed) result.Token = null;
            handlers?.Reverse().Handing(async (handler, context) => await handler.Validated(context, options), context);
            return result;
        }

        ValueTask<CaptchaImage> ICaptchaManager.Captcha<T>(BaseCaptchaOptions options)
        {
            var type = typeof(T).Name;
            return Captcha(type, options);
        }

        public async ValueTask<CaptchaImage> Captcha(string type, BaseCaptchaOptions options)
        {
            var captcha = _factory.Create(type);
            if (captcha == null) return default;
            var handlers = _validateHandlerFactory?.Create(type);
            CaptchaContext context = null;
            if (handlers?.Any() ?? false)
            {
                context = new CaptchaContext(options: options);
            }

            handlers?.Handing(async (handler, context) => await handler.Creating(context), context);
            var model = await captcha.Captcha(options);
            if (model.Token == null) model.Token = _token.Create();
            if(model.Options==null) model.Options = options?.Clone() as BaseCaptchaOptions;
            if (handlers?.Any() ?? false)
            {
                context.Captcha = model;
            }
             handlers?.Reverse().Handing(async (handler, context) => await handler.Created(context), context);
            return model;
        }
    }
}
