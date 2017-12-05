namespace HuaLiangWindow.Common.MateralTools.MWeChat.Model
{
    /// <summary>
    /// JSAPI_Ticket结果模型
    /// </summary>
    public sealed class JsapiTicketResultModel
    {
        public int errcode { get; set; }

        public string errmsg { get; set; }

        public string ticket { get; set; }

        public int expires_in { get; set; }

    }
}
