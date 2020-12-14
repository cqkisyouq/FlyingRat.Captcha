using FlyingRat.Captcha.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FlyingRat.Captcha.Validator;
using FlyingRat.Captcha.Context;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Processing;
using FlyingRat.Captcha.Configuration;

namespace FlyingRat.Captcha
{
    public class CaptchaManager:ICaptchaManager
    {
        private readonly ICaptchaFactory _factory;
        private readonly ICaptchaValidatorFactory _validatorFactory;
        private readonly IValidateHandlerFactory _validateHandlerFactory;
        private readonly ITokenGenerate _token;
        private readonly IImageProvider _imageProvider;
        private readonly Random _rd = new Random(DateTime.Now.Millisecond);
        public CaptchaManager(
            ICaptchaValidatorFactory validatorFactory,
            IValidateHandlerFactory validateHandlerFactory,
            ITokenGenerate token,
            IImageProvider imageProvider,
            ICaptchaFactory captchaFactory
            )
        {
            _validatorFactory = validatorFactory;
            _validateHandlerFactory = validateHandlerFactory;
            _token = token;
            _factory = captchaFactory;
            _imageProvider = imageProvider;
        }

        public ValueTask<ValidateResult> Validate<T>(CaptchaValidateContext context, BaseCaptchaOptions options) where T : ICaptcha, new()
        {
            var type = typeof(T).Name;
            return Validate(type, context, options);
        }
        public ValueTask<ValidateResult> Validate(string captcha, CaptchaValidateContext context, BaseCaptchaOptions options)
        {
            var validator = _validatorFactory.Create(captcha);
            if (validator == null) return new ValueTask<ValidateResult>(ValidateResult.Failed);
            if (!validator.AllowValidate(context, options)) return new ValueTask<ValidateResult>(context.GetResult());

            var handlers = _validateHandlerFactory?.Create(captcha);
            handlers?.Handing(async (handler, context) =>await handler.Validating(context,options), context);
            if (!validator.AllowValidate(context)) return new ValueTask<ValidateResult>(context.GetResult());

            validator.Validate(context,options);
            var result = context.GetResult();
            if (result.Token == null && result.Succeed) result.Token = _token.Create();
            if (!result.Succeed) result.Token = null;
            handlers?.Reverse().Handing(async (handler, context) => await handler.Validated(context, options), context);
            return new ValueTask<ValidateResult>(result);
        }

        ValueTask<CaptchaImage> ICaptchaManager.Captcha<T>(BaseCaptchaOptions options)
        {
            var type = typeof(T).Name;
            return Captcha(type, options);
        }

        public async ValueTask<CaptchaImage> Captcha(string type, BaseCaptchaOptions options)
        {
            var captcha = _factory.Create(type);
            var handlers = _validateHandlerFactory?.Create(type);
            CaptchaContext context = null;
            if (handlers?.Any() ?? false)
            {
                context = new CaptchaContext(options: options);
            }

            handlers?.Handing(async (handler, context) => await handler.Creating(context), context);
            var rdImage =captcha?.Captcha(options);
            var model = await rdImage.Value;
            if (model.Token == null) model.Token = _token.Create();
            model.Options = options;
            if (handlers?.Any() ?? false)
            {
                context.Captcha = model;
            }
             handlers?.Reverse().Handing(async (handler, context) => await handler.Created(context), context);
            return model;
        }
    }
}
