using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerificationSystem.DB
{
    public class Product
    {
        public Product()
        {
            Inquiries = new HashSet<Inquiry>();
        }
        public long ProductId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public long CompanyId { get; set; }

        public string RecordBy { get; set; }
        public DateTime RecordAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public Boolean Sms { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Inquiry> Inquiries { get; set; }
    }
}