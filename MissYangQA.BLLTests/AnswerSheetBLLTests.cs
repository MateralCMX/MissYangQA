using MateralTools.MConvert;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MissYangQA.BLL;
using MissYangQA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissYangQA.BLL.Tests
{
    [TestClass()]
    public class AnswerSheetBLLTests
    {
        private readonly AnswerSheetBLL _answerSheetBLL = new AnswerSheetBLL();
        [TestMethod()]
        public void GetAnswerSheetResultInfoByIDTest()
        {
            AnswerSheetResultModel resM = _answerSheetBLL.GetAnswerSheetResultInfoByID(Guid.Parse("4A4602E2-C068-4C99-B483-06D849F4472A"));
            string json = ConvertManager.ModelToJson(resM);
        }
    }
}