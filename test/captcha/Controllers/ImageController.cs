using FlyingRat.Captcha;
using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Context;
using FlyingRat.Captcha.Extensions;
using FlyingRat.Captcha.Interface;
using FlyingRat.Captcha.Validator;
using FlyingRat.Captcha.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Captcha.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageProvider _imageProvider;
        private readonly ImageDriver _imageHandler;
        private readonly IMemoryCache _memoryCache;
        private readonly ICaptchaManager _captchaManager;
        private static readonly string cacheKey = "image_catpture";
        public ImageController(ImageDriver handler, 
            IImageProvider imageFileProvider, 
            ICaptchaManager captchaManager,
            IMemoryCache cache)
        {
            _imageHandler = handler;
            _imageProvider = imageFileProvider;
            _memoryCache = cache;
            _captchaManager = captchaManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async ValueTask<IActionResult> RandImage(string type= "SliderCatpcha", int? row = 2, int? col = 13)
        {
            var captcha =await _captchaManager.Captcha(type, new BaseCaptchaOptions() { Col=col.Value,Row=row.Value,Validate_Max=2});
            var model = captcha.ToViewModel(Url.Action("Validate"),hasBackground:true);
            //model.BgGap = Url.Action("SliderBackground");
            //model.Full = Url.Action("Background");
            //model.Gap = Url.Action("Slider");

            _memoryCache.Set(cacheKey + model.Tk, captcha.ToCacheModel(), DateTimeOffset.Now.AddMinutes(5));

            return Json(model);
        }
        public IActionResult Background(string tk)
        {
            var data = _memoryCache.Get<CaptchaCacheModel>(cacheKey + tk);
            if (data == null) return File(new byte[1], "image/jpeg");
            using MemoryStream ms = new MemoryStream();
            data.Backgorund.SaveAsJpeg(ms);
            return File(ms.ToArray(), "image/jpeg");
        }
        public IActionResult SliderBackground(string tk)
        {
            var data = _memoryCache.Get<CaptchaCacheModel>(cacheKey + tk);
            if (data == null) return File(new byte[1], "image/jpeg");
            using MemoryStream ms = new MemoryStream();
            data.GapBackground.SaveAsJpeg(ms);
            return File(ms.ToArray(), "image/jpeg");
        }
        public IActionResult Slider(string tk)
        {
            var data = _memoryCache.Get<CaptchaCacheModel>(cacheKey + tk);
            if (data == null) return File(new byte[1], "image/png");
            using MemoryStream ms = new MemoryStream();
            data.Gap.SaveAsPng(ms);
            return File(ms.ToArray(), "image/png");
        }

        public async ValueTask<IActionResult> Validate([FromBody]CaptchaVerifyModel model)
        {
            var data = _memoryCache.Get<CaptchaCacheModel>(cacheKey + model.TK);
            if (data == null) return Json(ValidateResult.Failed);
            var context = new CaptchaValidateContext(new ValidateModel(data.Points), model.ToValidateModel(),data.Validate);
            data.Validate =await _captchaManager.Validate(data.Name, context,data.Options);
            return Json(data.Validate?.ToViewModel());
        }
    }
}
