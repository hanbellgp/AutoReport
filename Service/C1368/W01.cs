using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class W01 : NotificationContent
    {

        public W01()
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
                    else if (tbl.Columns[k].ColumnName == "rate1" || tbl.Columns[k].ColumnName == "rate2" )
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
                    if (tbl.Columns[k].ColumnName == "rate1")
                    {
                        if (obj.ToString() == "&nbsp;")
                        {
                            sb.Append("<span>" + obj.ToString() + "</span>");
                        }
                        else if (obj.ToString() != "&nbsp;" && (decimal.Parse(tbl.Rows[j]["rate1"].ToString()) >= 0))
                        {
                            sb.Append("<span style='color:red;'>" + obj.ToString() + "%" + "</span>");
                        }
                        else
                        {
                            sb.Append("<span>" + obj.ToString().Trim() + "%" + "</span>");
                        }
                    }
                    else if (tbl.Columns[k].ColumnName == "rate2")
                    {
                        if (decimal.Parse(tbl.Rows[j]["rate2"].ToString()) <= 0)
                        {
                            sb.Append("<span style='color:red;'>" + obj.ToString() + "%" + "</span>");
                        }
                        else
                        {
                            sb.Append("<span>" + obj.ToString().Trim() + "%" + "</span>");
                        }
                    }
                    else if (tbl.Columns[k].ColumnName == "rate1" || tbl.Columns[k].ColumnName == "rate2")
                    {
                        if (obj.ToString() != "&nbsp;") sb.Append("<span>" + obj.ToString() + "%" + "</span>");
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



        protected override string GetContentHead()
        {
            string yemon = string.Format("{0:Y}", System.DateTime.Now.AddMonths(-1));
            this.subject = yemon + this.subject;
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
            sb.Append(".subject {font-size: 22px;font-family:'微软雅黑';text-align: center;height: 60px;border-bottom: 6px solid #F0F0F0;line-height: 2.6em;margin-bottom: 20px;}");
            sb.Append("div.table-jg {height:100%;} ");
            sb.Append(".table-jg table tr {background: none repeat scroll 0% 0% #E8F9FF;font-size: 15pt;;height: 25px }");
            sb.Append(".table-jg table th {background: none repeat scroll 0% 0% #A9D8E8;font-size: 16pt;}");
            sb.Append(".table-jg table td {padding: 0px 3px;line-height: 0.8em;}");
            sb.Append(".table-jg table, .table-jg table th, .table-jg table td {border: 1px solid #CFEBF6;color: #666;}");
            sb.Append(" table {padding:3px 0px 3px 0px;border:solid 1 #000000;margin:0 0 0 0;BORDER-COLLAPSE: collapse;}");
            sb.Append("</style>");
            sb.Append("</HEAD><BODY><p style='text-align:left;'>");
            sb.Append(GetMailHeadAdd(this.ToString()));
            sb.Append("</p><DIV class='table-jg'>");
            sb.Append("<p class='subject'>" + yemon + GetMailSubject(this.ToString()) + "</p>");
            return sb.ToString();
        }

        private string GetNotification(string p, string p_2)
        {
            throw new NotImplementedException();
        }


        protected override void Init()
        {
            base.Init();

            //nc = new BxdmxConfig(Core.DBServerType.MSSQL, "SHBOA");
            nc = new W01Config(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList() != null)
            {
                //SetAttachment();
            }

            string[] title = { "产品别", "本年当月库存台数", "去年同期库存台数", "库存增长比率", "本年当月出货台数", "去年同期出货台数", "出货增长比率" };
            int[] width = { 100, 150, 150, 150, 150, 150, 150 };

            //string[] title = { };
            this.content = GetContent(nc.GetDataTable("W01"), title, width);


            if (nc.GetDataTable("W01").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }

        }



    }
}










