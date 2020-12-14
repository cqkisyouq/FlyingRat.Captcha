using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.ViewModel
{
    public class ValidateViewModel
    {
        public string Token { get; set; }
        /// <summary>
        /// 验证通过
        /// </summary>
        public bool Succeed { get; set; }
        /// <summary>
        /// 需要刷新验证码
        /// </summary>
        public bool Refresh { get; set; }
    }
}
