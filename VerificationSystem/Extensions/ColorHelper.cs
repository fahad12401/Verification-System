using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace VerificationSystem.Extensions
{
    public static class ColorHelper
    {


        public const string Completed = constant.Status.Completed;  
        public const string InProgress = constant.Status.InProgress;  
        public const string PartialComplete = constant.Status.PartialComplete;
        public const string New = constant.Status.New;
        public const string QualityCheck = constant.Status.QualityCheck;
       

        public const string CompletedColor = @"1, 255, 111";
        public const string InProgressColor = @"243, 157, 18";
        public const string PartialCompleteColor = @"0, 192, 239";
        public const string NewColor = @"255, 133, 27";
        public const string QualityCheckColor = @"221, 75, 57";
        public const string HoldColor = @"96, 92, 168";
        public const string ReturnColor = @"0, 166, 90";


        public const string Verified = constant.Outcome.Verified;
        public const string UnVerified = constant.Outcome.UnVerified;
        public const string VerifiedColor = @"0, 192, 239";
         public const string UnVerifiedColor = @"221, 75, 57";
         

        private static readonly ReadOnlyCollection<ColorHelperVM> _statusColorsList =
          new ReadOnlyCollection<ColorHelperVM>(new List<ColorHelperVM>()
          {


              new ColorHelperVM { Title = Completed, Color = CompletedColor },
                new ColorHelperVM { Title = InProgress, Color = InProgressColor },
                  new ColorHelperVM { Title = PartialComplete, Color = PartialCompleteColor },
                    new ColorHelperVM { Title = New, Color = NewColor },
                      new ColorHelperVM { Title = QualityCheck, Color = QualityCheckColor },
                        

          });
         
        
        public static ReadOnlyCollection<ColorHelperVM> StatusColorsList
        {
            get { return _statusColorsList; }
        }



        private static readonly ReadOnlyCollection<ColorHelperVM> _outcomeColorsList =
          new ReadOnlyCollection<ColorHelperVM>(new List<ColorHelperVM>()
          {


              new ColorHelperVM { Title = Verified, Color = VerifiedColor },
                new ColorHelperVM { Title = UnVerified, Color = UnVerifiedColor },
                  

          });


        public static ReadOnlyCollection<ColorHelperVM> OutcomeColorsList
        {
            get { return _outcomeColorsList; }
        }

        public class ColorHelperVM
        {
            public string Title { get; set; }
            public string Color { get; set; }

        }

    }
}