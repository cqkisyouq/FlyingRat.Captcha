using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats;

namespace FlyingRat.Captcha.Extensions
{
   public static class CaptchaImageExtension
    {
        /// <summary>
        /// html view model
        /// </summary>
        /// <param name="captcha"></param>
        /// <param name="imagePath">Get pictures using HTTP</param>
        /// <param name="format">Get pictures using Base64</param>
        /// <returns></returns>
        public static CaptchaViewModel ToViewModel(this CaptchaImage captcha,string validatePath,Action<CaptchaViewModel> imagePath=null,IImageFormat format=null)
        {
            if (captcha == null) return new CaptchaViewModel();
            var model = new CaptchaViewModel();
            model.Index = JsonSerializer.Serialize(captcha.Index);
            model.Change = JsonSerializer.Serialize(captcha.Change);
            model.Width = captcha.Backgorund.Width;
            model.Height = captcha.Backgorund.Height;
            model.Row = captcha.Row;
            model.Col = captcha.Col;
            model.X = captcha.Points.Count;
            model.validate = validatePath;
            model.Tips = captcha.Tips;
            model.tw = captcha.TipWidth;
            model.th = captcha.tipHeight;
            model.Name = captcha.Name;
            if (imagePath != null)
            {
                model.IsAction = true;
                imagePath(model);
            }
            else
            {
                model.IsAction = false;
                if (format == null) format = JpegFormat.Instance;
                model.Gap = captcha.Gap?.ToBase64String(PngFormat.Instance);
                model.BgGap = captcha.GapBackground?.ToBase64String(model.tw>0?PngFormat.Instance:format);
                model.Full = captcha.Backgorund?.ToBase64String(format);
            }

            if (model.X == 1)
            {
                model.Y = captcha.Points.First().Y;
            }
            model.Type = captcha.Type;
            model.Tk = captcha.Token;
            return model;
        }

        public static CaptchaCacheModel ToCacheModel(this CaptchaImage captcha)
        {
            if (captcha == null) return new CaptchaCacheModel();
            var model = new CaptchaCacheModel();
            model.Backgorund = captcha.Backgorund;
            model.Gap = captcha.Gap;
            model.GapBackground = captcha.GapBackground;
            model.Points = captcha.Points;
            model.Tips = captcha.Tips;
            model.Token = captcha.Token;
            model.Type = captcha.Type;
            return model;
        }
    }
}
