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
    public class ClassListBLLTests
    {
        /// <summary>
        /// 班级业务对象
        /// </summary>
        private readonly ClassListBLL _classListBLL = new ClassListBLL();
        /// <summary>
        /// 获取所有班级信息
        /// </summary>
        [TestMethod()]
        public void GetAllClassListInfoTest()
        {
            try
            {
                List<T_ClassList> listM = _classListBLL.GetAllClassListInfo();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}