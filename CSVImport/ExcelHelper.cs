using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Data;
using NPOI.SS.UserModel;
using System.Text;

namespace CSVImport
{
    public class ExcelHelper
    {

        #region 导入


        /// <summary> 由Excel导入DataTable </summary>    
        /// <param name="excelFileStream">Excel文件流</param>    
        /// <param name="sheetName">Excel工作表名称</param>   
        /// <param name="headerRowIndex">Excel表头行索引</param
        /// <returns>DataTable</returns>    
        public static DataTable ImportDataTableFromExcel(Stream excelFileStream, string sheetName, int headerRowIndex)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(excelFileStream);
            var sheet = workbook.GetSheet(sheetName);
            DataTable table = new DataTable();
            var headerRow = sheet.GetRow(headerRowIndex);
            int cellCount = headerRow.LastCellNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue); table.Columns.Add(column);
            }
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                    dataRow[j] = row.GetCell(j).ToString();
            }
            excelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }


        /// <summary> 由Excel导入DataTable </summary>   
        /// <param name="excelFilePath">Excel文件路径，为物理路径。</param>    
        /// <param name="sheetName">Excel工作表名称</param>    
        /// <param name="headerRowIndex">Excel表头行索引</param>    
        /// <returns>DataTable</returns>    
        public static DataTable ImportDataTableFromExcel(string excelFilePath, string sheetName, int headerRowIndex)
        {
            using (FileStream stream = System.IO.File.OpenRead(excelFilePath))
            {
                return ImportDataTableFromExcel(stream, sheetName, headerRowIndex);
            }
        }


        /// <summary> 由Excel导入DataTable </summary>   
        /// <param name="excelFileStream">Excel文件流</param> 
        /// <param name="sheetName">Excel工作表索引</param>    
        /// <param name="headerRowIndex">Excel表头行索引</param>   
        /// <returns>DataTable</returns>  
        public static DataTable ImportDataTableFromExcel(Stream excelFileStream, int sheetIndex, int headerRowIndex)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(excelFileStream);
            var sheet = workbook.GetSheetAt(sheetIndex);
            DataTable table = new DataTable();
            var headerRow = sheet.GetRow(headerRowIndex);
            int cellCount = headerRow.LastCellNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                if (headerRow.GetCell(i) == null || headerRow.GetCell(i).StringCellValue.Trim() == "")
                {
                    // 如果遇到第一个空列，则不再继续向后读取     
                    break;
                }

                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            int realCellCount = table.Columns.Count;

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null || row.GetCell(0) == null || row.GetCell(0).ToString().Trim() == "")
                {                // 如果遇到第一个空行，则不再继续向后读取              
                    break;
                }
                DataRow dataRow = table.NewRow();
                for (int j = row.FirstCellNum; j < realCellCount; j++)
                {
                    dataRow[j] = row.GetCell(j);
                } table.Rows.Add(dataRow);
            }
            excelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }


        /// <summary>由Excel导入DataTable    
        /// </summary>   
        /// <param name="excelFilePath">Excel文件路径，为物理路径。</param> 
        /// <param name="sheetName">Excel工作表索引</param>   
        /// <param name="headerRowIndex">Excel表头行索引</param>    
        /// <returns>DataTable</returns>    
        public static DataTable ImportDataTableFromExcel(string excelFilePath, int sheetIndex, int headerRowIndex)
        {
            using (FileStream stream = System.IO.File.OpenRead(excelFilePath))
            {
                return ImportDataTableFromExcel(stream, sheetIndex, headerRowIndex);
            }
        }



        /// <summary>    
        /// 由Excel导入DataSet，如果有多个工作表，则导入多个DataTable   
        /// </summary> 
        /// <param name="excelFileStream">Excel文件流</param>  
        /// <param name="headerRowIndex">Excel表头行索引</param>  
        /// <returns>DataSet</returns>   
        public static DataSet ImportDataSetFromExcel(Stream excelFileStream, int headerRowIndex)
        {
            DataSet ds = new DataSet();
            HSSFWorkbook workbook = new HSSFWorkbook(excelFileStream);
            for (int a = 0, b = workbook.NumberOfSheets; a < b; a++)
            {
                var sheet = workbook.GetSheetAt(a);
                DataTable table = new DataTable();
                var headerRow = sheet.GetRow(headerRowIndex);
                int cellCount = headerRow.LastCellNum;
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    if (headerRow.GetCell(i) == null || headerRow.GetCell(i).StringCellValue.Trim() == "")
                    {
                        // 如果遇到第一个空列，则不再继续向后读取                 
                        cellCount = i + 1;
                        break;
                    }
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);                                  table.Columns.Add(column);
                }
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i); if (row == null || row.GetCell(0) == null || row.GetCell(0).ToString().Trim() == "")
                    {
                        // 如果遇到第一个空行，则不再继续向后读取         
                        break;
                    }
                    DataRow dataRow = table.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            dataRow[j] = row.GetCell(j).ToString();
                        }
                    } table.Rows.Add(dataRow);
                }
                ds.Tables.Add(table);
            }
            excelFileStream.Close();
            workbook = null;
            return ds;
        }


        /// <summary>    
        /// 由Excel导入DataSet，如果有多个工作表，则导入多个DataTable    
        /// </summary>   
        /// <param name="excelFilePath">Excel文件路径，为物理路径。</param>    
        /// <param name="headerRowIndex">Excel表头行索引</param>   
        ///<returns>DataSet</returns> 
        public static DataSet ImportDataSetFromExcel(string excelFilePath, int headerRowIndex)    {       
            using (FileStream stream = System.IO.File.OpenRead(excelFilePath))   
            {       
                return ImportDataSetFromExcel(stream, headerRowIndex);  
            }    
        }

        #endregion

        /// <summary> 
        /// 将Excel的列索引转换为列名，列索引从0开始，列名从A开始。如第0列为A，第1列为B...   
        /// </summary>    
        /// <param name="index">列索引</param> 
        /// /// <returns>列名，如第0列为A，第1列为B...</returns>   
        public static string ConvertColumnIndexToColumnName(int index)   
        {      
            index = index + 1;     
            int system = 26;       
            char[] digArray = new char[100]; 
            int i = 0;       
            while (index > 0)      
            {            
                int mod = index % system;     
                if (mod == 0) mod = system;   
                digArray[i++] = (char)(mod - 1 + 'A');    
                index = (index - 1) / 26;       
            }    
            StringBuilder sb = new StringBuilder(i);   
            for (int j = i - 1; j >= 0; j--)       
            {            
                sb.Append(digArray[j]);     
            }        
            return sb.ToString(); 
        }

        public static Microsoft.Win32.SafeHandles.SafeFileHandle ServerfPath { get; set; }
    }
}