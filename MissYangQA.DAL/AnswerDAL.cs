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
    public sealed class AnswerDAL : BaseDAL<T_Answer, V_Answer>
    {
        /// <summary>
        /// 根据问题唯一标识获取答案信息
        /// </summary>
        /// <param name="problemID">问题唯一标识</param>
        /// <returns>答案信息</returns>
        public List<V_Answer> GetAnswerViewInfoByProblemID(Guid problemID)
        {
            List<V_Answer> model = (from m in _DB.V_Answer
                                    where m.FK_Problem == problemID
                                    orderby m.CreateTime
                                    select m).ToList();
            return model;
        }
    }
}
