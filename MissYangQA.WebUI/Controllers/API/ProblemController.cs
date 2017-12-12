using MateralTools.MEnum;
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
    /// 问题API控制器
    /// </summary>
    [RoutePrefix("api/Problem")]
    public sealed class ProblemController : ApiBaseController
    {
        #region 成员
        /// <summary>
        /// 问题业务控制层
        /// </summary>
        private readonly ProblemBLL _problemBLL = new ProblemBLL();
        #endregion
        #region API
        /// <summary>
        /// 根据试题唯一标识获得问题信息
        /// </summary>
        /// <param name="PaperID">试题唯一标识</param>
        /// <returns>所有问题信息</returns>
        [HttpGet]
        [Route("GetProblemInfoByPaperID")]
        [NotVerificationLogin]
        public MResultModel GetProblemInfoByPaperID(Guid PaperID)
        {
            MResultModel resM;
            List<ProblemModel> listM = _problemBLL.GetProblemInfoByPaperID(PaperID);
            resM = MResultModel<List<ProblemModel>>.GetSuccessResultM(listM, "查询成功");
            return resM;
        }
        /// <summary>
        /// 修改问题信息
        /// </summary>
        /// <param name="inputM">要修改的对象</param>
        [HttpPost]
        [Route("EditProblemInfo")]
        public MResultModel EditProblemInfo(EditProblemModel inputM)
        {
            MResultModel resM;
            try
            {
                _problemBLL.EditProblemInfo(inputM, inputM.Answers);
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
        /// 添加问题信息
        /// </summary>
        /// <param name="inputM">要添加的对象</param>
        [HttpPost]
        [Route("AddProblemInfo")]
        public MResultModel AddProblemInfo(EditProblemModel inputM)
        {
            MResultModel resM;
            try
            {
                _problemBLL.AddProblemInfo(inputM, inputM.Answers);
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
        /// 删除问题信息
        /// </summary>
        /// <param name="inputM">删除对象</param>
        [HttpPost]
        [Route("DeleteProblemInfo")]
        public MResultModel DeleteProblemInfo(DeleteModel inputM)
        {
            MResultModel resM;
            try
            {
                _problemBLL.DeleteProblemInfo(inputM.ID);
                resM = MResultModel.GetSuccessResultM("删除成功");
            }
            catch (ArgumentNullException ex)
            {
                resM = MResultModel.GetFailResultM(ex.Message);
            }
            return resM;
        }
        /// <summary>
        /// 根据问题唯一标识获得问题视图信息
        /// </summary>
        /// <param name="ID">问题唯一标识</param>
        /// <returns>查询结果</returns>
        [HttpGet]
        [Route("GetProblemInfoByID")]
        [NotVerificationLogin]
        public MResultModel GetProblemInfoByID(Guid ID)
        {
            MResultModel resM;
            ProblemModel listM = _problemBLL.GetProblemViewInfoByID(ID);
            resM = MResultModel<ProblemModel>.GetSuccessResultM(listM, "查询成功");
            return resM;
        }
        #endregion
    }
}
