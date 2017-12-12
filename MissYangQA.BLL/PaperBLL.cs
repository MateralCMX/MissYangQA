using MateralTools.MEncryption;
using MateralTools.MEnum;
using MateralTools.MResult;
using MateralTools.MVerify;
using MissYangQA.DAL;
using MissYangQA.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissYangQA.BLL
{
    /// <summary>
    /// 试题业务类
    /// </summary>
    public sealed class PaperBLL : BaseBLL<T_Paper>
    {
        #region 成员
        /// <summary>
        /// Token有效时间[分钟]
        /// </summary>
        private static double TokenOverdue = 1440;
        /// <summary>
        /// 试题数据访问对象
        /// </summary>
        private readonly PaperDAL _paperDAL = new PaperDAL();
        /// <summary>
        /// 默认密码
        /// </summary>
        private const string DEFULTPASSWORD = "123456";
        #endregion
        #region 构造方法
        public PaperBLL()
        {
            string tokenOverdue = ConfigurationManager.AppSettings["TokenOverdue"];
            if (!string.IsNullOrEmpty(tokenOverdue) && VerifyManager.IsInteger(tokenOverdue))
            {
                TokenOverdue = Convert.ToDouble(tokenOverdue);
            }
        }
        #endregion
        #region 公共方法
        /// <summary>
        /// 根据试题唯一标识获得试题视图信息
        /// </summary>
        /// <param name="id">试题唯一标识</param>
        /// <returns>查询结果</returns>
        public V_Paper GetPaperViewInfoByID(Guid id)
        {
            V_Paper resM;
            resM = _paperDAL.GetPaperViewInfoByID(id);
            return resM;
        }
        /// <summary>
        /// 添加试题信息
        /// </summary>
        /// <param name="model">要添加的对象</param>
        /// <exception cref="ArgumentException">参数错误</exception>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public void AddPaperInfo(T_Paper model)
        {
            if (model != null)
            {
                string msg = string.Empty;
                if (Verification(model, ref msg))
                {
                    model.IsDelete = false;
                    model.CreateTime = DateTime.Now;
                    _paperDAL.Insert(model);
                }
                else
                {
                    throw new ArgumentException(msg);
                }
            }
            else
            {
                throw new ArgumentNullException($"参数{nameof(model)}不可以为空。");
            }
        }
        /// <summary>
        /// 删除试题信息
        /// </summary>
        /// <param name="id">试题唯一标识</param>
        /// <exception cref="ArgumentNullException">参数错误异常</exception>
        public void DeletePaperInfo(Guid id)
        {
            T_Paper model = _paperDAL.GetPaperInfoByID(id);
            if (model != null)
            {
                model.IsDelete = true;
                _paperDAL.SaveChange();
            }
            else
            {
                throw new ArgumentNullException($"参数{nameof(id)}错误。");
            }
        }
        /// <summary>
        /// 修改试题信息
        /// </summary>
        /// <param name="model">要修改的对象</param>
        /// <exception cref="ArgumentException">验证不通过异常</exception>
        /// <exception cref="ArgumentNullException">参数错误异常</exception>
        public void EditPaperInfo(T_Paper model)
        {
            if (model != null)
            {
                string msg = string.Empty;
                if (Verification(model, ref msg))
                {
                    T_Paper dbModel = _paperDAL.GetPaperInfoByID(model.ID);
                    dbModel.Title = model.Title;
                    dbModel.IsEnable = model.IsEnable;
                    _paperDAL.SaveChange();
                }
                else
                {
                    throw new ArgumentException(msg);
                }
            }
            else
            {
                throw new ArgumentNullException($"参数{nameof(model)}不可以为空。");
            }
        }
        /// <summary>
        /// 根据条件获得试题信息
        /// </summary>
        /// <param name="title">试题标题</param>
        /// <param name="IsEnable">启用状态</param>
        /// <param name="pageM">分页对象</param>
        /// <returns>试题信息</returns>
        public List<V_Paper> GetPaperInfoByWhere(string title, bool? IsEnable, MPagingModel pageM)
        {
            List<V_Paper> listM = _paperDAL.GetPaperViewInfoByWhere(title, IsEnable, pageM);
            return listM;
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 验证模型
        /// </summary>
        /// <param name="model">要验证的模型</param>
        /// <param name="msg">提示信息</param>
        /// <returns>验证结果</returns>
        protected override bool Verification(T_Paper model, ref string msg)
        {
            if (string.IsNullOrEmpty(model.Title))
            {
                msg += "试题标题不可为空，";
            }
            return base.Verification(model, ref msg);
        }
        #endregion
    }
}
