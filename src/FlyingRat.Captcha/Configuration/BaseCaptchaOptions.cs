using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Configuration
{
    public class BaseCaptchaOptions:ICloneable
    {
        /// <summary>
        /// 乱序图片每分行割列数
        /// </summary>
        public virtual int Col { get; set; } = 1;
        /// <summary>
        /// 乱序图片分割行数
        /// </summary>
        public virtual int Row { get; set; } = 1;
        /// <summary>
        /// 验证偏移范围
        /// </summary>
        public virtual int Offset { get; set; } = 2;
        /// <summary>
        /// 提示
        /// </summary>
        public virtual string Tips { get; set; }
        /// <summary>
        /// 最大验证次数
        /// </summary>
        public virtual int Validate_Max { get; set; } = 3;
        public virtual SafelevelEnum Safelevel { get; set; } = SafelevelEnum.Middle;

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    public  enum SafelevelEnum
    {
        None,
        Low,
        Middle,
        Hight
    }
}
