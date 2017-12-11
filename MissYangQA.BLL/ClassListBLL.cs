using MateralTools.MEncryption;
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
    /// 班级业务类
    /// </summary>
    public sealed class ClassListBLL : BaseBLL<T_ClassList>
    {
        #region 成员
        /// <summary>
        /// 班级数据访问对象
        /// </summary>
        private readonly ClassListDAL _classListDAL = new ClassListDAL();
        #endregion
        #region 公共方法
        /// <summary>
        /// 获得所有的班级信息
        /// </summary>
        /// <returns>所有班级的信息</returns>
        public List<V_ClassList> GetAllClassListViewInfo()
        {
            List<V_ClassList> resM;
            resM = _classListDAL.GetAllClassListViewInfo();
            return resM;
        }
        /// <summary>
        /// 根据班级唯一标识获得班级视图信息
        /// </summary>
        /// <returns>查询结果</returns>
        public V_ClassList GetClassListViewInfoByID(Guid id)
        {
            V_ClassList resM;
            resM = _classListDAL.GetClassListViewInfoByID(id);
            return resM;
        }
        /// <summary>
        /// 添加班级信息
        /// </summary>
        /// <param name="model">要添加的对象</param>
        public void AddClassListInfo(T_ClassList model)
        {
            if (model != null)
            {
                string msg = string.Empty;
                if (Verification(model, ref msg))
                {
                    model.Rank = _classListDAL.GetClassListMaxRank() + 1;
                    _classListDAL.Insert(model);
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
        /// 删除班级信息
        /// </summary>
        /// <param name="id">班级唯一标识</param>
        /// <exception cref="ArgumentNullException">参数错误异常</exception>
        public void DeleteClassListInfo(Guid id)
        {
            T_ClassList model = _classListDAL.GetClassListInfoByID(id);
            if (model != null)
            {
                model.IsDelete = true;
                _classListDAL.SaveChange();
            }
            else
            {
                throw new ArgumentNullException($"参数{nameof(id)}错误。");
            }
        }
        /// <summary>
        /// 修改班级信息
        /// </summary>
        /// <param name="model">要修改的对象</param>
        /// <exception cref="ArgumentException">验证不通过异常</exception>
        /// <exception cref="ArgumentNullException">参数错误异常</exception>
        public void EditClassListInfo(T_ClassList model)
        {
            if (model != null)
            {
                string msg = string.Empty;
                if (Verification(model, ref msg))
                {
                    T_ClassList dbModel = _classListDAL.GetClassListInfoByID(model.ID);
                    dbModel.Name = model.Name;
                    _classListDAL.SaveChange();
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
        /// 调换班级位序
        /// </summary>
        /// <param name="classListID">要调换的班级唯一标识</param>
        /// <param name="targetClassListID">目标班级唯一标识</param>
        public void ChangeClassListRank(Guid classListID,Guid targetClassListID)
        {
            T_ClassList classListM = _classListDAL.GetClassListInfoByID(classListID);
            if (classListM != null)
            {
                T_ClassList targetClassListM = _classListDAL.GetClassListInfoByID(targetClassListID);
                if (targetClassListM != null)
                {
                    int tempRank = classListM.Rank;
                    classListM.Rank = targetClassListM.Rank;
                    targetClassListM.Rank = tempRank;
                    _classListDAL.SaveChange();
                }
                else
                {
                    throw new ArgumentException($"参数{nameof(targetClassListID)}错误。");
                }
            }
            else
            {
                throw new ArgumentException($"参数{nameof(classListID)}错误。");
            }
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 验证模型
        /// </summary>
        /// <param name="model">要验证的模型</param>
        /// <param name="msg">提示信息</param>
        /// <returns>验证结果</returns>
        protected override bool Verification(T_ClassList model, ref string msg)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                msg += "班级名不可为空，";
            }
            return base.Verification(model, ref msg);
        }
        #endregion
    }
}
