using MissYangQA.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace MissYangQA.WebUI
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiExceptionAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public WebApiExceptionAttribute()
        {
        }
        /// <summary>
        /// 异常处理方法
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Exception ex = actionExecutedContext.Exception;
            string Contents = $"Message:${ ex.Message}\nStackTrace:${ ex.StackTrace}";
            ApplicationLogBLL.WriteExceptionLog(ex.GetType().Name, Contents);
            base.OnException(actionExecutedContext);
            //actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }
}