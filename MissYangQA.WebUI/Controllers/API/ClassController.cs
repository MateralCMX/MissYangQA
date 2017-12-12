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
    [RoutePrefix("api/Class")]
    public sealed class ClassController : ApiBaseController
    {
        #region 成员
        /// <summary>
        /// 班级业务控制层
        /// </summary>
        private readonly ClassBLL _classListBLL = new ClassBLL();
        #endregion
        #region API
        /// <summary>
        /// 获得所有的班级信息
        /// </summary>
        /// <returns>所有班级信息</returns>
        [HttpGet]
        [Route("GetAllClassInfo")]
        [NotVerificationLogin]
        public MResultModel GetAllClassInfo()
        {
            MResultModel resM;
            List<V_Class> listM = _classListBLL.GetAllClassViewInfo();
            resM = MResultModel<List<V_Class>>.GetSuccessResultM(listM, "查询成功");
            return resM;
        }
        /// <summary>
        /// 调换班级位序
        /// </summary>
        /// <param name="inputM">要修改的对象</param>
        [HttpPost]
        [Route("ChangeClassRank")]
        public MResultModel ChangeClassRank(ChangeClassRankModel inputM)
        {
            MResultModel resM;
            try
            {
                _classListBLL.ChangeClassRank(inputM.ClassID, inputM.TargetClassID);
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
        [Route("EditClassInfo")]
        public MResultModel EditClassInfo(EditClassModel inputM)
        {
            MResultModel resM;
            try
            {
                _classListBLL.EditClassInfo(inputM);
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
        [Route("AddClassInfo")]
        public MResultModel AddClassInfo(EditClassModel inputM)
        {
            MResultModel resM;
            try
            {
                _classListBLL.AddClassInfo(inputM);
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
        [Route("DeleteClassInfo")]
        public MResultModel DeleteClassInfo(DeleteModel inputM)
        {
            MResultModel resM;
            try
            {
                _classListBLL.DeleteClassInfo(inputM.ID);
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
        [Route("GetClassInfoByID")]
        [NotVerificationLogin]
        public MResultModel GetClassInfoByID(Guid ID)
        {
            MResultModel resM;
            V_Class listM = _classListBLL.GetClassViewInfoByID(ID);
            resM = MResultModel<V_Class>.GetSuccessResultM(listM, "查询成功");
            return resM;
        }
        #endregion
    }
}
