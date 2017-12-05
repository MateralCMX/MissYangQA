using MateralTools.MEncryption;
using MateralTools.MResult;
using MissYangQA.DAL;
using MissYangQA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissYangQA.BLL
{
    /// <summary>
    /// 用户业务类
    /// </summary>
    public sealed class UserBLL : BaseBLL<T_User>
    {
        #region 成员
        /// <summary>
        /// 用户数据访问对象
        /// </summary>
        private readonly UserDAL _userDAL = new UserDAL();
        #endregion
        #region 公共方法
        /// <summary>
        /// 获得所有的用户信息
        /// </summary>
        /// <param name="pageM">分页信息</param>
        /// <returns>所有用户的信息</returns>
        public List<V_User> GetAllUserViewInfo(MPagingModel pageM)
        {
            List<V_User> resM;
            resM = _userDAL.GetAllUserViewInfo(pageM);
            return resM;
        }
        /// <summary>
        /// 获得所有用户的视图信息
        /// </summary>
        /// <returns>查询结果</returns>
        public V_User GetUserViewInfoByID(Guid id)
        {
            V_User resM;
            resM = _userDAL.GetUserViewInfoByID(id);
            return resM;
        }
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="model">要添加的对象</param>
        public void AddUserInfo(T_User model)
        {
            if (model != null)
            {
                string msg = string.Empty;
                if (VerificationAdd(model, ref msg))
                {
                    _userDAL.Insert(model);
                }
                else
                {
                    throw new ArgumentException(msg);
                }
            }
            else
            {
                throw new ArgumentNullException($"参数{nameof(model)}不可以为空。");
            }
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model">要修改的对象</param>
        public void EditUserInfo(T_User model)
        {
            if (model != null)
            {
                string msg = string.Empty;
                if (VerificationEdit(model, ref msg))
                {
                    T_User dbModel = _userDAL.GetUserInfoByID(model.ID);
                    dbModel.UserName = model.UserName;
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        dbModel.Password = EncryptionManager.MD5Encode_32(model.Password);
                    }
                    _userDAL.SaveChange();
                }
                else
                {
                    throw new ArgumentException(msg);
                }
            }
            else
            {
                throw new ArgumentNullException($"参数{nameof(model)}不可以为空。");
            }
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 验证模型
        /// </summary>
        /// <param name="model">要验证的模型</param>
        /// <param name="msg">提示信息</param>
        /// <returns>验证结果</returns>
        protected override bool Verification(T_User model, ref string msg)
        {
            if (string.IsNullOrEmpty(model.UserName))
            {
                msg += "用户名不可为空，";
            }
            return base.Verification(model, ref msg);
        }
        /// <summary>
        /// 验证添加模型
        /// </summary>
        /// <param name="model">要验证的模型</param>
        /// <param name="msg">提示信息</param>
        /// <returns>验证结果</returns>
        private bool VerificationAdd(T_User model, ref string msg)
        {
            msg = string.Empty;
            T_User dbModel = _userDAL.GetUserInfoByUserName(model.UserName);
            if (dbModel != null)
            {
                msg += "该用户名已被使用，";
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                msg += "密码不可为空，";
            }
            return Verification(model, ref msg);
        }
        /// <summary>
        /// 验证修改模型
        /// </summary>
        /// <param name="model">要验证的模型</param>
        /// <param name="msg">提示信息</param>
        /// <returns>验证结果</returns>
        private bool VerificationEdit(T_User model, ref string msg)
        {
            msg = string.Empty;
            T_User dbModel = _userDAL.GetUserInfoByUserName(model.UserName);
            if (dbModel != null && dbModel.ID != model.ID)
            {
                msg += "该用户名已被使用，";
            }
            return Verification(model, ref msg);
        }
        #endregion
    }
}
