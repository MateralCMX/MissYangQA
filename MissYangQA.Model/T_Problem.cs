//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MissYangQA.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_Problem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_Problem()
        {
            this.T_Answer = new HashSet<T_Answer>();
        }
    
        public System.Guid ID { get; set; }
        public System.Guid FK_Paper { get; set; }
        public string Contents { get; set; }
        public int Score { get; set; }
        public bool IsDelete { get; set; }
        public System.DateTime CreateTime { get; set; }
    
        public virtual T_Paper T_Paper { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_Answer> T_Answer { get; set; }
    }
}
