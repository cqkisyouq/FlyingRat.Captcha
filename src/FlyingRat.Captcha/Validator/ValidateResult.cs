using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Validator
{
    public class ValidateResult
    {
        /// <summary>
        /// 验证了
        /// </summary>
        public bool IsValidate { get; set; }
        /// <summary>
        /// 验证通过
        /// </summary>
        public bool Succeed { get; set; }
        /// <summary>
        /// 允许验证
        /// </summary>
        public bool AllowValidate { get; set; } = true;
        /// <summary>
        /// 验证次数
        /// </summary>
        public int Count { get; set; }
        public string Token { get; set; }
        public ValidateResult() { }
        public ValidateResult(bool succeed)
        {
            this.Succeed = succeed;
        }

        public static ValidateResult Failed
        {
            get { return new ValidateResult(); }
        }
        public static ValidateResult Success
        {
            get { return new ValidateResult(true); }
        }
    }

    
}
