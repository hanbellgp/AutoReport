using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class Temporary : NotificationContent
    {
        public Temporary() { }

        protected override void Init()
        {
            base.Init();
            nc = new TemporaryConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            DataTable dt1 = nc.GetDataTable("jtdaytlb");
            DataTable dt2 = nc.GetDataTable("jtmonthtlb");

            string[] title = { "产品别", "出货台数", "出货金额", "订单台数", "订单金额", "日期" };
            int[] width = { 100, 100, 100, 100, 100, 100 };
            this.content = GetContent(dt1,dt2,title, width);
            if (nc.GetDataTable("jtdaytlb").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }

        protected string GetContent(DataTable tbl, DataTable tbl2, string[] title, int[] width)
        {
            if (title != null && width != null && title.Length != width.Length)
            {
                return "指定的标题与栏位宽度设定不一致";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(GetContentHead());
            sb.Append("<p class='subject' style='text-align:left'>集团报表每日</p>");
            sb.Append("<p>制表时间" + DateTime.Now.ToString("yyyy年MM月dd日") + "</p>");
            sb.Append(GetHTMLTable(tbl, title, width));

            sb.Append("<p class='subject' style='text-align:left'>集团报表每月</p>");
            sb.Append("<p>制表时间" + DateTime.Now.ToString("yyyy年MM月dd日") + "</p>");
            sb.Append(GetHTMLTable(tbl2, title, width));
            sb.Append(GetContentFooter());
            return sb.ToString();
        }

    }
}
