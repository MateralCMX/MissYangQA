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
    public sealed class ProblemDAL : BaseDAL<T_Problem, V_Problem>
    {
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
