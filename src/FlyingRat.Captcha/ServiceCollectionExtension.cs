using FlyingRat.Captcha.Builder;
using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Interface;
using FlyingRat.Captcha.Validator;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCaptcha(this IServiceCollection service)
        {
            service.AddOptions<SliderOptions>();
            service.AddOptions<PointOptions>();
            service.AddSingleton<ImageDriver>();
            service.AddSingleton<IImageProvider,ImageFileProvider>();

            service.AddScoped<ICaptcha, SliderCaptcha>();
            service.AddScoped<ICaptcha, PointCaptcha>();
            service.AddScoped<ICaptchaFactory, DefaultCaptchaFactory>();
            service.AddScoped<ICaptchaManager, CaptchaManager>();

            service.AddScoped<IValidator,SliderValidator>();
            service.AddScoped<IValidator, PointValidator>();
            service.AddScoped<IValidateManager, ValidatorManager>();

            service.AddTransient<CaptchaImageBuilder>();
            service.AddScoped<ICaptchaValidatorFactory, DefaultCaptchaValidatorFactory>();
            return service;
        }
    }
}
