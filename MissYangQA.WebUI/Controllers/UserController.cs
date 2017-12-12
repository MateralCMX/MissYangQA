using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MissYangQA.WebUI.Controllers
{
    public class UserController : Controller
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult UserList()
        {
            return View();
        }
        /// <summary>
        /// 用户明细
        /// </summary>
        /// <returns></returns>
        public ActionResult UserDetails()
        {
            return View();
        }
        /// <summary>
        /// 更改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}