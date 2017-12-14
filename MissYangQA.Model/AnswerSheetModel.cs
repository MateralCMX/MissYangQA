using MateralTools.MConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissYangQA.Model
{
    /// <summary>
    /// 答题卡提交模型
    /// </summary>
    public class AnswerSheetSubmitModel
    {
        /// <summary>
        /// 试题ID
        /// </summary>
        public Guid PaperID { get; set; }
        /// <summary>
        /// 班级ID
        /// </summary>
        public Guid ClassID { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }
        /// <summary>
        /// 答案组
        /// </summary>
        public List<Guid> Answers { get; set; }
    }
    /// <summary>
    /// 答题卡结果模型
    /// </summary>
    public class AnswerSheetResultModel : V_AnswerSheet
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="model">视图模型</param>
        public AnswerSheetResultModel(V_AnswerSheet model)
        {
            ConvertManager.CopyProperties(model, this);
        }
        /// <summary>
        /// 答题卡结果试题
        /// </summary>
        public AnswerSheetResultPaperModel Paper { get; set; }
    }
    /// <summary>
    /// 答题卡结果试题模型
    /// </summary>
    public class AnswerSheetResultPaperModel
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="model">视图模型</param>
        public AnswerSheetResultPaperModel(V_Paper model)
        {
            ConvertManager.CopyProperties(model, this);
        }
        /// <summary>
        /// 问题总数
        /// </summary>
        public int ProblemCount { get; set; }
        /// <summary>
        /// 总分
        /// </summary>
        public int SumScore { get; set; }
        /// <summary>
        /// 问题内容
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 得分
        /// </summary>
        public int Score
        {
            get
            {
                int res = 0;
                _correctCount = 0;
                _errorCount = 0;
                foreach (var item in Problems)
                {
                    if (item.IsCorrect)
                    {
                        res += item.Score;
                        _correctCount++;
                    }
                    else
                    {
                        _errorCount++;
                    }
                }
                return res;
            }
        }
        private int _correctCount = 0;
        /// <summary>
        /// 正确数量
        /// </summary>
        public int CorrectCount
        {
            get
            {
                return _correctCount;
            }
        }
        private int _errorCount = 0;
        /// <summary>
        /// 错误数量
        /// </summary>
        public int ErrorCount
        {
            get
            {
                return _errorCount;
            }
        }
        /// <summary>
        /// 答题卡结果问题集
        /// </summary>
        public List<AnswerSheetResultProblemModel> Problems { get; set; }
    }
    /// <summary>
    /// 答题卡结果问题模型
    /// </summary>
    public class AnswerSheetResultProblemModel
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="model">视图模型</param>
        public AnswerSheetResultProblemModel(V_Problem model)
        {
            ConvertManager.CopyProperties(model, this);
        }
        public string Contents { get; set; }
        public int Score { get; set; }
        public byte ProblemType { get; set; }
        /// <summary>
        /// 是否正确
        /// </summary>
        public bool IsCorrect
        {
            get
            {
                foreach (var m in Answers)
                {
                    if ((m.IsSelect && !m.IsCorrect) || (!m.IsCorrect && m.IsCorrect))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        /// <summary>
        /// 答题卡结果答案模型
        /// </summary>
        public List<AnswerSheetResultAnswerModel> Answers { get; set; }
    }
    /// <summary>
    /// 答题卡结果答案模型
    /// </summary>
    public class AnswerSheetResultAnswerModel
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="model">视图模型</param>
        public AnswerSheetResultAnswerModel(V_Answer model)
        {
            ConvertManager.CopyProperties(model, this);
        }
        public string Contents { get; set; }
        public bool IsCorrect { get; set; }
        /// <summary>
        /// 已选择
        /// </summary>
        public bool IsSelect { get; set; }
    }
}
