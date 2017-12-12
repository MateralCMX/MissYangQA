using MateralTools.MConvert;
using MissYangQA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MissYangQA.DAL
{
    /// <summary>
    /// 数据操作类父类
    /// </summary>
    public class BaseDAL
    {
        /// <summary>
        /// 数据连接对象
        /// </summary>
        protected MissYangQADBEntities _DB;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <exception cref="Exception"></exception>
        public BaseDAL()
        {
            try
            {
                _DB = new MissYangQADBEntities();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 保存更改
        /// </summary>
        public void SaveChange()
        {
            _DB.SaveChanges();
        }
        /// <summary>
        /// 添加之前
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        protected T BeforeInsert<T>(T model)
        {
            return GetBeforeInsertModel(model);
        }
        /// <summary>
        /// 添加之前
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T GetBeforeInsertModel<T>(T model)
        {
            T addModel = default(T);
            Type tType = typeof(T);
            ConstructorInfo[] cis = tType.GetConstructors();
            if (cis.Length > 0 && cis[0].GetParameters().Length == 0)
            {
                addModel = (T)cis[0].Invoke(new object[0]);
                ConvertManager.CopyProperties(model, addModel);
                PropertyInfo pi = tType.GetProperty("ID");
                if (pi != null)
                {
                    Guid piGuid = pi.PropertyType.GUID;
                    if (piGuid == typeof(Guid).GUID)
                    {
                        pi.SetValue(addModel, Guid.NewGuid());
                    }
                }
            }
            return addModel;
        }
    }
}
