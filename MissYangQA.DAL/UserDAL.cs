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
    /// 用户数据访问类
    /// </summary>
    public sealed class UserDAL : BaseDAL
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">要添加的对象</param>
        public void Insert(T_User model)
        {
            model = BeforeInsert(model);
            _DB.T_User.Add(model);
            _DB.SaveChanges();
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="model">要移除的对象</param>
        public void Remove(T_User model)
        {
            _DB.T_User.Remove(model);
            _DB.SaveChanges();
        }
        /// <summary>
        /// 根据用户唯一标识获得用户信息
        /// </summary>
        /// <param name="id">用户唯一标示</param>
        /// <returns>查询结果</returns>
        public T_User GetUserInfoByID(Guid id)
        {
            T_User model = (from m in _DB.T_User
                            where m.ID == id
                            select m).FirstOrDefault();
            return model;
        }
        /// <summary>
        /// 根据用户唯一标识获得用户视图信息
        /// </summary>
        /// <param name="id">用户唯一标示</param>
        /// <returns>查询结果</returns>
        public V_User GetUserViewInfoByID(Guid id)
        {
            V_User model = (from m in _DB.V_User
                            where m.ID == id
                            select m).FirstOrDefault();
            return model;
        }
        /// <summary>
        /// 获得所有用户的视图信息
        /// </summary>
        /// <returns>查询结果</returns>
        public List<V_User> GetAllUserViewInfo(MPagingModel pageM)
        {
            int count = _DB.V_User.Count();
            List<V_User> listM = _DB.V_User.OrderBy(m => m.UserName).Skip(pageM.PagingIndex * pageM.PagingSize).Take(pageM.PagingSize).ToList();
            pageM.DataCount = count;
            return listM;
        }
        /// <summary>
        /// 根据用户名获得用户的信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>查询结果</returns>
        public T_User GetUserInfoByUserName(string userName)
        {
            T_User model = (from m in _DB.T_User
                            where m.UserName == userName
                            select m).FirstOrDefault();
            return model;
        }
    }
}
