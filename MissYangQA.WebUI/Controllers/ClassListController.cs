using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MissYangQA.WebUI.Controllers
{
    public class ClassListController : Controller
    {
        /// <summary>
        /// 班级列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassList()
        {
            return View();
        }
        /// <summary>
        /// 班级明细
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassListDetailed()
        {
            return View();
        }
    }
}
