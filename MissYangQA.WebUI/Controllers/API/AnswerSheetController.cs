using MateralTools.MEnum;
using MateralTools.MResult;
using MissYangQA.BLL;
using MissYangQA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MissYangQA.WebUI.Controllers.API
{
    /// <summary>
    /// 答题卡API控制器
    /// </summary>
    [RoutePrefix("api/AnswerSheet")]
    public sealed class AnswerSheetController : ApiBaseController
    {
        #region 成员
        /// <summary>
        /// 答题卡业务控制层
        /// </summary>
        private readonly AnswerSheetBLL _answerSheetBLL = new AnswerSheetBLL();
        #endregion
        #region API
        /// <summary>
        /// 修改答题卡信息
        /// </summary>
        /// <param name="inputM">要修改的对象</param>
        [HttpPost]
        [Route("SubmitAnswerSheet")]
        public MResultModel SubmitAnswerSheet(AnswerSheetSubmitModel inputM)
        {
            MResultModel resM;
            try
            {
                inputM.StudentName = HttpUtility.UrlDecode(inputM.StudentName);
                Guid ID = _answerSheetBLL.SubmitAnswerSheet(inputM);
                resM = MResultModel<Guid>.GetSuccessResultM(ID, "提交成功");
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
        /// 根据唯一标识获得答题卡结果
        /// </summary>
        /// <param name="ID">唯一标识</param>
        /// <returns>答题卡结果</returns>
        [HttpGet]
        [Route("GetAnswerSheetResultInfoByID")]
        public MResultModel GetAnswerSheetResultInfoByID(Guid ID)
        {
            MResultModel resM;
            try
            {
                AnswerSheetResultModel asrM = _answerSheetBLL.GetAnswerSheetResultInfoByID(ID);
                resM = MResultModel<AnswerSheetResultModel>.GetSuccessResultM(asrM, "查询成功");
            }
            catch (ArgumentException ex)
            {
                resM = MResultModel.GetFailResultM(ex.Message);
            }
            return resM;
        }
        #endregion
    }
}
