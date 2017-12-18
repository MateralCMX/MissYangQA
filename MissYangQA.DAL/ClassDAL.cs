using MateralTools.MResult;
using MissYangQA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissYangQA.DAL
{
    /// <summary>
    /// 班级数据访问类
    /// </summary>
    public sealed class ClassDAL : BaseDAL<T_Class, V_Class>
    {
        /// <summary>
        /// 获得所有班级视图信息
        /// </summary>
        /// <returns>查询结果</returns>
        public List<V_Class> GetAllClassViewInfo()
        {
            List<V_Class> listM = _DB.V_Class.OrderBy(m => m.Rank).ToList();
            return listM;
        }
        /// <summary>
        /// 根据班级名获得班级的信息
        /// </summary>
        /// <param name="name">班级名</param>
        /// <returns>查询结果</returns>
        public T_Class GetClassInfoByClassName(string name)
        {
            T_Class model = (from m in _DB.T_Class
                             where m.Name == name
                             select m).FirstOrDefault();
            return model;
        }
        /// <summary>
        /// 获得班级最大位序
        /// </summary>
        /// <returns>最大位序</returns>
        public int GetClassMaxRank()
        {
            int maxRank = 0;
            if (_DB.T_Class.FirstOrDefault() != null)
            {
                maxRank = _DB.T_Class.Max(m => m.Rank);
            }
            return maxRank;
        }
    }
}
