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
    public sealed class AnswerSheetDetailsDAL : BaseDAL
    {
        /// <summary>
        /// 根据答题卡唯一标识获得答题卡明细信息
        /// </summary>
        /// <param name="id">答题卡ID</param>
        /// <returns>答题卡明细信息</returns>
        public List<T_AnswerSheetDetails> GetAnswerSheetDetailsInfoByAnswerSheetID(Guid id)
        {
            return (from m in _DB.T_AnswerSheetDetails
                    where m.FK_AnswerSheet == id
                    select m).ToList();
        }
    }
}
