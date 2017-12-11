using MateralTools.MResult;
using System;

namespace MissYangQA.Model
{
    /// <summary>
    /// 删除模型
    /// </summary>
    public class DeleteModel : IVerificationLoginModel
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 登录用户ID
        /// </summary>
        public Guid LoginUserID { get; set; }
        /// <summary>
        /// 登录用户令牌
        /// </summary>
        public string LoginUserToken { get; set; }
    }
    /// <summary>
    /// 登录验证模型接口
    /// </summary>
    public interface IVerificationLoginModel
    {
        /// <summary>
        /// 登录用户ID
        /// </summary>
        Guid LoginUserID { get; set; }
        /// <summary>
        /// 登录用户令牌
        /// </summary>
        string LoginUserToken { get; set; }
    }
}
