using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CSVImport
{
    /// <summary>
    /// UploadFile 的摘要说明
    /// </summary>
    public class UploadFile : IHttpHandler
    {
        private int _maxLength = 5 * 1024 * 1024;
        private readonly string[] _imgExtensions = new string[] { ".csv", ".excel" };

        public void ProcessRequest(HttpContext context)
        {
            var files = context.Request.Files;
            if (files == null || files.Count == 0)
            {
                JsonResult(false, "请上传文件");
                return;
            }
            var file = files["file"];
            if (file != null)
            {
                if (file.ContentLength > _maxLength || file.ContentLength == 0)
                {
                    JsonResult(false, "文件大小必须大于0，小于5M");
                }
                string baseDir = "/UploadFiles/";
                string fileDir = baseDir;
                string filename = string.Empty;
                var waterMarkFileName = string.Empty;
                var extension = Path.GetExtension(file.FileName);

                fileDir = baseDir + DateTime.Now.ToString("yyyyMMdd") + "/";
                filename = fileDir + DateTime.Now.ToString("hhmmssfff") + Path.GetExtension(file.FileName);

                if (!Directory.Exists(context.Server.MapPath(fileDir)))
                {
                    Directory.CreateDirectory(context.Server.MapPath(fileDir));
                }
                var filePath = context.Server.MapPath(filename);
                file.SaveAs(context.Server.MapPath(filename));
                SaveData(filePath);
                JsonResult(true,data:filename);
                return;
            }

            JsonResult(false);
        }


        private void JsonResult(bool isSuccess, string message = "", string data = "")
        {
            HttpContext.Current.Response.Write("{\"success\":" + isSuccess.ToString().ToLower() + ",\"message\":\"" + message + "\",\"data\":\"" + data + "\"}");
            HttpContext.Current.Response.End();
        }

        private void SaveData(string filePath)
        {
            var dataset= ExcelHelper.ImportDataSetFromExcel(filePath, 0);
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}