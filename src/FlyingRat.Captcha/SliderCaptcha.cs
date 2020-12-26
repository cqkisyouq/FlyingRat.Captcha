using FlyingRat.Captcha.Builder;
using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Interface;
using FlyingRat.Captcha.Model;
using FlyingRat.Captcha.Validator;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
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
            int rotate =rd.Next(0,2);
            var point = RandPoint(image, alpha);
            _driver.CopyNoAlpha(image, new Point(point.X, point.Y), alpha);
            alpha.Mutate(x =>
            {
                x.DrawImage(border, 1);
                if(rotate>0)
                {
                    rotate = rd.Next(0, 2);
                    x.RotateFlip(RotateMode.Rotate180, rotate==1?FlipMode.Horizontal:FlipMode.Vertical);
                    rotate = rotate == 0 ? 180 : -180;
                }
            });
            image.Mutate(x =>
            {
                x.DrawImage(slider, new Point(point.X, point.Y), 1);
                if (option?.Safelevel != SafelevelEnum.None)
                {
                    var level = (int)option.Safelevel + 1;
                    for (int i = 0; i < level; i++)
                    {
                        using var safe = slider.Clone();
                        var rePoint = RandPoint(image, slider);
                        if ((i & 1) == 1) rePoint.Y = point.Y;
                        var xpos = Math.Abs(point.X - rePoint.X);
                        var ypost = Math.Abs(point.Y - rePoint.Y);
                        if (xpos <= (slider.Width >> 1))
                        {
                            xpos = rd.Next(slider.Width-xpos, slider.Width);
                            rePoint.X += (i & 1) == 1 ? -xpos : xpos;
                        }
                        if (ypost <= (slider.Height >> 1))
                        {
                            ypost = rd.Next(0, slider.Height>>1);
                            rePoint.Y += (i & 1) == 1 ? ypost : -ypost;
                        }
                        safe.Mutate(x => x.Rotate(rd.Next(30, 60)));
                        x.DrawImage(safe, rePoint.Point, 0.7f);
                    }
                }
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
                .AddExtension(new SliderExtension(point.Y, rotate))
                .AddOptions(option);
            return _captchaImageBuilder.Build();
        }
    }
}
