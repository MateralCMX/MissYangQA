﻿using System;

namespace MissYangQA.Model
{
    /// <summary>
    /// 查询模型
    /// </summary>
    public class QueryBaseModel : IVerificationLoginModel
    {
        #region 成员
        /// <summary>
        /// 登录用户ID
        /// </summary>
        public Guid? LoginUserID { get; set; }
        /// <summary>
        /// 登录用户令牌
        /// </summary>
        public string LoginUserToken { get; set; }
        #endregion
    }
    /// <summary>
    /// 分页查询模型
    /// </summary>
    public class QueryPagingBaseModel : QueryBaseModel
    {
        #region 成员
        /// <summary>
        /// 页数
        /// </summary>
        public int? PageIndex { get; set; }
        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int? PageSize { get; set; }
        #endregion
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        public QueryPagingBaseModel()
        {
            PageIndex = 1;
            PageSize = 10;
        }
        #endregion
    }
    /// <summary>
    /// 登录验证模型接口
    /// </summary>
    public interface IVerificationLoginModel
    {
        /// <summary>
        /// 登录用户ID
        /// </summary>
        Guid? LoginUserID { get; set; }
        /// <summary>
        /// 登录用户令牌
        /// </summary>
        string LoginUserToken { get; set; }
    }
}