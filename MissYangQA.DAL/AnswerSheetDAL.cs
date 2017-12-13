using MateralTools.MLinq;
using MateralTools.MResult;
using MissYangQA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MissYangQA.DAL
{
    /// <summary>
    /// 答题卡数据访问类
    /// </summary>
    public sealed class AnswerSheetDAL : BaseDAL
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">要添加的对象</param>
        public void Insert(T_AnswerSheet model)
        {
            _DB.T_AnswerSheet.Add(model);
            _DB.SaveChanges();
        }
    }
}
