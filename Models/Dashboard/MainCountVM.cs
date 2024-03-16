using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerificationSystem.Models.Dashboard
{
    public class MainCountVM
    {
        public int OpenCount { get; set; } 
        public int InProgressCount { get; set; }
        public int HoldCount { get; set; }
        public int ReturnCount { get; set; }

        public int PartialCompletedCount { get; set; }
        public int QCCount { get; set; }
        public int CompletedCount { get; set; }
        public int AllCount { get; set; }

        public int FIOCount { get; set; }
        public int CompanyCount { get; set; }
        public int CustomerBranchCount { get; set; }
        public int ProductCount { get; set; }

        public int AllVerificationCount { get; set; } 

    }
}