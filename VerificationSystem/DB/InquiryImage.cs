using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerificationSystem.DB
{
    public class InquiryImage
    {
        public long InquiryImageId { get; set; }
        public long InquiryId { get; set; }
        public string ImagePath { get; set; }
        public string VerificationType { get; set; }
        public string PersonType { get; set; }

        public long VerificationId { get; set; }

        public virtual Inquiry Inquiry { get; set; }

        [NotMapped]
        private byte[] image;

        [NotMapped]
        public byte[] Image
        {
            get
            {
                if (image != null)
                    return image;

                if (string.IsNullOrEmpty(ImagePath))
                    return null;

                return Extensions.HtmlExtensions.ReadImageFile(ImagePath);

            }
            set { image = value; }
        }


    }
}