using MateralTools.MEncryption;
using MateralTools.MResult;
using MateralTools.MVerify;
using MissYangQA.DAL;
using MissYangQA.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        /// Token有效时间[分钟]
        /// </summary>
        private static double TokenOverdue = 1440;
        /// <summary>
        /// 用户数据访问对象
        /// </summary>
        private readonly UserDAL _userDAL = new UserDAL();
        /// <summary>
        /// 默认密码
        /// </summary>
        private const string DEFULTPASSWORD = "123456";
        #endregion
        #region 构造方法
        public UserBLL()
        {
            string tokenOverdue = ConfigurationManager.AppSettings["TokenOverdue"];
            if (!string.IsNullOrEmpty(tokenOverdue) && VerifyManager.IsInteger(tokenOverdue))
            {
                TokenOverdue = Convert.ToDouble(tokenOverdue);
            }
        }
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
        /// 根据用户唯一标识获得用户视图信息
        /// </summary>
        /// <param name="id">用户唯一标识</param>
        /// <returns>查询结果</returns>
        public V_User GetUserViewInfoByID(Guid id)
        {
            V_User resM;
            resM = _userDAL.GetUserViewInfoByID(id);
            return resM;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="isEncrypted">是否已加密</param>
        /// <returns>登录用户信息</returns>
        public V_User Login(string userName,string password,bool isEncrypted)
        {
            if (!isEncrypted)
            {
                password = EncryptionManager.MD5Encode_32(password);
            }
            return Login(userName, password);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginM">登录对象</param>
        /// <returns>登录用户信息</returns>
        public V_User Login(LoginModel loginM)
        {
            return Login(loginM.UserName, loginM.Password, loginM.IsEncrypted);
        }
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="model">要添加的对象</param>
        /// <exception cref="ArgumentException">参数错误</exception>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public void AddUserInfo(T_User model)
        {
            if (model != null)
            {
                model.Password = EncryptionManager.MD5Encode_32(DEFULTPASSWORD);
                string msg = string.Empty;
                if (VerificationAdd(model, ref msg))
                {
                    model.Token = GetNewToken();
                    model.TokenCreateTime = DateTime.Now;
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
        /// 删除用户信息
        /// </summary>
        /// <param name="id">用户唯一标识</param>
        /// <exception cref="ArgumentNullException">参数错误异常</exception>
        public void DeleteUserInfo(Guid id)
        {
            T_User model = _userDAL.GetUserInfoByID(id);
            if (model != null)
            {
                _userDAL.Remove(model);
            }
            else
            {
                throw new ArgumentNullException($"参数{nameof(id)}错误。");
            }
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model">要修改的对象</param>
        /// <exception cref="ArgumentException">验证不通过异常</exception>
        /// <exception cref="ArgumentNullException">参数错误异常</exception>
        public void EditUserInfo(T_User model)
        {
            if (model != null)
            {
                string msg = string.Empty;
                if (VerificationEdit(model, ref msg))
                {
                    T_User dbModel = _userDAL.GetUserInfoByID(model.ID);
                    dbModel.UserName = model.UserName;
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
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户唯一标识</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <exception cref="ArgumentException">验证不通过异常</exception>
        /// <exception cref="ArgumentNullException">参数错误异常</exception>
        /// <exception cref="ApplicationException">逻辑错误</exception>
        public void EditPassword(Guid id, string oldPassword,string newPassword)
        {
            if (id != Guid.Empty && !string.IsNullOrEmpty(oldPassword) && !string.IsNullOrEmpty(newPassword))
            {
                T_User dbModel = _userDAL.GetUserInfoByID(id);
                if (dbModel != null)
                {
                    if (dbModel.Password == EncryptionManager.MD5Encode_32(oldPassword))
                    {
                        dbModel.Password = EncryptionManager.MD5Encode_32(newPassword);
                        _userDAL.SaveChange();
                    }
                    else
                    {
                        throw new ApplicationException("旧密码错误!");
                    }
                }
                else
                {
                    throw new ArgumentException($"参数{nameof(id)}错误。");
                }
            }
            else
            {
                if (id == Guid.Empty)
                {
                    throw new ArgumentNullException($"参数{nameof(id)}不可以为空。");
                }
                else if (string.IsNullOrEmpty(oldPassword))
                {
                    throw new ArgumentNullException($"参数{nameof(oldPassword)}不可以为空。");
                }
                else if (string.IsNullOrEmpty(newPassword))
                {
                    throw new ArgumentNullException($"参数{nameof(newPassword)}不可以为空。");
                }
            }
        }
        /// <summary>
        /// Token是否有效
        /// </summary>
        /// <param name="id">用户唯一标识</param>
        /// <param name="token">Token</param>
        /// <returns>是否有效</returns>
        public bool TokenValid(Guid id, string token)
        {
            T_User model = _userDAL.GetUserInfoByIDAndToken(id, token);
            return model != null && model.TokenCreateTime.AddMinutes(TokenOverdue) >= DateTime.Now;
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录用户信息</returns>
        private V_User Login(string userName, string password)
        {
            V_User resM = null;
            T_User model = _userDAL.GetUserInfoByUserName(userName);
            if (model != null)
            {
                if (model.Password.ToUpper() == password.ToUpper())
                {
                    model.Token = GetNewToken();
                    model.TokenCreateTime = DateTime.Now;
                    _userDAL.SaveChange();
                    resM = _userDAL.GetUserViewInfoByID(model.ID);
                }
            }
            return resM;
        }
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
        /// <summary>
        /// 获得新的Token
        /// </summary>
        /// <returns>新的Token</returns>
        private string GetNewToken()
        {
            string lib = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random rd = new Random();
            int Count;
            string token;
            T_User model;
            do
            {
                Count = rd.Next(15, 32);
                token = string.Empty;
                for (int i = 0; i < Count; i++)
                {
                    token += lib[rd.Next(0, lib.Length)];
                }
                token = EncryptionManager.MD5Encode_32(token);
                model = _userDAL.GetUserInfoByToken(token);
            } while (model != null);
            return token;
        }
        #endregion
    }
}
