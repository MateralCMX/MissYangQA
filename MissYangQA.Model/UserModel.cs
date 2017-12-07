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
}
