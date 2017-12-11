using MateralTools.MResult;
using MissYangQA.BLL;
using MissYangQA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MissYangQA.WebUI.Controllers.API
{
    /// <summary>
    /// 用户API控制器
    /// </summary>
    [RoutePrefix("api/User")]
    public sealed class UserController : ApiBaseController
    {
        #region 成员
        /// <summary>
        /// 用户业务控制层
        /// </summary>
        private readonly UserBLL _userBLL = new UserBLL();
        #endregion
        #region API
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="inputM">登录模型</param>
        /// <returns>登录结果</returns>
        [HttpPost]
        [Route("Login")]
        [NotVerificationLogin]
        public MResultModel Login(LoginModel inputM)
        {
            MResultModel resM;
            V_User model = _userBLL.Login(inputM);
            if (model != null)
            {
                resM = MResultModel<V_User>.GetSuccessResultM(model, "登录成功");
            }
            else
            {
                resM = MResultModel.GetFailResultM("登录失败，用户名或者密码错误");
            }
            return resM;
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="inputM">要修改的对象</param>
        [HttpPost]
        [Route("EditUserInfo")]
        public MResultModel EditUserInfo(EditUserModel inputM)
        {
            MResultModel resM;
            try
            {
                _userBLL.EditUserInfo(inputM);
                resM = MResultModel.GetSuccessResultM("修改成功");
            }
            catch (ArgumentNullException ex)
            {
                resM = MResultModel.GetFailResultM(ex.Message);
            }
            catch (ArgumentException ex)
            {
                resM = MResultModel.GetFailResultM(ex.Message);
            }
            return resM;
        }
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="inputM">要添加的对象</param>
        [HttpPost]
        [Route("AddUserInfo")]
        public MResultModel AddUserInfo(EditUserModel inputM)
        {
            MResultModel resM;
            try
            {
                _userBLL.AddUserInfo(inputM);
                resM = MResultModel.GetSuccessResultM("添加成功");
            }
            catch (ArgumentNullException ex)
            {
                resM = MResultModel.GetFailResultM(ex.Message);
            }
            catch (ArgumentException ex)
            {
                resM = MResultModel.GetFailResultM(ex.Message);
            }
            return resM;
        }
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="inputM">删除对象</param>
        [HttpPost]
        [Route("DeleteUserInfo")]
        public MResultModel DeleteUserInfo(DeleteModel inputM)
        {
            MResultModel resM;
            try
            {
                _userBLL.DeleteUserInfo(inputM.ID);
                resM = MResultModel.GetSuccessResultM("删除成功");
            }
            catch (ArgumentNullException ex)
            {
                resM = MResultModel.GetFailResultM(ex.Message);
            }
            return resM;
        }
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="inputM">修改密码对象</param>
        [HttpPost]
        [Route("ChangePassword")]
        public MResultModel ChangePassword(EditPasswordModel inputM)
        {
            MResultModel resM;
            try
            {
                _userBLL.ChangePassword(inputM.ID, inputM.OldPassword, inputM.NewPassword);
                resM = MResultModel.GetSuccessResultM("修改成功");
            }
            catch (ApplicationException ex)
            {
                resM = MResultModel.GetFailResultM(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                resM = MResultModel.GetFailResultM(ex.Message);
            }
            catch (ArgumentException ex)
            {
                resM = MResultModel.GetFailResultM(ex.Message);
            }
            return resM;
        }
        /// <summary>
        /// 获得所有的用户信息
        /// </summary>
        /// <param name="PageIndex">页数</param>
        /// <param name="PageSize">每页显示数量</param>
        /// <returns>所有用户信息</returns>
        [HttpGet]
        [Route("GetAllUserInfo")]
        [NotVerificationLogin]
        public MResultModel GetAllUserInfo(int PageIndex, int PageSize)
        {
            MResultModel resM;
            if (PageIndex > 0)
            {
                if (PageSize > 0)
                {
                    MPagingModel pageM = new MPagingModel
                    {
                        PagingIndex = PageIndex,
                        PagingSize = PageSize
                    };
                    List<V_User> listM = _userBLL.GetAllUserViewInfo(pageM);
                    resM = MResultPagingModel<List<V_User>>.GetSuccessResultM(listM, pageM, "查询成功");
                }
                else
                {
                    resM = MResultModel.GetFailResultM($"参数${nameof(PageSize)}必须大于0");
                }
            }
            else
            {
                resM = MResultModel.GetFailResultM($"参数${nameof(PageIndex)}必须大于0");
            }
            return resM;
        }
        /// <summary>
        /// 根据用户唯一标识获得用户视图信息
        /// </summary>
        /// <param name="ID">用户唯一标识</param>
        /// <returns>查询结果</returns>
        [HttpGet]
        [Route("GetUserViewInfoByID")]
        [NotVerificationLogin]
        public MResultModel GetUserViewInfoByID(Guid ID)
        {
            MResultModel resM;
            V_User listM = _userBLL.GetUserViewInfoByID(ID);
            resM = MResultModel<V_User>.GetSuccessResultM(listM, "查询成功");
            return resM;
        }
        #endregion
    }
}
