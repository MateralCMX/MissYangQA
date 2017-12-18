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
    /// 试题数据访问类
    /// </summary>
    public sealed class PaperDAL : BaseDAL<T_Paper, V_Paper>
    {
        /// <summary>
        /// 根据条件获得试题信息
        /// </summary>
        /// <param name="title">试题标题</param>
        /// <param name="IsEnable">启用状态</param>
        /// <param name="pageM">分页对象</param>
        /// <returns>试题信息</returns>
        public List<V_Paper> GetPaperViewInfoByWhere(string title,bool? IsEnable, MPagingModel pageM)
        {
            Expression<Func<V_Paper, bool>> searchPredicate = LinqManager.True<V_Paper>();
            if (!string.IsNullOrEmpty(title))
            {
                searchPredicate = LinqManager.And(searchPredicate, m => m.Title.Contains(title));
            }
            if (IsEnable != null)
            {
                searchPredicate = LinqManager.And(searchPredicate, m => m.IsEnable == IsEnable.Value);
            }
            pageM.DataCount = _DB.V_Paper.Where(searchPredicate.Compile()).Count();
            List<V_Paper> resM = new List<V_Paper>();
            if (pageM.DataCount > 0)
            {
                resM = _DB.V_Paper.Where(searchPredicate.Compile()).OrderByDescending(m => m.CreateTime).Skip((pageM.PagingIndex - 1) * pageM.PagingSize).Take(pageM.PagingSize).ToList();
            }
            return resM;
        }
        /// <summary>
        /// 获得所有启用的试题信息
        /// </summary>
        /// <returns>试题信息</returns>
        public List<V_Paper> GetAllEnablePaperInfo()
        {
            List<V_Paper> listM = (from m in _DB.V_Paper
                                   where m.IsEnable == true
                                   orderby m.CreateTime descending
                                   select m).ToList();
            return listM;
        }
    }
}
