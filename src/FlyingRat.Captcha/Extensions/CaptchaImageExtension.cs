using FlyingRat.Captcha.Model;
using FlyingRat.Captcha.ViewModel;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.Text.Json;

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
        public static CaptchaViewModel ToViewModel(this CaptchaImage captcha,string validatePath,Action<CaptchaViewModel> imagePath=null,bool hasBackground=false,IImageFormat format=null)
        {
            if (captcha == null) return new CaptchaViewModel();
            var model = new CaptchaViewModel();
            model.Extension = captcha.Extension;
            model.Index = JsonSerializer.Serialize(captcha.Index);
            model.Change = JsonSerializer.Serialize(captcha.Change);
            model.Width = captcha.Backgorund.Width;
            model.Height = captcha.Backgorund.Height;
            model.Row = captcha.Row;
            model.Col = captcha.Col;
            model.X = captcha.Points.Count;
            model.validate = validatePath;
            model.Tips = captcha.Tips;
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
                model.BgGap = captcha.GapBackground?.ToBase64String(model.Height!=captcha.GapBackground.Height?PngFormat.Instance:format);
                if(hasBackground) model.Full = captcha.Backgorund?.ToBase64String(format);
            }

            model.Type = captcha.Type;
            model.Tk = captcha.Token;
            model.Name = captcha.Name;
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
            model.Token = captcha.Token;
            model.Type = captcha.Type;
            model.Name = captcha.Name;
            model.Options = captcha.Options;
            return model;
        }
    }
}
