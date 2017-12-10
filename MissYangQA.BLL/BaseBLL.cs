using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissYangQA.BLL
{
    /// <summary>
    /// 业务处理类父类
    /// </summary>
    /// <typeparam name="T">数据库对应模型</typeparam>
    public class BaseBLL<T>
    {
        /// <summary>
        /// 验证模型
        /// </summary>
        /// <param name="model">要验证的模型</param>
        /// <returns>验证结果</returns>
        protected bool Verification(T model)
        {
            string msg = string.Empty;
            return Verification(model, ref msg);
        }
        /// <summary>
        /// 验证模型
        /// </summary>
        /// <param name="model">要验证的模型</param>
        /// <param name="msg">提示信息</param>
        /// <returns>验证结果</returns>
        protected virtual bool Verification(T model, ref string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                msg = "验证失败：" + msg.Substring(0, msg.Length - 1) + "。";
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
