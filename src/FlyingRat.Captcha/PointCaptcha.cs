using FlyingRat.Captcha.Builder;
using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Interface;
using FlyingRat.Captcha.Model;
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
        public override CaptchaType Type => CaptchaType.Point;
        public override async ValueTask<CaptchaImage> CaptchaCreate(BaseCaptchaOptions options = null)
        {
            var path = _imageProvider.SliderRoot;
            using Image<Rgba32> image = await _imageProvider.LoadBackground();
            var fullImage = image.Clone();
            var option = options ?? _options;
            int col = option.Col;
            int row = option.Row;
            var lumps = await _imageProvider.LoadRandImages(_imageProvider.LumpRoot);
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
            var randImage = _driver.ImageByRandImages(image, randData, col);
            var xpos = 0;
            var ypos = randImage.Height+2;
            var height = lumps.Max(x => x.Height)+2;

            var pointRandImage = new Image<Rgba32>(randImage.Width, randImage.Height + height);
            _driver.Copy(randImage.CloneAs<Rgba32>(), Point.Empty, pointRandImage);
            pointRandImage.Mutate(x =>
            {
                for (int i = 0; i < lumps.Count; i++)
                {
                    var lump = lumps[i];
                    lump.Mutate(x => x.Invert());
                    x.DrawImage(lump, new Point(xpos, ypos), 1);
                    xpos += lump.Width + 5;
                }
            });
            _captchaImageBuilder.AddBackground(fullImage)
                .AddRandData(randData)
                .AddGapBackground(pointRandImage)
                .AddIndex(randData.Select(x => x.Index).ToArray())
                .AddChange(randData.Where(x => x.Change).Select(x => x.Index).ToArray())
                .AddColumn(col).AddRow(row)
                .AddType(Type).AddName(Name)
                .AddTips("请依次点击")
                .AddExtension(new PointExtension(height,xpos))
                .AddPoints(points).AddOptions(option);
            return _captchaImageBuilder.Build();
        }
    }
}
