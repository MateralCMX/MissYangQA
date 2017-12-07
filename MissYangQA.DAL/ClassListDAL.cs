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
    public sealed class ClassListDAL : BaseDAL
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">要添加的对象</param>
        public void Insert(T_ClassList model)
        {
            model = BeforeInsert(model);
            _DB.T_ClassList.Add(model);
            _DB.SaveChanges();
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="model">要移除的对象</param>
        public void Remove(T_ClassList model)
        {
            _DB.T_ClassList.Remove(model);
            _DB.SaveChanges();
        }
        /// <summary>
        /// 根据班级唯一标识获得班级信息
        /// </summary>
        /// <param name="id">班级唯一标识</param>
        /// <returns>查询结果</returns>
        public T_ClassList GetClassListInfoByID(Guid id)
        {
            T_ClassList model = (from m in _DB.T_ClassList
                                 where m.ID == id
                                 select m).FirstOrDefault();
            return model;
        }
        /// <summary>
        /// 获得所有班级的视图信息
        /// </summary>
        /// <returns>查询结果</returns>
        public List<T_ClassList> GetAllClassListInfo()
        {
            List<T_ClassList> listM = _DB.T_ClassList.OrderByDescending(m => m.Rank).ToList();
            return listM;
        }
        /// <summary>
        /// 根据班级名获得班级的信息
        /// </summary>
        /// <param name="name">班级名</param>
        /// <returns>查询结果</returns>
        public T_ClassList GetClassListInfoByClassListName(string name)
        {
            T_ClassList model = (from m in _DB.T_ClassList
                                 where m.Name == name
                                 select m).FirstOrDefault();
            return model;
        }
        /// <summary>
        /// 获得班级最大位序
        /// </summary>
        /// <returns>最大位序</returns>
        public int GetClassListMaxRank()
        {
            int maxRank = 0;
            if (_DB.T_ClassList.FirstOrDefault() != null)
            {
                maxRank = _DB.T_ClassList.Max(m => m.Rank);
            }
            return maxRank;
        }
    }
}
