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
        public static IServiceCollection AddCaptchaCore(this IServiceCollection service)
        {
            service.AddSingleton<ImageDriver>();
            service.AddSingleton<IImageProvider,ImageFileProvider>();

            service.AddCaptcha<SliderCaptcha>();
            service.AddCaptcha<PointCaptcha>();
            service.AddScoped<ICaptchaFactory, DefaultCaptchaFactory>();
            service.AddScoped<ICaptchaManager, CaptchaManager>();

            service.AddCaptchaValidator<SliderValidator>();
            service.AddCaptchaValidator<PointValidator>();

            service.AddTransient<CaptchaImageBuilder>();
            service.AddScoped<ICaptchaValidatorFactory, DefaultCaptchaValidatorFactory>();

            service.AddScoped<IValidateHandlerFactory, DefaultValidateHandlerFactory>();
            service.AddSingleton<ITokenGenerate, DefaultTokenGenerate>(); 
            return service;
        }

        public static IServiceCollection AddCaptcha<T>(this IServiceCollection service) where T : ICaptcha, new()
        {
            service.AddScoped(typeof(ICaptcha), typeof(T));
            return service;
        }
        public static IServiceCollection AddCaptchaValidator<T>(this IServiceCollection service) where T : IValidator, new()
        {
            service.AddScoped(typeof(IValidator), typeof(T));
            return service;
        }
        public static IServiceCollection AddCaptchaHandler<T>(this IServiceCollection service) where T:ICaptchaHandler,new ()
        {
            service.AddScoped(typeof(ICaptchaHandler), typeof(T));
            return service;
        }
    }
}
