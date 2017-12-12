using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MissYangQA.WebUI.Controllers
{
    public class ProblemController : Controller
    {
        /// <summary>
        /// 问题列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ProblemList()
        {
            return View();
        }
        /// <summary>
        /// 问题明细
        /// </summary>
        /// <returns></returns>
        public ActionResult ProblemDetails()
        {
            return View();
        }
    }
}