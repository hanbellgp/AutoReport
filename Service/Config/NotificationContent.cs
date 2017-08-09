using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Excel = Microsoft.Office.Interop.Excel;
using Hanbell.AutoReport.Core;
using System.Xml;

namespace Hanbell.AutoReport.Config
{

    public class NotificationContent : Notification
    {

        protected NotificationConfig nc;

        public NotificationContent()
        {

        }

        public NotificationConfig GetNotificationConfig()
        {
            return nc;
        }

        public override void Dispose()
        {
            if (nc != null)
            {
                nc.Dispose();
            }
            nc = null;
            base.Dispose();
        }

        protected virtual string GetContentHead()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<HTML><HEAD>");
            sb.Append("<title>" + this.subject + "</title>");
            sb.Append("<META HTTP-EQUIV='content-type' CONTENT='text/html; charset=UTF8'> ");
            sb.Append("<script language=javascript>");
            sb.Append("self.resizeBy(0,0);");
            sb.Append("self.resizeTo(screen.availWidth,screen.availHeight);");
            sb.Append("</script>");
            sb.Append("<style type=text/css>");
            sb.Append(".number {text-align:right;} ");
            sb.Append(".subject {font-size: 18px;font-family:'微软雅黑';text-align: center;height: 50px;border-bottom: 1px solid #F0F0F0;line-height: 2.6em;margin-bottom: 20px;}");
            sb.Append("div.table-jg {height:100%;} ");
            sb.Append(".table-jg table tr {background: none repeat scroll 0% 0% #E8F9FF;font-size: 9pt;;height: 25px }");
            sb.Append(".table-jg table th {background: none repeat scroll 0% 0% #A9D8E8;font-size: 10pt;}");
            sb.Append(".table-jg table td {padding: 0px 3px;line-height: 0.8em;}");
            sb.Append(".table-jg table, .table-jg table th, .table-jg table td {border: 1px solid #CFEBF6;color: #666;}");
            sb.Append(" table {padding:3px 0px 3px 0px;border:solid 1 #000000;margin:0 0 0 0;BORDER-COLLAPSE: collapse;}");
            sb.Append("</style>");
            sb.Append("</HEAD><BODY><p style='text-align:left;'>");
            sb.Append(GetMailHeadAdd(this.ToString()));
            sb.Append("</p><DIV class='table-jg'>");
            sb.Append("<p class='subject'>" + GetMailSubject(this.ToString()) + "</p>");
            return sb.ToString();
        }

        protected virtual string GetContent(DataTable tbl, string[] title)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetContentHead());
            sb.Append(GetHTMLTable(tbl, title, null));
            sb.Append(GetContentFooter());
            return sb.ToString();
        }

        protected virtual string GetContent(DataTable tbl, string[] title, int[] width)
        {
            if (title != null && width != null && title.Length != width.Length)
            {
                return "指定的标题与栏位宽度设定不一致";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(GetContentHead());
            sb.Append(GetHTMLTable(tbl, title, width));
            sb.Append(GetContentFooter());
            return sb.ToString();
        }

        protected virtual string GetContentFooter()
        {
            return "</DIV><p style='text-align:left;'>" + GetMailFooterAdd(this.ToString()) + "</p></BODY></HTML>";
        }

        protected virtual string GetHTMLTable(DataTable tbl, string[] title, int[] width)
        {
            int i;
            string display;
            StringBuilder sb = new StringBuilder();
            sb.Append("<TABLE>");
            sb.Append("<tr valign='middle'>");
            foreach (DataColumn column in tbl.Columns)
            {
                if (title != null)
                {
                    i = tbl.Columns.IndexOf(column);
                    if (i < title.Length)
                    {
                        display = title[i];
                        if (width != null)
                        {
                            sb.Append("<th width='" + width[i] + "'><span>" + display + "</span></th>");
                        }
                        else
                        {
                            sb.Append("<th><span>" + display + "</span></th>");
                        }
                    }
                    else
                    {
                        sb.Append("<th><span>" + column.Caption + "</span></th>");
                    }
                }
                else
                {
                    sb.Append("<th><span>" + column.Caption + "</span></th>");
                }
            }
            sb.Append("</tr>");
            int colsCount = tbl.Columns.Count;
            int rowsCount = tbl.Rows.Count;
            object obj;
            for (int j = 0; j < rowsCount; j++)
            {
                sb.Append("<tr>");
                for (int k = 0; k < colsCount; k++)
                {
                    if ((tbl.Columns[k].DataType == System.Type.GetType("System.Double")) || (tbl.Columns[k].DataType == System.Type.GetType("System.Decimal")) ||
                       (tbl.Columns[k].DataType == System.Type.GetType("System.Int16")) || (tbl.Columns[k].DataType == System.Type.GetType("System.Int32")) || (tbl.Columns[k].DataType == System.Type.GetType("System.Int64")))
                    {
                        //数字格式右对齐
                        sb.Append("<td class='number'>");
                    }
                    else
                    {
                        sb.Append("<td>");
                    }
                    obj = tbl.Rows[j][k];
                    if (obj == DBNull.Value || obj.ToString() == "")
                    {
                        // 如果是NULL则在HTML里面使用一个空格替换之  
                        obj = "&nbsp;";
                    }
                    if (tbl.Columns[k].DataType == System.Type.GetType("System.DateTime"))
                    {
                        if (obj.ToString() != "&nbsp;") sb.Append("<span>" + Base.Format(obj.ToString(), "yyyy-MM-dd") + "</span>");
                    }
                    else if (tbl.Columns[k].DataType == System.Type.GetType("System.Decimal"))
                    {
                        sb.Append("<span>" + String.Format("{0:N}", Decimal.Parse(obj.ToString())) + "</span>");
                    }
                    else
                    {
                        sb.Append("<span>" + obj.ToString().Trim() + "</span>");
                    }
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</TABLE>");
            return sb.ToString();
        }

        protected virtual string GetReportName(string notification)
        {
            XmlNode node = Base.GetXmlNode("NotificationConfig.xml", notification);
            if (node != null)
            {
                return Base.GetServiceInstallPath() + "\\Data\\" + node.Attributes["reportName"].Value.ToString() + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss") + ".xls";
            }
            else
            {
                return Base.GetServiceInstallPath() + "\\Data\\" + "Merged_" + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss") + ".xls";
            }
        }

        /// <summary>
        /// 将DataTable转成Excel
        /// </summary>
        /// <param name="tbl">需要转换的DataTable</param>
        /// <param name="fileName">Excel文件完整名称</param>
        /// <param name="flag">True自动加入附件,False不加入附件</param>
        protected void DataTableToExcel(System.Data.DataTable tbl, string fileName, bool flag)
        {
            if (tbl == null)
                return;
            int rowNum = tbl.Rows.Count;
            int columnNum = tbl.Columns.Count;
            int rowIndex = 1;
            int columnIndex = 0;

            var excelApp = new Excel.ApplicationClass();
            excelApp.Visible = false;
            excelApp.DisplayAlerts = false;

            Excel.Workbook newBook = excelApp.Workbooks.Add();
            //newBook.SaveAs(fileName);
            //将DataTable的列名导入Excel表第一行
            foreach (DataColumn col in tbl.Columns)
            {
                columnIndex++;
                excelApp.Cells[rowIndex, columnIndex] = col.Caption;
            }
            //将DataTable中的数据导入Excel中
            for (int i = 0; i < rowNum; i++)
            {
                rowIndex++;//数据从第二行开始
                columnIndex = 0;
                for (int j = 0; j < columnNum; j++)
                {
                    columnIndex++;
                    excelApp.Cells[rowIndex, columnIndex] = tbl.Rows[i][j].ToString();
                }
            }
            newBook.SaveCopyAs(fileName);
            excelApp.Workbooks.Close();
            excelApp.Quit();
            //Kill打开的Excel进程
            Process[] excelApps;
            excelApps = Process.GetProcessesByName("EXCEL");
            foreach (Process p in excelApps)
            {
                p.Kill();
            }//End
            if (flag)
            {
                AddAtt(fileName); //加入附件中
            }

        }

        /// <summary>
        /// 合并多个Excel
        /// </summary>
        /// <param name="files">多个Excel</param>
        /// <param name="newFile">合并后的Excel文件名</param>
        /// <param name="flag">True清除原附件,False不清除原附件</param>
        protected void MergeMulitExcelToOne(Hashtable files, string newFile, bool flag)
        {
            //声明Excel对象
            var excelApp = new Excel.ApplicationClass();
            excelApp.Visible = false;
            excelApp.DisplayAlerts = false;

            Excel.Workbook book;
            Excel.Workbook newBook;
            Excel.Worksheet sheet;
            Excel.Worksheet newSheet;

            newBook = excelApp.Workbooks.Add();
            newBook.SaveAs(newFile);
            int m = newBook.Sheets.Count;
            for (int n = 1; n < m; n++)
            {
                newSheet = (Microsoft.Office.Interop.Excel.Worksheet)newBook.Sheets[n];
                newSheet.Delete();
                newBook.Save();
            };
            excelApp.Workbooks.Close();

            int i;
            foreach (string item in files.Values)
            {
                newBook = excelApp.Workbooks.Open(newFile);
                book = excelApp.Workbooks.Open(item);
                foreach (var entity in book.Sheets)
                {
                    sheet = (Microsoft.Office.Interop.Excel.Worksheet)entity;
                    i = item.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase);
                    sheet.Name = item.Substring(i + 1);
                    book.Save();
                    newSheet = (Microsoft.Office.Interop.Excel.Worksheet)newBook.ActiveSheet;
                    sheet.Copy(newBook.Sheets[newSheet.Index], Type.Missing);
                    newBook.Save();
                }
                excelApp.Workbooks.Close();
                i = 0;
            }
            excelApp.Workbooks.Close();
            excelApp.Quit();
            //Kill打开的Excel进程
            Process[] excelApps;
            excelApps = Process.GetProcessesByName("EXCEL");
            foreach (Process p in excelApps)
            {
                p.Kill();
            }//End
            if (flag)
            {
                ClearAtt();
            }
            //加入附件中
            AddAtt(newFile);
        }

        protected void SetAttachment()
        {
            nc.ConfigReport();
            string file;
            ExportFormatType type;

            foreach (ReportClass item in nc.GetReportList())
            {
                file = Base.GetAttachmentFileName(this.ToString(), item.FullResourceName);
                type = Base.GetAttachmentFileType(this.ToString(), item.FullResourceName);
                file = nc.ExportReport(type, file, item);
                AddAtt(file);
            }
        }

        protected string GetMailAddressByEmployeeIdFromOA(string employeeid)
        {
            if (nc == null) return "";
            string sqlstr = "SELECT mailAddress FROM Users WHERE id='{0}'";
            return nc.GetQueryString(DBServerType.MSSQL, Base.GetDBConnectionString("EFGP"), String.Format(sqlstr, employeeid));
        }

        protected string GetManagerIdByEmployeeIdFromOA(string employeeid)
        {
            if (nc == null) return "";
            string sqlstr = "select id from Users where Users.OID = '{0}'";
            return nc.GetQueryString(DBServerType.MSSQL, Base.GetDBConnectionString("EFGP"), String.Format(sqlstr, GetManagerOIDByEmployeeIdFromOA(employeeid)));
        }

        protected string GetManagerOIDByEmployeeIdFromOA(string employeeid)
        {
            if (nc == null) return "";
            string sqlstr = "select specifiedManagerOID from Functions,Users where Users.OID = Functions.occupantOID and Functions.isMain=1 and Users.id='{0}'";
            return nc.GetQueryString(DBServerType.MSSQL, Base.GetDBConnectionString("EFGP"), String.Format(sqlstr, employeeid));
        }

        protected string[] GetHTMLFileContents(string fileName, string beginTag, string endTag, int beginAdjust, int endAdjust)
        {
            string[] contents = new string[] { };
            string content;
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            if (!stream.CanRead)
            {
                throw new FileNotFoundException();
            }

            try
            {
                StreamReader reader = new StreamReader(stream);
                string fileContent = reader.ReadToEnd();
                int begin = 0;
                int end = 0;
                while (1 == 1)
                {
                    begin = fileContent.IndexOf(beginTag, begin + 1);
                    if (begin == -1) break;
                    end = fileContent.IndexOf(endTag, end + 1);
                    if (end == -1) break;
                    content = fileContent.Substring(begin + beginAdjust, end - (begin + beginAdjust) + endAdjust);
                    if (content != "")
                    {
                        Array.Resize(ref contents, contents.Length + 1);
                        contents.SetValue(content, contents.Length - 1);
                    }
                }
                reader.Close();
                reader.Dispose();
                stream.Dispose();
                return contents;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected string GetHTMLFileLastContent(string fileName, string beginTag, string endTag, int beginAdjust, int endAdjust)
        {
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            if (!stream.CanRead)
            {
                throw new FileNotFoundException();
            }

            try
            {
                StreamReader reader = new StreamReader(stream);
                string fileContent = reader.ReadToEnd();
                int begin = fileContent.LastIndexOf(beginTag);
                int end = fileContent.LastIndexOf(endTag);
                fileContent = fileContent.Substring(begin + beginAdjust, end - (begin + beginAdjust) + endAdjust);
                reader.Close();
                reader.Dispose();
                stream.Dispose();
                return fileContent;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
