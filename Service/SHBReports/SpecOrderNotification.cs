using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using CrystalDecisions.Shared;
using System.Data;
using System.IO;

namespace Hanbell.AutoReport.Config
{

    public class SpecOrderNotification : NotificationContent
    {

        public SpecOrderNotification()
        {

        }

        protected override string GetContent(DataTable tbl, string[] title, int[] width)
        {
            if (title.Length != width.Length)
            {
                return "指定的标题与栏位宽度设定不一致";
            }
            int i;
            string display;
            StringBuilder sb = new StringBuilder();
            sb.Append(GetContentHead());
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
                        sb.Append("<th width='" + width[i] + "'><span>" + display + "</span></th>");
                    }
                    else
                    {
                        sb.Append("<th><span>" + column.ColumnName + "</span></th>");
                    }
                }
                else
                {
                    sb.Append("<th><span>" + column.ColumnName + "</span></th>");
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
                    if (tbl.Columns[k].ColumnName == "plan")
                    {
                        if (obj.ToString() == "&nbsp;")
                        {
                            sb.Append("<span>" + obj.ToString() + "</span>");
                        }
                        else if (obj.ToString() != "&nbsp;" && (DateTime.Parse(obj.ToString()).Date < DateTime.Now.Date) && (tbl.Rows[j]["fact"].ToString() == ""))
                        {
                            sb.Append("<span style='color:red;'>" + Base.Format(obj.ToString(), "yyyy-MM-dd") + "</span>");
                        }
                        else
                        {
                            sb.Append("<span>" + Base.Format(obj.ToString(), "yyyy-MM-dd") + "</span>");
                        }
                    }
                    else if (tbl.Columns[k].ColumnName == "dplan")
                    {
                        if ((DateTime.Parse(obj.ToString()).Date < DateTime.Now.Date) && (tbl.Rows[j]["dfact"].ToString() == ""))
                        {
                            sb.Append("<span style='color:red;'>" + Base.Format(obj.ToString(), "yyyy-MM-dd") + "</span>");
                        }
                        else
                        {
                            sb.Append("<span>" + Base.Format(obj.ToString(), "yyyy-MM-dd") + "</span>");
                        }
                    }
                    else if (tbl.Columns[k].ColumnName == "fact" || tbl.Columns[k].ColumnName == "dfact")
                    {
                        if (obj.ToString() != "&nbsp;") sb.Append("<span>" + Base.Format(obj.ToString(), "yyyy-MM-dd") + "</span>");
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
            sb.Append(GetContentFooter());
            return sb.ToString();
        }

        protected override void Init()
        {
            base.Init();

            nc = new SpecOrderNotificationConfig(DBServerType.SybaseASE, "SHBERP",this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList() != null)
            {
                SetAttachment();
            }
            string[] title = { "编号", "项目", "标注", "计划日期", "实际日期", "延迟", "单号", "序号", "内容", "数量", "已验收", "部门", "负责人", "计划日期", "实际完成", "延迟" };
            int[] width = { 80, 200, 50, 70, 70, 50, 80, 45, 200, 50, 50, 50, 60, 70, 70, 50 };
            this.content = GetContent(nc.GetDataTable("tblresults"), title, width);

            if (nc.GetDataTable("tblresults").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }

    }

}