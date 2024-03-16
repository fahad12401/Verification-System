using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace VerificationSystem.DB
{
    //public class General
    //{
    //    public DateTime? Disable { get; set; }
    //    public DateTime RecordAt { get; set; }
    //    public string RecordBy { get; set; }
    //    public DateTime? UpdateAt { get; set; }
    //    public string UpdateBy { get; set; }
    //}
    //public class CustomForm:General
    //{
    //    public CustomForm()
    //    {
    //        CustomTable = new HashSet<CustomTable>();
    //    }
    //    public long CustomFormId { get; set; }
    //    public string Name { get; set; }
    //    public string SpecifiedName { get; set; }
    //    public virtual ICollection<CustomTable> CustomTable { get; set; }
    //}

    //public class CustomTable:General
    //{
    //    public CustomTable()
    //    {
    //        CustomColumn = new HashSet<CustomColumn>();
    //    }
    //    public long CustomTableId { get; set; }
    //    public string Name { get; set; }
    //    public int AttributesCount { get; set; }
    //    public long CustomFormId { get; set; }
    //    public virtual CustomForm CustomForm { get; set; }
    //    public virtual ICollection<CustomColumn> CustomColumn { get; set; }
    //}

    //public class CustomColumn:General
    //{
    //    public long CustomColumnId { get; set; }
    //    public string Name { get; set; }
    //    public string AttributeLabel { get; set; }
    //    public string AttributeValueType { get; set; }
    //    public string ColumnValue { get; set; }
    //    public virtual CustomTable CustomTable { get; set; }
    //}

    //public class CustomControl:General
    //{
    //    public CustomControl()
    //    {
    //        ControlType = new HashSet<ControlType>();
    //    }
    //    public long CustomConrolId { get; set; }
    //    public string ControlName { get; set; }
    //    public string ControlType { get; set; }
    //    public virtual ICollection<ControlType> ControlType { get; set; }
    //}

    //public class ControlType : General
    //{
    //    public long ControlTypeId { get; set; }
    //    public string TypeName { get; set; }
    //    public long CustomConrolId { get; set; }
    //    public virtual CustomControl CustomControl { get; set; }
    //}

    public class CustomAppAnswer
    {
        [Key]
        public long CustomAppAnsId { get; set; }
        public string CNIC { get; set; }

        public long QuesCode { get; set; }

        public long AnsCode { get; set; }

        public string Answers { get; set; }
    }

    public class CustomQuestion
    {
        [Key]
        public long QuesCode { get; set; }

        public string Question { get; set; }
    
    }
    public class CustomAnswer
    {
        [Key]
        public long AnsCode { get; set; }
        
        public string Answers { get; set; }

        public long QuesCode { get; set; }

        public string AnswerType { get; set; }
    }
}