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
        private readonly IEnumerable<IValidateHandler> _handlers;
        private readonly IImageProvider _imageProvider;
        private readonly Random _rd = new Random(DateTime.Now.Millisecond);
        public CaptchaManager(
            ICaptchaValidatorFactory validatorFactory,
            IEnumerable<IValidateHandler> handlers,
            IImageProvider imageProvider,
            ICaptchaFactory captchaFactory
            )
        {
            _validatorFactory = validatorFactory;
            _handlers = handlers;
            _factory = captchaFactory;
            _imageProvider = imageProvider;
        }

        public ValueTask<ValidateResult> Validate<T>(CaptchaContext context, BaseCaptchaOptions options) where T : ICaptcha, new()
        {
            var type = typeof(T).Name;
            return Validate(type, context, options);
        }
        public ValueTask<ValidateResult> Validate(string captcha, CaptchaContext context, BaseCaptchaOptions options)
        {
            var validator = _validatorFactory.Create(captcha);
            if (validator == null) return new ValueTask<ValidateResult>(ValidateResult.Failed);
            _handlers?.Handing((handler, context) => handler.Validating(context,options), context);
            validator.Validate(context,options);
            _handlers?.Reverse().Handing((handler, context) => handler.Validated(context,options), context);
            var result = context.GetResult();
            if (result.Token == null && result.Succeed) result.Token = Guid.NewGuid().ToString("N");
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
            var rdImage =captcha?.Captcha(options);
            var model = await rdImage.Value;
            if (model.Token == null) model.Token = Guid.NewGuid().ToString("N");
            return model;
        }
    }
}
