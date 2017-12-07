using MateralTools.MResult;
using System;
using System.Web;

namespace MissYangQA.WebUI
{
    /// <summary>
    /// 应用程序管理类
    /// </summary>
    public static class ApplicationManager
    {
        /// <summary>
        /// 验证码SessionKey
        /// </summary>
        public const string VALIDATECODEKEY = "ValidateCodeValue";
        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        public static void SetSession(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }
        /// <summary>
        /// 获得Session
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>保存的值</returns>
        public static object GetSession(string key)
        {
            return HttpContext.Current.Session[key];
        }
        /// <summary>
        /// 获得Session
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>保存的值</returns>
        public static T GetSession<T>(string key)
        {
            return (T)GetSession(key);
        }
    }
}
