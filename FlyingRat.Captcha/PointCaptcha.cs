using FlyingRat.Captcha.Builder;
using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Interface;
using FlyingRat.Captcha.Validator;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingRat.Captcha
{
    public class PointCaptcha : BaseCaptcha
    {
        private readonly ImageDriver _driver;
        private readonly IImageProvider _imageProvider;
        private readonly PointOptions _options;
        private readonly CaptchaImageBuilder _captchaImageBuilder;
        public PointCaptcha(
            ImageDriver driver,
            IOptions<PointOptions> options,
            IImageProvider imageProvider,
            CaptchaImageBuilder builder
            )
        {
            _driver = driver;
            _imageProvider = imageProvider;
            _options = options.Value;
            _captchaImageBuilder = builder;
        }
        public PointCaptcha()
        {

        }
        public override async ValueTask<CaptchaImage> CaptchaCreate(BaseCaptchaOptions options = null)
        {
            var path = _imageProvider.SliderRoot;
            using Image<Rgba32> image = await _imageProvider.LoadBackground();
            var fullImage = image.Clone();
            int col = options?.Col ?? _options.Col;
            int row = options?.Row ?? _options.Row;
            var lumps = await _imageProvider.LoadImages(_imageProvider.LumpRoot);
            var points = new List<CaptchaPoint>(lumps.Count);
            var rdPoints = RandPoints(image, lumps.ToArray());
            image.Mutate(x =>
            {
                foreach (var lump in lumps)
                {
                    var point = RandPoint(rdPoints);
                    if (point != null)
                    {
                        x.DrawImage(lump, new Point(point.X, point.Y), 1);
                        points.Add(point);
                    }
                }
            });
            rdPoints.Clear();
            var randData = _driver.RandImagesBy(image, ref col, ref row);
            _captchaImageBuilder.AddBackground(fullImage)
                .AddRandData(randData)
                .AddGapBackground(_driver.ImageByRandImages(image, randData, col))
                .AddIndex(randData.Select(x => x.Index).ToArray())
                .AddChange(randData.Where(x => x.Change).Select(x => x.Index).ToArray())
                .AddColumn(col).AddRow(row)
                .AddType(CaptchaType.Point)
                .AddPoints(points);
            return _captchaImageBuilder.Build();
        }
    }
}
