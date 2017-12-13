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
    public class PaperBLLTests
    {
        private PaperBLL _paperBLL = new PaperBLL();
        [TestMethod()]
        public void GetExamInfoByPaperIDTest()
        {
            PaperModel resM = _paperBLL.GetExamInfoByPaperID(Guid.Parse("6D08E87F-672A-4B20-AA55-EB35774F48A4"));
        }
    }
}