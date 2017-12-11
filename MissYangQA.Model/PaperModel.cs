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
    /// 试卷状态枚举
    /// </summary>
    public enum PaperStateEnum : byte
    {
        [EnumShowName("尚未开始")]
        Ready,
        [EnumShowName("正在进行")]
        Beging,
        [EnumShowName("已经结束")]
        End
    }
    /// <summary>
    /// 试题修改模型
    /// </summary>
    public class EditPaperModel : T_Paper, IVerificationLoginModel
    {
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
