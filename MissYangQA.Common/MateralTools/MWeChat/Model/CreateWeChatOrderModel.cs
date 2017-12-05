using System;
using HuaLiangWindow.Common.MateralTools.MWeChat.Manager;

namespace HuaLiangWindow.Common.MateralTools.MWeChat.Model
{
    /// <summary>
    /// 创建微信订单模型
    /// </summary>
    public sealed class CreateWeChatOrderModel
    {
        ///公众账号ID appid 是 String(32) wxd678efh567hg6787 微信支付分配的公众账号ID（企业号corpid即为此appId） 
        [CreateModel(Require =true,NullOrEmptyMessage ="appid不可以为空")]
        public string appid { get; set; }

        ///商户号 mch_id 是 String(32) 1230000109 微信支付分配的商户号
        [CreateModel(Require = true, NullOrEmptyMessage = "mch_id不可以为空")]
        public string mch_id { get; set; }

        //随机字符串 nonce_str 是 String(32) 5K8264ILTKCH16CQ2502SI8ZNMTM67VS 随机字符串，长度要求在32位以内。推荐随机数生成算法
        [CreateModel(Require = true)]
        public string nonce_str { get; set; }

        //签名 sign 是 String(32) C380BEC2BFD727A4B6845133519F3AD6 通过签名算法计算得出的签名值，详见签名生成算法
        [CreateModel(Require = false, NullOrEmptyMessage = "sign不可以为空")]
        public string sign { get; set; }

        //签名类型 sign_type 否 String(32) MD5 签名类型，默认为MD5，支持HMAC-SHA256和MD5。 
        [CreateModel(Require = false)]
        public string sign_type { get; set; }

        //商品描述 body 是 String(128) 腾讯充值中心-QQ会员充值 商品简单描述，该字段请按照规范传递，具体请见参数规定
        [CreateModel(Require = true, NullOrEmptyMessage = "body不可以为空")]
        public string body { get; set; }


        //商户订单号 out_trade_no 是 String(32) 20150806125346 商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|*@ ，且在同一个商户号下唯一。详见商户订单号
        [CreateModel(Require = true, NullOrEmptyMessage = "out_trade_no不可以为空")]
        public string out_trade_no { get; set; }

        // 标价金额 total_fee 是 Int 88 订单总金额，单位为分，详见支付金额
        [CreateModel(Require = true, NullOrEmptyMessage = "tatal_fee不可以为空")]
        public int total_fee { get; set; }

        //终端IP spbill_create_ip 是 String(16) 123.12.12.123 APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP。
        [CreateModel(Require = true, NullOrEmptyMessage = "spbill_create_ip不可以为空")]
        public string spbill_create_ip { get; set; }

        //通知地址 notify_url 是 String(256) http://www.weixin.qq.com/wxpay/pay.php 异步接收微信支付结果通知的回调地址，通知url必须为外网可访问的url，不能携带参数。
        [CreateModel(Require = true, NullOrEmptyMessage = "notify_url不可以为空")]
        public string notify_url { get; set; }

        //交易类型 trade_type 是 String(16) JSAPI 取值如下：JSAPI，NATIVE，APP等，说明详见参数规定
        [CreateModel(Require = true, NullOrEmptyMessage = "trade_type不可以为空")]
        public string trade_type { get; set; }

        //指定支付方式 limit_pay 否 String(32) no_credit 上传此参数no_credit--可限制用户不能使用信用卡支付
        [CreateModel(Require = false)]
        public string limit_pay { get; set; }

        //用户标识 openid 否 String(128) oUpF8uMuAJO_M2pxb1Q9zNjWeS6o trade_type = JSAPI时（即公众号支付），此参数必传，此参数为微信用户在商户对应appid下的唯一标识。openid如何获取，可参考【获取openid】。企业号请使用【企业号OAuth2.0接口】获取企业号内成员userid，再调用【企业号userid转openid接口】进行转换
        [CreateModel(Require = true,NullOrEmptyMessage ="openId不可以为空")]
        public string openid { get; set; }

        //附加数据 attach 否 String(127) 深圳分店 附加数据，在查询API和支付通知中原样返回，可作为自定义参数使用。
        [CreateModel(Require = false)]
        public string attach { get; set; }
    }
}
