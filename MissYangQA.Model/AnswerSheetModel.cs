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
}
