using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissYangQA.Model
{
    /// <summary>
    /// 更改班级排序输入模型
    /// </summary>
    public class ChangeClassListRankModel:IVerificationLoginModel
    {
        /// <summary>
        /// 班级列表ID
        /// </summary>
        public Guid ClassListID { get; set; }
        /// <summary>
        /// 目标班级列表ID
        /// </summary>
        public Guid TargetClassListID { get; set; }
        /// <summary>
        /// 登录用户唯一标识
        /// </summary>
        public Guid LoginUserID { get; set; }
        /// <summary>
        /// 登录用户Token
        /// </summary>
        public string LoginUserToken { get; set; }
    }
}
