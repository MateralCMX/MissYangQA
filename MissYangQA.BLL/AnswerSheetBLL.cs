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
    /// 答题卡业务类
    /// </summary>
    public sealed class AnswerSheetBLL : BaseBLL<AnswerSheetDAL, T_AnswerSheet>
    {
        #region 成员
        /// <summary>
        /// 答题卡明细数据访问对象
        /// </summary>
        private readonly AnswerSheetDetailsDAL _answerSheetDetailsDAL = new AnswerSheetDetailsDAL();
        /// <summary>
        /// 试题数据访问对象
        /// </summary>
        private readonly PaperDAL _paperDAL = new PaperDAL();
        /// <summary>
        /// 问题数据访问对象
        /// </summary>
        private readonly ProblemDAL _problemDAL = new ProblemDAL();
        /// <summary>
        /// 问题数据访问对象
        /// </summary>
        private readonly AnswerDAL _answerDAL = new AnswerDAL();
        #endregion
        #region 公共方法
        /// <summary>
        /// 提交答题卡
        /// </summary>
        /// <param name="inputM">答题卡对象</param>
        public Guid SubmitAnswerSheet(AnswerSheetSubmitModel inputM)
        {
            T_AnswerSheet asM = new T_AnswerSheet
            {
                FK_Class = inputM.ClassID,
                FK_Paper = inputM.PaperID,
                StudentName = inputM.StudentName,
                ID = Guid.NewGuid()
            };
            T_AnswerSheetDetails tempM;
            foreach (var answer in inputM.Answers)
            {
                tempM = new T_AnswerSheetDetails
                {
                    FK_Answer = answer,
                    FK_AnswerSheet = asM.ID,
                    ID = Guid.NewGuid()
                };
                asM.T_AnswerSheetDetails.Add(tempM);
            }
            _dal.Insert(asM);
            return asM.ID;
        }
        /// <summary>
        /// 根据唯一标识获得答题卡结果
        /// </summary>
        /// <param name="id">唯一标识</param>
        /// <returns>答题卡结果</returns>
        /// <exception cref="ArgumentException">参数错误</exception>
        public AnswerSheetResultModel GetAnswerSheetResultInfoByID(Guid id)
        {
            V_AnswerSheet dbAnSM = _dal.GetDBModelViewInfoByID(id);
            if (dbAnSM != null)
            {
                List<T_AnswerSheetDetails> dbAnSDMs = _answerSheetDetailsDAL.GetAnswerSheetDetailsInfoByAnswerSheetID(id);
                if (dbAnSDMs.Count > 0)
                {
                    AnswerSheetResultModel resM = new AnswerSheetResultModel(dbAnSM);
                    V_Paper paperM = _paperDAL.GetDBModelViewInfoByID(dbAnSDMs[0].T_Answer.T_Problem.T_Paper.ID);
                    List<V_Problem> problemMs = _problemDAL.GetProblemViewInfoByPaperID(paperM.ID);
                    List<V_Answer> answerMs;
                    AnswerSheetResultAnswerModel asraM;
                    AnswerSheetResultProblemModel asrpM;
                    AnswerSheetResultPaperModel asrapaM = new AnswerSheetResultPaperModel(paperM);
                    #region 组装问题
                    asrapaM.Problems = new List<AnswerSheetResultProblemModel>();
                    foreach (V_Problem item in problemMs)
                    {
                        asrpM = new AnswerSheetResultProblemModel(item);
                        #region 组装答案
                        answerMs = _answerDAL.GetAnswerViewInfoByProblemID(item.ID);
                        asrpM.Answers = new List<AnswerSheetResultAnswerModel>();
                        foreach (V_Answer answerM in answerMs)
                        {
                            asraM = new AnswerSheetResultAnswerModel(answerM);
                            asraM.IsSelect = (from m in dbAnSDMs
                                              where m.FK_Answer == answerM.ID
                                              select m).FirstOrDefault() != null;
                            asrpM.Answers.Add(asraM);
                        }
                        #endregion
                        asrapaM.Problems.Add(asrpM);
                    }
                    #endregion
                    resM.Paper = asrapaM;
                    return resM;
                }
                else
                {
                    throw new ArgumentException($"参数{nameof(id)}错误");
                }
            }
            else
            {
                throw new ArgumentException($"参数{nameof(id)}错误");
            }
        }
        #endregion
    }
}
