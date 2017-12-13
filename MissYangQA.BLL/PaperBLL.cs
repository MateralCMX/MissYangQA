using MateralTools.Base;
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
        /// 试题数据访问对象
        /// </summary>
        private readonly PaperDAL _paperDAL = new PaperDAL();
        private readonly ProblemBLL _problemBLL = new ProblemBLL();
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
        /// <summary>
        /// 获得所有启用的试题信息
        /// </summary>
        /// <returns>试题信息</returns>
        public List<V_Paper> GetAllEnablePaperInfo()
        {
            List<V_Paper> listM = _paperDAL.GetAllEnablePaperInfo();
            return listM;
        }
        /// <summary>
        /// 根据试题ID获得试卷信息
        /// </summary>
        /// <param name="id">试题ID</param>
        /// <returns>试卷信息</returns>
        public PaperModel GetExamInfoByPaperID(Guid id)
        {
            V_Paper DBPaperM = _paperDAL.GetPaperViewInfoByID(id);
            if (DBPaperM != null)
            {
                PaperModel paperM = new PaperModel(DBPaperM);
                List<ProblemModel> listM = _problemBLL.GetProblemInfoByPaperID(id);
                paperM.Problems = listM;
                UpsetProblem(paperM);
                return paperM;
            }
            else
            {
                throw new ArgumentException($"参数{nameof(id)}错误");
            }
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 打乱问题顺序
        /// </summary>
        /// <param name="paperM"></param>
        private void UpsetProblem(PaperModel paperM)
        {
            Random rd = new Random();
            List<int> upsetIndex = new List<int>();
            for (int i = 0; i < paperM.Problems.Count; i++)
            {
                upsetIndex.Add(i);
            }
            int Index;
            int TrueIndex;
            List<ProblemModel> problems = new List<ProblemModel>();
            ProblemModel tempM;
            while (upsetIndex.Count > 0)
            {
                Index = rd.Next(0, upsetIndex.Count);
                TrueIndex = upsetIndex[Index];
                tempM = paperM.Problems[TrueIndex];
                UpsetAnswer(tempM);
                problems.Add(tempM);
                upsetIndex.RemoveAt(Index);
            }
            paperM.Problems = problems;
        }
        /// <summary>
        /// 打乱答案顺序
        /// </summary>
        /// <param name="paperM"></param>
        private void UpsetAnswer(ProblemModel problemM)
        {
            Random rd = new Random();
            List<int> upsetIndex = new List<int>();
            for (int i = 0; i < problemM.Answers.Count; i++)
            {
                upsetIndex.Add(i);
            }
            int Index;
            int TrueIndex;
            List<V_Answer> answers = new List<V_Answer>();
            V_Answer tempM;
            while (upsetIndex.Count > 0)
            {
                Index = rd.Next(0, upsetIndex.Count);
                TrueIndex = upsetIndex[Index];
                tempM = problemM.Answers[TrueIndex];
                tempM.IsCorrect = false;
                answers.Add(tempM);
                upsetIndex.RemoveAt(Index);
            }
            problemM.Answers = answers;
        }
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
