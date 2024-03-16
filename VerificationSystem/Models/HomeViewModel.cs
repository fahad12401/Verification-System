using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerificationSystem.Models
{
    public class HomeIndexViewModel
    {
        public double Total { get; set; }
        public double Open { get; set; }
        public double InProgress { get; set; }
        public double Partial { get; set; }
        public double QualityCheck { get; set; }
        public double Completed { get; set; }

      



        public double TotalCurrent { get; set; }
        public double OpenCurrent { get; set; }
        public double InProgressCurrent { get; set; }
        public double PartialCurrent { get; set; }
        public double QualityCheckCurrent { get; set; }
        public double CompletedCurrent { get; set; }

        




        public List<DB.Inquiry> InquiriesInProgress { get; set; }
        public List<DB.Inquiry> InquiriesCompleted { get; set; }
        public List<DB.Inquiry> InquiriesQualityCheck { get; set; }
    }
}