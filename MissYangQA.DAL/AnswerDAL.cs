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
    public sealed class AnswerDAL : BaseDAL
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">要添加的对象</param>
        public void Insert(T_Answer model)
        {
            model = BeforeInsert(model);
            _DB.T_Answer.Add(model);
            _DB.SaveChanges();
        }
        /// <summary>
        /// 根据答案唯一标识获得答案信息
        /// </summary>
        /// <param name="id">答案唯一标识</param>
        /// <returns>查询结果</returns>
        public T_Answer GetAnswerInfoByID(Guid id)
        {
            T_Answer model = (from m in _DB.T_Answer
                              where m.ID == id
                              select m).FirstOrDefault();
            return model;
        }
        /// <summary>
        /// 根据答案唯一标识获得答案视图信息
        /// </summary>
        /// <param name="id">答案唯一标识</param>
        /// <returns>查询结果</returns>
        public V_Answer GetAnswerViewInfoByID(Guid id)
        {
            V_Answer model = (from m in _DB.V_Answer
                              where m.ID == id
                              select m).FirstOrDefault();
            return model;
        }
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
