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
    /// 试题API控制器
    /// </summary>
    [RoutePrefix("api/Paper")]
    public sealed class PaperController : ApiBaseController
    {
        #region 成员
        /// <summary>
        /// 试题业务控制层
        /// </summary>
        private readonly PaperBLL _paperBLL = new PaperBLL();
        #endregion
        #region API
        /// <summary>
        /// 获得所有启用的试题信息
        /// </summary>
        /// <returns>所有试题信息</returns>
        [HttpGet]
        [Route("GetAllEnablePaperInfo")]
        [NotVerificationLogin]
        public MResultModel GetAllEnablePaperInfo()
        {
            MResultModel resM;
            List<V_Paper> listM = _paperBLL.GetAllEnablePaperInfo();
            resM = MResultModel<List<V_Paper>>.GetSuccessResultM(listM, "查询成功");
            return resM;
        }
        /// <summary>
        /// 根据条件获得试题信息
        /// </summary>
        /// <param name="Title">标题</param>
        /// <param name="IsEnable">启用状态</param>
        /// <param name="PageIndex">页数</param>
        /// <param name="PageSize">每页显示数量</param>
        /// <returns>所有试题信息</returns>
        [HttpGet]
        [Route("GetPaperInfoByWhere")]
        [NotVerificationLogin]
        public MResultModel GetPaperInfoByWhere(string Title, bool? IsEnable, int PageIndex, int PageSize)
        {
            MResultModel resM;
            MPagingModel pageM = new MPagingModel(PageIndex, PageSize);
            List<V_Paper> listM = _paperBLL.GetPaperInfoByWhere(Title, IsEnable, pageM);
            resM = MResultPagingModel<List<V_Paper>>.GetSuccessResultM(listM, pageM, "查询成功");
            return resM;
        }
        /// <summary>
        /// 修改试题信息
        /// </summary>
        /// <param name="inputM">要修改的对象</param>
        [HttpPost]
        [Route("EditPaperInfo")]
        public MResultModel EditPaperInfo(EditPaperModel inputM)
        {
            MResultModel resM;
            try
            {
                _paperBLL.EditPaperInfo(inputM);
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
        /// 添加试题信息
        /// </summary>
        /// <param name="inputM">要添加的对象</param>
        [HttpPost]
        [Route("AddPaperInfo")]
        public MResultModel AddPaperInfo(EditPaperModel inputM)
        {
            MResultModel resM;
            try
            {
                _paperBLL.AddPaperInfo(inputM);
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
        /// 删除试题信息
        /// </summary>
        /// <param name="inputM">删除对象</param>
        [HttpPost]
        [Route("DeletePaperInfo")]
        public MResultModel DeletePaperInfo(DeleteModel inputM)
        {
            MResultModel resM;
            try
            {
                _paperBLL.DeletePaperInfo(inputM.ID);
                resM = MResultModel.GetSuccessResultM("删除成功");
            }
            catch (ArgumentNullException ex)
            {
                resM = MResultModel.GetFailResultM(ex.Message);
            }
            return resM;
        }
        /// <summary>
        /// 根据试题唯一标识获得试题视图信息
        /// </summary>
        /// <param name="ID">试题唯一标识</param>
        /// <returns>查询结果</returns>
        [HttpGet]
        [Route("GetPaperInfoByID")]
        [NotVerificationLogin]
        public MResultModel GetPaperInfoByID(Guid ID)
        {
            MResultModel resM;
            V_Paper listM = _paperBLL.GetPaperViewInfoByID(ID);
            resM = MResultModel<V_Paper>.GetSuccessResultM(listM, "查询成功");
            return resM;
        }
        #endregion
    }
}
