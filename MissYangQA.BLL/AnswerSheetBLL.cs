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
    public sealed class AnswerSheetBLL : BaseBLL<T_AnswerSheet>
    {
        #region 成员
        /// <summary>
        /// 答题卡数据访问对象
        /// </summary>
        private readonly AnswerSheetDAL _answerSheetDAL = new AnswerSheetDAL();
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
                    FK_AnswerShet = asM.ID,
                    ID = Guid.NewGuid()
                };
                asM.T_AnswerSheetDetails.Add(tempM);
            }
            _answerSheetDAL.Insert(asM);
            return asM.ID;
        }
        #endregion
    }
}
