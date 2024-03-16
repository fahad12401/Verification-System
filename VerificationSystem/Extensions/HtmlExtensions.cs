using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Collections.Specialized;
using System.Net;

namespace VerificationSystem.Extensions
{
    public static class HtmlExtensions
    {

        public static string GetImageInBase64(string path)
        {
            string stringImage = "";
            try
            {
                if (File.Exists(path))
                {

                    byte[] bytes = ReadImageFile(path);
                    if (bytes == null)
                        stringImage = "";
                    else
                    {
                        stringImage = Convert.ToBase64String(bytes);
                    }

                    return stringImage;

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return stringImage;

        }
        public static string GetImageInBase64(this HtmlHelper html, string path)
        {
            string stringImage = "";
            try
            {
                if (File.Exists(path))
                {

                    byte[] bytes = ReadImageFile(@"C:\Users\fahadali\Pictures\fahad.jpg");
                    if (bytes == null)
                        stringImage = "";
                    else
                        stringImage = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(bytes));


                    return stringImage;

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return stringImage;

        }

        public static string GetImageInBase64Server(this HtmlHelper html, string path)
        {
            string stringImage = "";
            try
            {


                byte[] bytes = ReadImageFileFromServer(path);
                if (bytes == null)
                    stringImage = "";
                else
                    stringImage = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(bytes));


                return stringImage;



            }
            catch (Exception ex)
            {

                throw ex;
            }
            return stringImage;

        }

        public static byte[] ReadImageFileFromServer(string imageLocation)
        {
            string imageServer = System.Configuration.ConfigurationManager.AppSettings["imageServer"];

            imageLocation = imageLocation.Replace(@"D:\Images EVS", imageServer);
            var webClient = new WebClient();
            byte[] imageBytes = webClient.DownloadData(imageLocation);
            return imageBytes;
        }
        public static byte[] ReadImageFile(string imageLocation)
        {
            byte[] imageData = null;
            FileInfo fileInfo = new FileInfo(imageLocation);
            if (!fileInfo.Exists)
            {
                return null;
            }
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);
            br.Close();
            fs.Close();
            return imageData;
        }

        public static byte[] ConvertToByteHttpPostedFileBase(HttpPostedFileBase file)
        {
            byte[] imageByte = null;
            BinaryReader rdr = new BinaryReader(file.InputStream);
            imageByte = rdr.ReadBytes((int)file.ContentLength);
            return imageByte;
        }

        public static string GetBytesInBase64(this HtmlHelper html, byte[] image)
        {
            var img = "";
            if (image == null)
                img = "";
            else
                img = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image));

            return img;

        }

        public static string ToYesNo(this HtmlHelper html, bool value)
        {
            if (value)
                return "Yes";
            else
                return "No";
        }
        public static string ToYesNo(this HtmlHelper html, bool? value)
        {
            if (value.HasValue)
            {
                if (value.Value)
                    return "Yes";
                else
                    return "No";
            }
            else
            {
                return "";
            }
        }

        public static string ToDateTimeFormat(this HtmlHelper html, DateTime? dateTime, string format = null)
        {
            if (dateTime == null)
            {
                return "N/D";
            }
            if (string.IsNullOrEmpty(format))
            {
                return dateTime.Value.ToString(format);
            }
            else
            {
                return dateTime.Value.ToString("dd-MM-yyyy HH-mm-ss");
            }
        }

        public static string GetQueryStringWithParameter(this HtmlHelper html, string param, string value)
        {
            // NameValueCollection qs = new NameValueCollection(html.ViewContext.HttpContext.Request.QueryString);

            var qs = HttpUtility.ParseQueryString(html.ViewContext.HttpContext.Request.QueryString.ToString());

            if (string.IsNullOrEmpty(qs.Get(param)))
            {
                qs.Add(param, value);
            }
            else
            {
                qs.Set(param, value);
            }
            return qs.ToString();

        }

    }
}