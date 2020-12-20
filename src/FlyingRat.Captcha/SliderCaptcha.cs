using FlyingRat.Captcha.Builder;
using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Interface;
using FlyingRat.Captcha.Validator;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlyingRat.Captcha
{
    public class SliderCaptcha : BaseCaptcha
    {
        private readonly ImageDriver _driver;
        private readonly IImageProvider _imageProvider;
        private readonly SliderOptions _options;
        private readonly CaptchaImageBuilder _captchaImageBuilder;
        public SliderCaptcha(ImageDriver driver,
            IOptions<SliderOptions> options,
            CaptchaImageBuilder builder,
            IImageProvider imageProvider)
        {
            _driver = driver;
            _imageProvider = imageProvider;
            _options = options.Value;
            _captchaImageBuilder = builder;
        }
        public SliderCaptcha() { }
        public override CaptchaType Type => CaptchaType.Slider;
        public override async ValueTask<CaptchaImage> CaptchaCreate(BaseCaptchaOptions options)
        {
            var path = _imageProvider.SliderRoot;
            using Image<Rgba32> image = await _imageProvider.LoadBackground();
            using var alpha = await _imageProvider.LoadAlpha(path);
            using var border = await _imageProvider.LoadBorder(path);
            using var slider = await _imageProvider.LoadSlider(path);
            var fullImage = image.Clone();
            var option = options ?? _options;
            int col = option.Col;
            int row = option.Row;
            var point = RandPoint(image, alpha);
            _driver.CopyNoAlpha(image, new Point(point.X, point.Y), alpha);

            alpha.Mutate(x =>
            {
                x.DrawImage(border, 1);
            });

            image.Mutate(x =>
            {
                x.DrawImage(slider,new Point(point.X,point.Y), 1);
            });

            var randData = _driver.RandImagesBy(image, ref col, ref row);
            _captchaImageBuilder.AddBackground(fullImage)
                .AddGap(alpha.Clone())
                .AddRandData(randData)
                .AddGapBackground(_driver.ImageByRandImages(image, randData, col))
                .AddIndex(randData.Select(x => x.Index).ToArray())
                .AddChange(randData.Where(x => x.Change).Select(x => x.Index).ToArray())
                .AddColumn(col).AddRow(row)
                .AddType(Type).AddName(Name)
                .AddTips("向右拖动滑块填充拼图")
                .AddPoints(new List<CaptchaPoint>(1) { point })
                .AddOptions(option);
            return _captchaImageBuilder.Build();
        }
    }
}
