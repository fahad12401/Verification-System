using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerificationSystem.DB
{
    public class ErrorLog
    {
        public long ErrorLogId { get; set; }
        public string Error  { get; set; }
        public string Detail { get; set; }
        
        public string ActionName { get; set; }
        public DateTime RecortAt { get; set; }
        public string UserId { get; set; }

    }
}