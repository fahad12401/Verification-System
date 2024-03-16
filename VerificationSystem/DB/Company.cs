using System;
using System.Collections.Generic;

namespace VerificationSystem.DB
{
    public class Company
    {
        public Company()
        {
            Inquiries = new HashSet<Inquiry>();
            AppUsers = new HashSet<ApplicationUser>();
            Products = new HashSet<Product>();
        }

     
        public long CompanyId { get; set; }

        public string Name { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string WebAddress { get; set; }

        public DateTime? DisableDate { get; set; }

        public string Descriptions { get; set; }

        public string RecordBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime RecordAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        


        public virtual ICollection<ApplicationUser> AppUsers { get; set; }
        public virtual ICollection<Inquiry> Inquiries { get; set; }
        public virtual ICollection<Product> Products { get; set; }

    }
}