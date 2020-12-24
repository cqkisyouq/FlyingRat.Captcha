using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Model;
using FlyingRat.Captcha.Validator;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;

namespace FlyingRat.Captcha.Builder
{
    public class CaptchaImageBuilder
    {
        private Lazy<CaptchaImage> Captcha;
        public CaptchaImageBuilder()
        {
            Captcha = new Lazy<CaptchaImage>();
        }
        public CaptchaImageBuilder AddBackground(Image image)
        {
            Captcha.Value.Backgorund = image;
            return this;
        }
        public CaptchaImageBuilder AddRandData(List<RandData> datas)
        {
            Captcha.Value.RandData = datas;
            return this;
        }
        public CaptchaImageBuilder AddGap(Image image)
        {
            Captcha.Value.Gap = image;
            return this;
        }
        public CaptchaImageBuilder AddGapBackground(Image image)
        {
            Captcha.Value.GapBackground = image;
            return this;
        }
        public CaptchaImageBuilder AddIndex(int[] index)
        {
            Captcha.Value.Index = index;
            return this;
        }
        public CaptchaImageBuilder AddChange(int[] change)
        {
            Captcha.Value.Change = change;
            return this;
        }
        public CaptchaImageBuilder AddColumn(int col)
        {
            Captcha.Value.Col = col;
            return this;
        }
        public CaptchaImageBuilder AddRow(int row)
        {
            Captcha.Value.Row = row;
            return this;
        }
        public CaptchaImageBuilder AddPoints(List<CaptchaPoint> points)
        {
            Captcha.Value.Points = points;
            return this;
        }
        public CaptchaImageBuilder AddTips(string tips)
        {
            Captcha.Value.Tips = tips;
            return this;
        }
        public CaptchaImageBuilder AddExtension(object value)
        {
            Captcha.Value.Extension=value;
            return this;
        }
        public CaptchaImageBuilder AddCode(string code)
        {
            Captcha.Value.Code = code;
            return this;
        }
        public CaptchaImageBuilder AddType(CaptchaType type)
        {
            Captcha.Value.Type = type;
            return this;
        }
        public CaptchaImageBuilder AddName(string name)
        {
            Captcha.Value.Name = name;
            return this;
        }

        public CaptchaImageBuilder AddOptions(BaseCaptchaOptions options)
        {
            Captcha.Value.Options = options.Clone() as BaseCaptchaOptions;
            return this;
        }
        public CaptchaImageBuilder AddToken(string token)
        {
            Captcha.Value.Token = token;
            return this;
        }
        public CaptchaImage Build()
        {
            var model = Captcha;
            Captcha = new Lazy<CaptchaImage>();
            return model.Value;
        }
    }
}
