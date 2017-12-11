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
    /// 班级API控制器
    /// </summary>
    [RoutePrefix("api/ClassList")]
    public sealed class ClassListController : ApiBaseController
    {
        #region 成员
        /// <summary>
        /// 班级业务控制层
        /// </summary>
        private readonly ClassListBLL _classListBLL = new ClassListBLL();
        #endregion
        #region API
        /// <summary>
        /// 获得所有的班级信息
        /// </summary>
        /// <param name="PageIndex">页数</param>
        /// <param name="PageSize">每页显示数量</param>
        /// <returns>所有班级信息</returns>
        [HttpGet]
        [Route("GetAllClassListInfo")]
        [NotVerificationLogin]
        public MResultModel GetAllClassListInfo()
        {
            MResultModel resM;
            List<V_ClassList> listM = _classListBLL.GetAllClassListViewInfo();
            resM = MResultModel<List<V_ClassList>>.GetSuccessResultM(listM, "查询成功");
            return resM;
        }
        /// <summary>
        /// 调换班级位序
        /// </summary>
        /// <param name="inputM">要修改的对象</param>
        [HttpPost]
        [Route("ChangeClassListRank")]
        public MResultModel ChangeClassListRank(ChangeClassListRankModel inputM)
        {
            MResultModel resM;
            try
            {
                _classListBLL.ChangeClassListRank(inputM.ClassListID, inputM.TargetClassListID);
                resM = MResultModel.GetSuccessResultM("调换成功");
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
        /// 修改班级信息
        /// </summary>
        /// <param name="inputM">要修改的对象</param>
        [HttpPost]
        [Route("EditClassListInfo")]
        public MResultModel EditClassListInfo(EditClassListModel inputM)
        {
            MResultModel resM;
            try
            {
                _classListBLL.EditClassListInfo(inputM);
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
        /// 添加班级信息
        /// </summary>
        /// <param name="inputM">要添加的对象</param>
        [HttpPost]
        [Route("AddClassListInfo")]
        public MResultModel AddClassListInfo(EditClassListModel inputM)
        {
            MResultModel resM;
            try
            {
                _classListBLL.AddClassListInfo(inputM);
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
        /// 删除班级信息
        /// </summary>
        /// <param name="inputM">删除对象</param>
        [HttpPost]
        [Route("DeleteClassListInfo")]
        public MResultModel DeleteClassListInfo(DeleteModel inputM)
        {
            MResultModel resM;
            try
            {
                _classListBLL.DeleteClassListInfo(inputM.ID);
                resM = MResultModel.GetSuccessResultM("删除成功");
            }
            catch (ArgumentNullException ex)
            {
                resM = MResultModel.GetFailResultM(ex.Message);
            }
            return resM;
        }
        /// <summary>
        /// 根据班级唯一标识获得班级视图信息
        /// </summary>
        /// <param name="ID">班级唯一标识</param>
        /// <returns>查询结果</returns>
        [HttpGet]
        [Route("GetClassListInfoByID")]
        [NotVerificationLogin]
        public MResultModel GetClassListInfoByID(Guid ID)
        {
            MResultModel resM;
            V_ClassList listM = _classListBLL.GetClassListViewInfoByID(ID);
            resM = MResultModel<V_ClassList>.GetSuccessResultM(listM, "查询成功");
            return resM;
        }
        #endregion
    }
}
