using System.Xml.Serialization;

namespace HuaLiangWindow.Common.MateralTools.MWeChat.Model
{
    /// <summary>
    /// 支付结果模型
    /// </summary>
    [XmlRoot]
    public sealed class PaymentResultModel
    {
        [XmlElement("return_code")]
        public string return_code { get; set; }

        [XmlElement("return_msg")]
        public string return_msg { get; set; }

        //公众账号ID appid   是 String(32)  wx8888888888888888 调用接口提交的公众账号ID
        [XmlElement("appid")]
        public string appid { get; set; }

        //商户号 mch_id  是 String(32)  1900000109	调用接口提交的商户号
        [XmlElement("mch_id")]
        public string mch_id { get; set; }

        //    设备号 device_info 否   String(32)  013467007045764	自定义参数，可以为请求支付的终端设备号等
        [XmlElement("device_info")]
        public string device_info { get; set; }

        //    随机字符串   nonce_str 是   String(32)  5K8264ILTKCH16CQ2502SI8ZNMTM67VS 微信返回的随机字符串
        [XmlElement("nonce_str")]
        public string nonce_str { get; set; }

        //签名 sign    是 String(32)  C380BEC2BFD727A4B6845133519F3AD6 微信返回的签名值，详见签名算法
        [XmlElement("sign")]
        public string sign { get; set; }

        //    业务结果    result_code 是   String(16)  SUCCESS SUCCESS/FAIL
        [XmlElement("result_code")]
        public string result_code { get; set; }

        //    错误代码    err_code 否   String(32)  SYSTEMERROR 详细参见下文错误列表
        [XmlElement("err_code")]
        public string err_code { get; set; }

        //错误代码描述 err_code_des    否 String(128) 系统错误 错误信息描述
        [XmlElement("err_code_des")]
        public string err_code_des { get; set; }

        //交易类型 trade_type  是 String(16)  JSAPI 交易类型，取值为：JSAPI，NATIVE，APP等，说明详见参数规定
        [XmlElement("trade_type")]
        public string trade_type { get; set; }

        //    预支付交易会话标识   prepay_id 是   String(64)  wx201410272009395522657a690389285100 微信生成的预支付会话标识，用于后续接口调用中使用，该值有效期为2小时
        [XmlElement("prepay_id")]
        public string prepay_id { get; set; }

        //    二维码链接   code_url 否   String(64)  URl：weixin：//wxpay/s/An4baqw	trade_type为NATIVE时有返回，用于生成二维码，展示给用户进行扫码支付
        [XmlElement("code_url")]
        public string code_url { get; set; }

    }
}
