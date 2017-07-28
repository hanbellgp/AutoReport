using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class JZchmxb : NotificationContent
    {

        protected override void Init()
        {
            base.Init();
            nc = new JZchmxbConfig(Core.DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();

            

            if (nc.GetDataTable("tblresult").Rows.Count > 0)
            {
                DataTableToExcel(nc.GetDataTable("tblresult"), GetReportName(this.ToString()),true);
                this.content = GetContentHead() + "<br><br><br>" + GetContentFooter();
                AddNotify(new MailNotify());
            }

        }

        protected override string GetContentHead()
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
            return sb.ToString();
        }

   
    }
}
