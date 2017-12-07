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
        /// <param name="model">登录模型</param>
        /// <returns>登录结果</returns>
        [HttpPost]
        [Route("Login")]
        [NotVerificationLogin]
        public MResultModel Login(LoginModel inputM)
        {
            MResultModel resM;
            try
            {
                V_User model = _userBLL.Login(inputM);
                if (model != null)
                {
                    resM = MResultModel<V_User>.GetSuccessResultM(model, "登录成功");
                }
                else
                {
                    resM = MResultModel.GetFailResultM("登录失败，用户名或者密码错误");
                }
            }
            catch(Exception)
            {
                resM = MResultModel.GetErrorResultM("应用程序出错了！");
            }
            return resM;
        }
        #endregion
    }
}
