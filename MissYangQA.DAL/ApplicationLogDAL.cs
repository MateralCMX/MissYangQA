using MissYangQA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissYangQA.DAL
{
    /// <summary>
    /// 日志数据访问类
    /// </summary>
    public sealed class ApplicationLogDAL :BaseDAL
    {
        /// <summary>
        /// 添加一个日志
        /// </summary>
        /// <param name="model">要添加的日志信息</param>
        public void Insert(T_ApplicationLog model)
        {
            model = BeforeInsert(model);
            _DB.T_ApplicationLog.Add(model);
            _DB.SaveChanges();
        }
    }
}
