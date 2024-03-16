using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerificationSystem.DB
{
    public class UserProduct
    {
        public long UserProductId { get; set; }
        public string UserId { get; set; }
        public long ProductId { get; set; }
    }
}