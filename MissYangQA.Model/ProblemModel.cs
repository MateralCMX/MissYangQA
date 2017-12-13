using MateralTools.MConvert;
using MateralTools.MEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissYangQA.Model
{
    /// <summary>
    /// 问题
    /// </summary>
    public class ProblemModel : V_Problem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="model">视图模型</param>
        public ProblemModel(V_Problem model)
        {
            ConvertManager.CopyProperties(model, this);
        }
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="listM"></param>
        public static List<ProblemModel> GetList(List<V_Problem> listM)
        {
            List<ProblemModel> resM = new List<ProblemModel>();
            foreach (V_Problem item in listM)
            {
                resM.Add(new ProblemModel(item));
            }
            return resM;
        }
        /// <summary>
        /// 答案
        /// </summary>
        public List<V_Answer> Answers { get; set; }
        /// <summary>
        /// 问题类型
        /// </summary>
        public ProblemTypeEnum ProblemTypeE
        {
            get
            {
                return (ProblemTypeEnum)ProblemType;
            }
        }
        /// <summary>
        /// 问题类型字符
        /// </summary>
        public string ProblemTypeStr
        {
            get
            {
                return EnumManager.GetShowName(ProblemTypeE);
            }
        }
    }
    /// <summary>
    /// 问题类型枚举
    /// </summary>
    public enum ProblemTypeEnum : byte
    {
        [EnumShowName("单选题")]
        Radio = 1,
        [EnumShowName("多选题")]
        Multiple = 2,
        [EnumShowName("问答题")]
        QA = 3,
        [EnumShowName("其他")]
        Other = 0
    }
    /// <summary>
    /// 问题修改模型
    /// </summary>
    public class EditProblemModel : T_Problem
    {
        /// <summary>
        /// 答案组
        /// </summary>
        public List<T_Answer> Answers { get; set; }
    }
}
