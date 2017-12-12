using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MissYangQA.WebUI.Controllers
{
    public class QRCodeController : Controller
    {
        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateQRCode()
        {
            return View();
        }
    }
}