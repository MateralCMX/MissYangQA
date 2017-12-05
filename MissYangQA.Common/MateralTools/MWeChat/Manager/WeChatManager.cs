using MateralTools.MCache;
using MateralTools.MHttpWeb;
using System;
using System.Configuration;
using System.Text;
using HuaLiangWindow.Common.MateralTools.MWeChat.Model;
using System.Web.Script.Serialization;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Xml;

namespace MateralTools.MWeChat
{
    public sealed class WeChatManager
    {
        /// <summary>
        /// APPID
        /// </summary>
        public string Appid => ConfigurationManager.AppSettings["OfficialAccountsAppId"];
        /// <summary>
        /// APPSecret
        /// </summary>
        public string AppSecret => ConfigurationManager.AppSettings["OfficialAccountsAppSecret"];
        /// <summary>
        /// 支付密钥
        /// </summary>
        public string PaymentKey => ConfigurationManager.AppSettings["PaymentKey"];
        /// <summary>
        /// 商户ID
        /// </summary>
        public string MerchantId => ConfigurationManager.AppSettings["MerchantId"];

        /// <summary>
        /// 基础Token
        /// </summary>
        private const string OfficialAccountsTokenKey = "OfficialAccountsToken";

        /// <summary>
        /// 认证Token
        /// </summary>
        private const string OfficialAccountsOAuthAccessToken = "OAuthAccessToken";

        /// <summary>
        /// 刷新Token
        /// </summary>
        private const string OfficialAccountOAuthRefreshToken = "OAuthRefreshToken";

        /// <summary>
        /// jsapi_ticket
        /// </summary>
        private const string JsapiTicket = "JsapiTicket";

        /// <summary>
        /// 用户认证
        /// </summary>
        /// <param name="code">票据</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">无效的code</exception>
        public AuthorizationResult UserAuthorization(string code)
        {
            var url =
                $"https://api.weixin.qq.com/sns/oauth2/access_token?appid={Appid}&secret={AppSecret}&code={code}&grant_type=authorization_code";
            var result = HttpWebManager.SendRequest(url, "", MethodType.Get, ParamType.Text, Encoding.UTF8);
            var authorizationResult = new JavaScriptSerializer().Deserialize<AuthorizationResult>(result);
            if (!string.IsNullOrEmpty(authorizationResult.errcode))
                throw new ArgumentException(authorizationResult.errcode + authorizationResult.errmsg);

            SetOAuthAccessToken(authorizationResult.access_token);
            WebCacheManager.Set(OfficialAccountOAuthRefreshToken, authorizationResult.refresh_token,
                30 * 24 * 60);
            return authorizationResult;
        }

        /// <summary>
        /// 刷新AccessToken
        /// </summary>
        /// <returns></returns>
        public string RefreshAccessToken()
        {
            var refreshToken = GetOAuthRefressToken();
            var url =
                $"https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={Appid}&grant_type=refresh_token&refresh_token={refreshToken}";
            var result = HttpWebManager.SendRequest(url, "", MethodType.Get, ParamType.Text, Encoding.UTF8);

            var authorizationResult = new JavaScriptSerializer().Deserialize<AuthorizationResult>(result);
            return authorizationResult.access_token;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public WeChatUserInfoResult GetWeChatUserInfo(string openId)
        {
            ValidateAccessToken(openId);
            var accessToken = GetOAuthAccessToken();

            var url =
                $"https://api.weixin.qq.com/sns/userinfo?access_token={accessToken}&openid={openId}&lang=zh_CN";
            var result = HttpWebManager.SendRequest(url, "", MethodType.Get, ParamType.Text, Encoding.UTF8);
            var authorizationResult = new JavaScriptSerializer().Deserialize<WeChatUserInfoResult>(result);
            if (!string.IsNullOrEmpty(authorizationResult.errcode))
            {
                throw new ArgumentException(authorizationResult.errcode + authorizationResult.errmsg);
            }
            return authorizationResult;
        }

        /// <summary>
        /// 验证AccessToken有效性
        /// </summary>
        /// <param name="openId"></param>
        /// <exception cref="ArgumentException"></exception>
        public void ValidateAccessToken(string openId)
        {
            var accessToken = GetOAuthAccessToken();
            var url =
                $"https://api.weixin.qq.com/sns/userinfo?access_token={accessToken}&openid={openId}&lang=zh_CN";
            var result = HttpWebManager.SendRequest(url, "", MethodType.Get, ParamType.Text, Encoding.UTF8);
            var validateAccessTokenResult = new JavaScriptSerializer().Deserialize<ValidateAccessTokenResult>(result);

            if (!string.IsNullOrEmpty(validateAccessTokenResult.errcode))
                throw new ArgumentException(validateAccessTokenResult.errcode + validateAccessTokenResult.errmsg);
        }

        /// <summary>
        /// 计算微信签名
        /// </summary>
        /// <returns>noncestr,timestamp,signature</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Tuple<string, string, string, string> CalculateWechatSign(string url)
        {
            var noncestr = Guid.NewGuid().ToString("N");
            var timestamp = CalculateUnixTimeStamp();
            var ticket = GetTicket();
            var hashtable = new Hashtable
            {
                {"noncestr", noncestr},
                {"jsapi_ticket",ticket},
                {"timestamp", timestamp},
                {"url", url}
            };

            var str = GetStringOrderByASCII(hashtable);
            var signature = GetSignToSha1(str);
            return new Tuple<string, string, string, string>(noncestr, timestamp, signature, ticket);
        }

        /// <summary>
        /// 从微信服务器下载图片
        /// </summary>
        /// <returns></returns>
        public string DownloadImgFromWeChatServer(string mediaId)
        {
            var token = GetOfficialAccountsToken().access_token;
            var fileName = $"{DateTime.Now:yyyyMMddHHmmssffff}.jpg";

            var filePath = HttpContext.Current.Server.MapPath($"/UploadFiles/Images/{fileName}");
            //从微信服务器下载图片
            var url = $"http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={token}&media_id={mediaId}";
            var webClient = new WebClient();
            var result = webClient.DownloadData(new Uri(url));
            var ms = new MemoryStream(result);
            var img = Image.FromStream(ms);

            img.Save(filePath);
            ms.Dispose();
            img.Dispose();

            return fileName;
        }

        /// <summary>
        /// 生成订单
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public Tuple<string, string, string, string, string,string> GenerateOrder(CreateWeChatOrderModel orderModel)
        {
            orderModel.appid = Appid;
            orderModel.mch_id = MerchantId;
            orderModel.spbill_create_ip = GetHostAddress();
            orderModel.nonce_str = Guid.NewGuid().ToString("N");

            var hashTable = GetPaymentModelHashTable(orderModel);

            var stringA = GetStringOrderByASCII(hashTable);

            var key = PaymentKey;
            var stringSignTemp = stringA + "&key=" + key; //注：key为商户平台设置的密钥key
            var sign = MakeSign(stringSignTemp).ToUpper(); //注：MD5签名方式
            orderModel.sign = sign;

            var xmlPost = GetXmlDocument(orderModel);

            const string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            var result = HttpWebManager.SendRequest(url, xmlPost.InnerXml, MethodType.Post, ParamType.Text, Encoding.UTF8);

            var doc = new XmlDocument(); doc.LoadXml(result);
            var rootElement = doc.DocumentElement;
            if (rootElement == null) throw new InvalidOperationException("无效的操作，返回结果异常");

            var returnCode = rootElement.SelectSingleNode("return_code")?.InnerText;
            var returnMsg = rootElement.SelectSingleNode("return_msg")?.InnerText;

            if (returnCode != "SUCCESS" || returnMsg != "OK") throw new ArgumentException(returnMsg);

            var prepayId = rootElement.SelectSingleNode("prepay_id")?.InnerText;
            var paymentSigns = CalculateWechatPaymentSigns(prepayId);
            return paymentSigns;
        }


        #region Private Methods

        /// <summary>
        /// 计算微信支付签名
        /// </summary>
        /// <param name="prepayId"></param>
        /// <returns></returns>
        private Tuple<string, string, string, string, string,string> CalculateWechatPaymentSigns(string prepayId)
        {
            var timeStamp = CalculateUnixTimeStamp();
            var nonceStr = Guid.NewGuid().ToString("N");
            var package = "prepay_id=" + prepayId;
            var signType = "MD5";
            var hashTable = new Hashtable
            {
                {"appId", Appid},
                {"timeStamp", timeStamp},
                {"nonceStr", nonceStr},
                {"package", package},
                {"signType", signType}
            };

            var url = GetStringOrderByASCII(hashTable);
            url += "&key=" + PaymentKey;
            var sign = MakeSign(url).ToUpper();
            return new Tuple<string, string, string, string, string,string>(timeStamp, nonceStr, package, signType, sign,url);
        }


        /// <summary>
        /// 获取客户端IP地址（无视代理）
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        private static string GetHostAddress()
        {
            var userHostAddress = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }


        /// <summary>
        /// @生成签名，详见签名生成算法
        ///return 签名, sign字段不参加签名
        /// </summary>
        /// <returns></returns>
        public string MakeSign(string url)
        {
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(url));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }


        /// <summary>
        /// 按ASCII升序组装URL
        /// </summary>
        /// <param name="hashtable"></param>
        /// <returns></returns>
        private string GetStringOrderByASCII(Hashtable hashtable)
        {
            var keys = new ArrayList(hashtable.Keys);
            keys.Sort();
            var str = string.Empty;
            foreach (string key in keys)
            {
                var value = $"{hashtable[key]}";
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    str += $"{key}={value}&";
                }
            }
            return str.TrimEnd('&');
        }

        /// <summary>
        /// 生成支付签名
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        private static Hashtable GetPaymentModelHashTable(CreateWeChatOrderModel orderModel)
        {
            var properties = orderModel.GetType().GetProperties();
            var hashTable = new Hashtable();

            foreach (var property in properties)
            {
                var customerAttributes = property.GetCustomAttributesData();
                var customerAttr = customerAttributes.FirstOrDefault();

                var propertyValue = property.GetValue(orderModel);
                var propertyType = property.PropertyType.FullName;

                if (propertyType == typeof(int).FullName)
                {
                    if ((int)propertyValue > 0)
                    {
                        hashTable.Add(property.Name, propertyValue);
                        continue;
                    }
                }
                if (propertyType == typeof(string).FullName)
                {
                    if (!string.IsNullOrEmpty((string)propertyValue))
                    {
                        hashTable.Add(property.Name, propertyValue);
                        continue;
                    }
                }
                if (propertyType == typeof(Guid).FullName)
                {
                    if (propertyValue != null && (Guid)propertyValue != Guid.Empty)
                    {
                        hashTable.Add(property.Name, propertyValue);
                        continue;
                    }
                }

                if (customerAttr?.NamedArguments == null) continue;

                var requiredType = customerAttr.NamedArguments.FirstOrDefault(q => q.MemberName.Equals("Require"));

                if (requiredType.MemberInfo == null) continue;

                if (!(bool)requiredType.TypedValue.Value) continue;

                var nullOrEmptyType = customerAttr.NamedArguments.FirstOrDefault(q => q.MemberName.Equals("NullOrEmptyMessage"));
                if (nullOrEmptyType.MemberInfo != null)
                    throw new ArgumentException((string)nullOrEmptyType.TypedValue.Value);
            }
            return hashTable;
        }

        /// <summary>
        /// 生成XML
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        private XmlDocument GetXmlDocument(CreateWeChatOrderModel orderModel)
        {
            var properties = orderModel.GetType().GetProperties();
            var xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateElement("xml"));

            foreach (var property in properties)
            {

                var propertyValue = property.GetValue(orderModel);

                var element = xmlDocument.CreateElement(property.Name);
                if (propertyValue == null) continue;

                element.InnerText = propertyValue.ToString();
                xmlDocument.DocumentElement.AppendChild(element);
            }
            return xmlDocument;
        }



        /// <summary>
        /// 获取SHA1签名
        /// </summary>
        /// <param name="encypStr">要签名的字符串</param>
        /// <returns></returns>
        private static string GetSignToSha1(string encypStr)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            var enc = new ASCIIEncoding();
            var dataToHash = enc.GetBytes(encypStr);
            var dataHashed = sha.ComputeHash(dataToHash);
            var hash = BitConverter.ToString(dataHashed).Replace("-", "");
            return hash.ToLower();
        }

        /// <summary>
        /// 计算Unix时间戳
        /// </summary>
        /// <returns></returns>
        private string CalculateUnixTimeStamp()
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 获取jsapi_Ticket
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private string GetTicket()
        {
            var jsapiTicket = WebCacheManager.Get<string>(JsapiTicket);
            if (jsapiTicket != null) return jsapiTicket;

            var token = GetOfficialAccountsToken().access_token;
            var url =
                $"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={token}&type=jsapi";
            var resStr = HttpWebManager.SendRequest(url, "", MethodType.Get, ParamType.Text, Encoding.UTF8);
            var ticketResultModel = new JavaScriptSerializer().Deserialize<JsapiTicketResultModel>(resStr);
            if (ticketResultModel.errcode != 0)
                throw new ArgumentException("无法获得jsapi_ticket，" + ticketResultModel.errcode + "  " +
                                            ticketResultModel.errmsg + "   " + token);

            WebCacheManager.Set(JsapiTicket, ticketResultModel.ticket, 110);
            return ticketResultModel.ticket;
        }


        /// <summary>
        /// 获取用户认证授权AccessToken
        /// </summary>
        /// <returns></returns>
        private string GetOAuthAccessToken()
        {
            var accessToken = WebCacheManager.Get<string>(OfficialAccountsOAuthAccessToken);
            if (accessToken != null) return accessToken;

            accessToken = RefreshAccessToken();
            SetOAuthAccessToken(accessToken);
            return accessToken;
        }

        /// <summary>
        /// 缓存认证AccessToken
        /// </summary>
        /// <param name="accessToken"></param>
        private static void SetOAuthAccessToken(string accessToken)
        {
            WebCacheManager.Set(OfficialAccountsOAuthAccessToken, accessToken,
                120);
        }

        /// <summary>
        /// 获取用户真正授权RefreshToken
        /// </summary>
        /// <returns></returns>
        private static string GetOAuthRefressToken()
        {
            var refreshToken = WebCacheManager.Get<string>(OfficialAccountOAuthRefreshToken);
            if (refreshToken == null)
            {
                throw new InvalidOperationException("无效的操作，请退出页面重新操作");
            }
            return refreshToken;
        }

        /// <summary>
        /// 获取微信Token
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private WeChatTokenModel GetOfficialAccountsToken()
        {
            var token = WebCacheManager.Get<WeChatTokenModel>(OfficialAccountsTokenKey);
            if (token != null) return token;

            var url =
                $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={Appid}&secret={AppSecret}";
            var resStr = HttpWebManager.SendRequest(url, "", MethodType.Get, ParamType.Text, Encoding.UTF8);
            var weChatTokenModel = new JavaScriptSerializer().Deserialize<WeChatTokenModel>(resStr);
            if (weChatTokenModel.errcode > 0)
                throw new InvalidOperationException(weChatTokenModel.errcode + "   " + weChatTokenModel.errmsg);

            WebCacheManager.Set(OfficialAccountsTokenKey, weChatTokenModel, 110);
            return weChatTokenModel;
        }

        #endregion

    }
}
