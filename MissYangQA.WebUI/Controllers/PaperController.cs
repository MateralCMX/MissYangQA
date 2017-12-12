using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MissYangQA.WebUI.Controllers
{
    public class PaperController : Controller
    {
        /// <summary>
        /// 试题列表
        /// </summary>
        /// <returns></returns>
        public ActionResult PaperList()
        {
            return View();
        }
        /// <summary>
        /// 试题详情
        /// </summary>
        /// <returns></returns>
        public ActionResult PaperDetails()
        {
            return View();
        }
    }
}