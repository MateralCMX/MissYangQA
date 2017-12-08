﻿using MateralTools.MResult;
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
        /// <param name="id">用户唯一标识</param>
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
        /// <param name="id">用户唯一标识</param>
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
        /// <param name="pageM">分页模型</param>
        /// <returns>查询结果</returns>
        public List<V_User> GetAllUserViewInfo(MPagingModel pageM)
        {
            List<V_User> listM = new List<V_User>();
            pageM.DataCount = _DB.V_User.Count();
            if (pageM.DataCount > 0)
            {
                listM.AddRange(_DB.V_User.OrderBy(m => m.UserName).Skip((pageM.PagingIndex - 1) * pageM.PagingSize).Take(pageM.PagingSize).ToList());
            }
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
        /// <summary>
        /// 根据Token获得用户信息
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns>用户信息</returns>
        public T_User GetUserInfoByToken(string token)
        {
            T_User model = (from m in _DB.T_User
                            where m.Token == token
                            select m).FirstOrDefault();
            return model;
        }
        /// <summary>
        /// 根据用户唯一标识和Token获得用户信息
        /// </summary>
        /// <param name="id">用户唯一标识</param>
        /// <param name="token">Token</param>
        /// <returns>用户信息</returns>
        public T_User GetUserInfoByIDAndToken(Guid id, string token)
        {
            T_User model = (from m in _DB.T_User
                            where m.ID == id && m.Token == token
                            select m).FirstOrDefault();
            return model;
        }
    }
}
