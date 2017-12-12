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
    public sealed class ProblemDAL : BaseDAL
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">要添加的对象</param>
        public void Insert(T_Problem model)
        {
            model = BeforeInsert(model);
            _DB.T_Problem.Add(model);
            _DB.SaveChanges();
        }
        /// <summary>
        /// 根据问题唯一标识获得问题信息
        /// </summary>
        /// <param name="id">问题唯一标识</param>
        /// <returns>查询结果</returns>
        public T_Problem GetProblemInfoByID(Guid id)
        {
            T_Problem model = (from m in _DB.T_Problem
                             where m.ID == id
                             select m).FirstOrDefault();
            return model;
        }
        /// <summary>
        /// 根据问题唯一标识获得问题视图信息
        /// </summary>
        /// <param name="id">问题唯一标识</param>
        /// <returns>查询结果</returns>
        public V_Problem GetProblemViewInfoByID(Guid id)
        {
            V_Problem model = (from m in _DB.V_Problem
                             where m.ID == id
                             select m).FirstOrDefault();
            return model;
        }
        /// <summary>
        /// 根据试题ID获得问题列表
        /// </summary>
        /// <param name="paperID">试题ID</param>
        /// <returns>问题列表</returns>
        public List<T_Problem> GetProblemInfoByPaperID(Guid paperID)
        {
            List<T_Problem> resM = (from m in _DB.T_Problem
                                    where m.FK_Paper == paperID
                                    orderby m.CreateTime
                                    select m).ToList();
            return resM;
        }
        /// <summary>
        /// 根据试题ID获得问题列表
        /// </summary>
        /// <param name="paperID">试题ID</param>
        /// <returns>问题列表</returns>
        public List<V_Problem> GetProblemViewInfoByPaperID(Guid paperID)
        {
            List<V_Problem> resM = (from m in _DB.V_Problem
                                    where m.FK_Paper == paperID
                                    orderby m.CreateTime
                                    select m).ToList();
            return resM;
        }
    }
}
