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
    /// <summary>
    /// 试题模型
    /// </summary>
    public class PaperModel : V_Paper
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="model">视图模型</param>
        public PaperModel(V_Paper model)
        {
            ConvertManager.CopyProperties(model, this);
        }
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="listM"></param>
        public static List<PaperModel> GetList(List<V_Paper> listM)
        {
            List<PaperModel> resM = new List<PaperModel>();
            foreach (V_Paper item in listM)
            {
                resM.Add(new PaperModel(item));
            }
            return resM;
        }
        /// <summary>
        /// 问题
        /// </summary>
        public List<ProblemModel> Problems { get; set; }
    }
}
