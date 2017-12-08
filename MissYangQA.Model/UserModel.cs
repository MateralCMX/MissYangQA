using System;

namespace MissYangQA.Model
{
    /// <summary>
    /// 登录模型
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 是否已经加密
        /// </summary>
        private bool _isEncrypted = false;
        /// <summary>
        /// 是否已经加密
        /// </summary>
        public bool IsEncrypted { get => _isEncrypted; set => _isEncrypted = value; }
    }
    /// <summary>
    /// 修改用户模型
    /// </summary>
    public class EditUserModel : T_User, IVerificationLoginModel
    {
        /// <summary>
        /// 登录用户唯一标识
        /// </summary>
        public Guid LoginUserID { get; set; }
        /// <summary>
        /// 登录用户Token
        /// </summary>
        public string LoginUserToken { get; set; }
    }
}
