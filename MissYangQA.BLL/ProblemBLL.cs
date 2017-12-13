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
    /// 问题业务类
    /// </summary>
    public sealed class ProblemBLL : BaseBLL<T_Problem>
    {
        #region 成员
        /// <summary>
        /// 问题数据访问对象
        /// </summary>
        private readonly ProblemDAL _problemDAL = new ProblemDAL();
        /// <summary>
        /// 答案数据访问对象
        /// </summary>
        private readonly AnswerDAL _answerDAL = new AnswerDAL();
        #endregion
        #region 公共方法
        /// <summary>
        /// 根据问题唯一标识获得问题视图信息
        /// </summary>
        /// <param name="id">问题唯一标识</param>
        /// <returns>查询结果</returns>
        public ProblemModel GetProblemViewInfoByID(Guid id)
        {
            ProblemModel resM;
            V_Problem model = _problemDAL.GetProblemViewInfoByID(id);
            resM = new ProblemModel(model);
            resM.Answers = _answerDAL.GetAnswerViewInfoByProblemID(resM.ID);
            return resM;
        }
        /// <summary>
        /// 添加问题信息
        /// </summary>
        /// <param name="model">要添加的对象</param>
        /// <param name="answers">答案对象</param>
        /// <exception cref="ArgumentException">参数错误</exception>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public void AddProblemInfo(T_Problem model, List<T_Answer> answers)
        {
            if (model != null)
            {
                string msg = string.Empty;
                if (Verification(model, ref msg))
                {
                    if (answers.Where(m => !string.IsNullOrEmpty(m.Contents)).Count() == answers.Count)
                    {
                        if (answers.Where(m => m.IsCorrect).Count() > 0)
                        {
                            model = BaseDAL.GetBeforeInsertModel(model);
                            model.CreateTime = DateTime.Now;
                            model.IsDelete = false;
                            T_Answer tempAnswerM;
                            foreach (T_Answer item in answers)
                            {
                                tempAnswerM = BaseDAL.GetBeforeInsertModel(item);
                                tempAnswerM.FK_Problem = model.ID;
                                tempAnswerM.CreateTime = DateTime.Now;
                                tempAnswerM.IsDelete = false;
                                model.T_Answer.Add(tempAnswerM);
                            }
                            int Count = model.T_Answer.Count;
                            if (Count > 1)
                            {
                                Count = model.T_Answer.Where(m => m.IsCorrect).Count();
                                if (Count == 1)
                                {
                                    model.ProblemType = (byte)ProblemTypeEnum.Radio;
                                }
                                else if (Count > 1)
                                {
                                    model.ProblemType = (byte)ProblemTypeEnum.Multiple;
                                }
                            }
                            else
                            {
                                model.ProblemType = (byte)ProblemTypeEnum.QA;
                            }
                            _problemDAL.Insert(model);
                        }
                        else
                        {
                            throw new ArgumentException("请指定一个正确答案");
                        }
                    }
                    else
                    {
                        throw new ArgumentNullException("答案内容不可为空");
                    }
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
        /// 删除问题信息
        /// </summary>
        /// <param name="id">问题唯一标识</param>
        /// <exception cref="ArgumentNullException">参数错误异常</exception>
        public void DeleteProblemInfo(Guid id)
        {
            T_Problem model = _problemDAL.GetProblemInfoByID(id);
            if (model != null)
            {
                model.IsDelete = true;
                _problemDAL.SaveChange();
            }
            else
            {
                throw new ArgumentNullException($"参数{nameof(id)}错误。");
            }
        }
        /// <summary>
        /// 修改问题信息
        /// </summary>
        /// <param name="model">要修改的对象</param>
        /// <exception cref="ArgumentException">验证不通过异常</exception>
        /// <exception cref="ArgumentNullException">参数错误异常</exception>
        public void EditProblemInfo(T_Problem model, List<T_Answer> answers)
        {
            if (model != null)
            {
                string msg = string.Empty;
                if (Verification(model, ref msg))
                {
                    if (answers.Where(m => !string.IsNullOrEmpty(m.Contents)).Count() == answers.Count)
                    {
                        if (answers.Where(m => m.IsCorrect).Count() > 0)
                        {
                            T_Problem dbModel = _problemDAL.GetProblemInfoByID(model.ID);
                            if (dbModel != null)
                            {
                                dbModel.Contents = model.Contents;
                                dbModel.Score = model.Score;
                                Guid[] answersIDs = (from m in answers
                                                     where m.ID != Guid.Empty
                                                     select m.ID).ToArray();
                                T_Answer tempAnswerM;
                                #region 删除的答案
                                List<T_Answer> deleteAnswers = dbModel.T_Answer.Where(m => !answersIDs.Contains(m.ID)).ToList();
                                foreach (T_Answer item in deleteAnswers)
                                {
                                    item.IsDelete = true;
                                }
                                #endregion
                                #region 修改的答案
                                List<T_Answer> updateAnswers = dbModel.T_Answer.Where(m => answersIDs.Contains(m.ID)).ToList();
                                for (int i = 0; i < updateAnswers.Count; i++)
                                {
                                    tempAnswerM = answers.Where(m => m.ID == updateAnswers[i].ID).FirstOrDefault();
                                    if (tempAnswerM != null)
                                    {
                                        updateAnswers[i].Contents = tempAnswerM.Contents;
                                        updateAnswers[i].IsCorrect = tempAnswerM.IsCorrect;
                                    }
                                }
                                #endregion
                                #region 添加的答案
                                List<T_Answer> addAnswers = answers.Where(m => m.ID == Guid.Empty).ToList();
                                foreach (T_Answer item in addAnswers)
                                {
                                    tempAnswerM = BaseDAL.GetBeforeInsertModel(item);
                                    tempAnswerM.FK_Problem = model.ID;
                                    tempAnswerM.CreateTime = DateTime.Now;
                                    tempAnswerM.IsDelete = false;
                                    dbModel.T_Answer.Add(tempAnswerM);
                                }
                                #endregion
                                int Count = dbModel.T_Answer.Count;
                                if (Count > 1)
                                {
                                    Count = dbModel.T_Answer.Where(m => m.IsCorrect).Count();
                                    if (Count == 1)
                                    {
                                        dbModel.ProblemType = (byte)ProblemTypeEnum.Radio;
                                    }
                                    else if (Count > 1)
                                    {
                                        dbModel.ProblemType = (byte)ProblemTypeEnum.Multiple;
                                    }
                                }
                                else
                                {
                                    dbModel.ProblemType = (byte)ProblemTypeEnum.QA;
                                }
                                _problemDAL.SaveChange();
                            }
                            else
                            {
                                throw new ArgumentException($"参数{nameof(model)}错误。");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("请指定一个正确答案");
                        }
                    }
                    else
                    {
                        throw new ArgumentNullException("答案内容不可为空");
                    }
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
        /// 根据试题ID获取问题信息
        /// </summary>
        /// <param name="paperID">试题ID</param>
        /// <returns></returns>
        public List<ProblemModel> GetProblemInfoByPaperID(Guid paperID)
        {
            List<V_Problem> listM = _problemDAL.GetProblemViewInfoByPaperID(paperID);
            List<ProblemModel> resM = ProblemModel.GetList(listM);
            foreach (ProblemModel item in resM)
            {
                item.Answers = _answerDAL.GetAnswerViewInfoByProblemID(item.ID);
            }
            return resM;
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 验证模型
        /// </summary>
        /// <param name="model">要验证的模型</param>
        /// <param name="msg">提示信息</param>
        /// <returns>验证结果</returns>
        protected override bool Verification(T_Problem model, ref string msg)
        {
            if (string.IsNullOrEmpty(model.Contents))
            {
                msg += "问题不可为空，";
            }
            return base.Verification(model, ref msg);
        }
        #endregion
    }
}
