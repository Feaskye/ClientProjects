using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace CSVImport
{
    /// <summary>
    /// UploadFile 的摘要说明
    /// </summary>
    public class UploadFile : IHttpHandler
    {
        private int _maxLength = 5 * 1024 * 1024;
        private readonly string[] _imgExtensions = new string[] { ".csv"};

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
                string filename = string.Empty;
                var extension = Path.GetExtension(file.FileName);

                var fileDir = "/UploadFiles";
                filename = "/UploadFiles/csv" + extension;

                if (!Directory.Exists(context.Server.MapPath(fileDir)))
                {
                    Directory.CreateDirectory(context.Server.MapPath(fileDir));
                }
                var filePath = context.Server.MapPath(filename);
                file.SaveAs(filePath);
                var result = SaveData(filePath);
                if (!result)
                {
                    JsonResult(false, "导入数据库失败");
                    return;
                }
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

        private bool SaveData(string filePath)
        {
            var dt = OpenCSVFile(filePath);
            if (dt.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (DataRow dr in dt.Rows)
                {
                    var eANBarCode = dr["EANBarCode"] + "";
                    if (!string.IsNullOrWhiteSpace(eANBarCode))
                    {
                        int stockLevel;
                        if (int.TryParse(dr["StockLevel"] + "", out stockLevel))
                        {
                            sb.AppendFormat("update ETDB_productinfo set StockLevel={0} where EANBarCode='{1}';", dr["StockLevel"], eANBarCode);
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(sb.ToString()))
                {
                    return PublicSqlHelper.SqlHelper.ExecuteNonQuery(CommandType.Text, sb.ToString()) > 0;
                }

            }
            return false;
        }


        private DataTable OpenCSVFile(string filepath)
        {
            var mycsvdt=new DataTable();
            string strpath = filepath; //csv文件的路径
            try
            {
                int intColCount = 0;
                bool blnFlag = true;

                DataColumn mydc;
                DataRow mydr;

                string strline;
                string[] aryline;
                using (StreamReader mysr = new StreamReader(strpath, System.Text.Encoding.Default))
                {
                    while ((strline = mysr.ReadLine()) != null)
                    {
                        aryline = strline.Split(new char[] { ',' });
                        //给datatable加上列名
                        if (blnFlag)
                        {
                            blnFlag = false;
                            intColCount = aryline.Length;
                            int col = 0;
                            for (int i = 0; i < aryline.Length; i++)
                            {
                                col = i + 1;
                                mydc = new DataColumn(aryline[i]);
                                mycsvdt.Columns.Add(mydc);
                            }
                        }
                        //填充数据并加入到datatable中
                        mydr = mycsvdt.NewRow();
                        for (int i = 0; i < intColCount; i++)
                        {
                            mydr[i] = aryline[i];
                        }
                        mycsvdt.Rows.Add(mydr);
                    }
                }
                return mycsvdt;

            }
            catch (Exception e)
            {
                return mycsvdt;
            }
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