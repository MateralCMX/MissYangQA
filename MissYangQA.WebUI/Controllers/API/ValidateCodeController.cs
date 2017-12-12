using MateralTools.MEncryption;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace MissYangQA.WebUI.Controllers.API
{
    /// <summary>
    /// 验证码控制器
    /// </summary>
    [RoutePrefix("api/ValidateCode")]
    public class ValidateCodeController : ApiBaseController
    {
        /// <summary>
        ///  获取二维码
        /// </summary>
        /// <param name="ClassID">班级ID</param>
        /// <param name="PaperID">试题ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetQRImage")]
        public HttpResponseMessage GetQRImage(Guid ClassID, Guid PaperID)
        {
            string _domainName = System.Configuration.ConfigurationManager.AppSettings["DomainName"];
            Bitmap img = EncryptionManager.QRCodeEncode(_domainName + $"Paper/ExamIndex?ClassID={ClassID.ToString()}&PaperID={PaperID.ToString()}");
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Jpeg);
                img.Dispose();
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(ms.ToArray())
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                return result;
            }
        }
    }
}