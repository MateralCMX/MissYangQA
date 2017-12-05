namespace MateralTools.MWeChat
{

    /// <summary>
    /// 微信Token模型
    /// </summary>
    public class WeChatTokenModel
    {
        /// <summary>
        /// Token
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 有效时间
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string errmsg { get; set; }
    }
}
