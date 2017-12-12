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
    public sealed class ClassDAL : BaseDAL
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">要添加的对象</param>
        public void Insert(T_Class model)
        {
            model = BeforeInsert(model);
            _DB.T_Class.Add(model);
            _DB.SaveChanges();
        }
        /// <summary>
        /// 根据班级唯一标识获得班级信息
        /// </summary>
        /// <param name="id">班级唯一标识</param>
        /// <returns>查询结果</returns>
        public T_Class GetClassInfoByID(Guid id)
        {
            T_Class model = (from m in _DB.T_Class
                             where m.ID == id
                                 select m).FirstOrDefault();
            return model;
        }
        /// <summary>
        /// 根据班级唯一标识获得班级视图信息
        /// </summary>
        /// <param name="id">班级唯一标识</param>
        /// <returns>查询结果</returns>
        public V_Class GetClassViewInfoByID(Guid id)
        {
            V_Class model = (from m in _DB.V_Class
                                 where m.ID == id
                                 select m).FirstOrDefault();
            return model;
        }
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
