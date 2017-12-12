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
    public sealed class ClassBLL : BaseBLL<T_Class>
    {
        #region 成员
        /// <summary>
        /// 班级数据访问对象
        /// </summary>
        private readonly ClassDAL _classDAL = new ClassDAL();
        #endregion
        #region 公共方法
        /// <summary>
        /// 获得所有的班级信息
        /// </summary>
        /// <returns>所有班级的信息</returns>
        public List<V_Class> GetAllClassViewInfo()
        {
            List<V_Class> resM;
            resM = _classDAL.GetAllClassViewInfo();
            return resM;
        }
        /// <summary>
        /// 根据班级唯一标识获得班级视图信息
        /// </summary>
        /// <returns>查询结果</returns>
        public V_Class GetClassViewInfoByID(Guid id)
        {
            V_Class resM;
            resM = _classDAL.GetClassViewInfoByID(id);
            return resM;
        }
        /// <summary>
        /// 添加班级信息
        /// </summary>
        /// <param name="model">要添加的对象</param>
        public void AddClassInfo(T_Class model)
        {
            if (model != null)
            {
                string msg = string.Empty;
                if (Verification(model, ref msg))
                {
                    model.Rank = _classDAL.GetClassMaxRank() + 1;
                    _classDAL.Insert(model);
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
        public void DeleteClassInfo(Guid id)
        {
            T_Class model = _classDAL.GetClassInfoByID(id);
            if (model != null)
            {
                model.IsDelete = true;
                _classDAL.SaveChange();
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
        public void EditClassInfo(T_Class model)
        {
            if (model != null)
            {
                string msg = string.Empty;
                if (Verification(model, ref msg))
                {
                    T_Class dbModel = _classDAL.GetClassInfoByID(model.ID);
                    dbModel.Name = model.Name;
                    _classDAL.SaveChange();
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
        /// <param name="classID">要调换的班级唯一标识</param>
        /// <param name="targetClassID">目标班级唯一标识</param>
        public void ChangeClassRank(Guid classID,Guid targetClassID)
        {
            T_Class classM = _classDAL.GetClassInfoByID(classID);
            if (classM != null)
            {
                T_Class targetClassM = _classDAL.GetClassInfoByID(targetClassID);
                if (targetClassM != null)
                {
                    int tempRank = classM.Rank;
                    classM.Rank = targetClassM.Rank;
                    targetClassM.Rank = tempRank;
                    _classDAL.SaveChange();
                }
                else
                {
                    throw new ArgumentException($"参数{nameof(targetClassID)}错误。");
                }
            }
            else
            {
                throw new ArgumentException($"参数{nameof(classID)}错误。");
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
        protected override bool Verification(T_Class model, ref string msg)
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
