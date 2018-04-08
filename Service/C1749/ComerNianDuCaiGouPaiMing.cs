using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class ComerNianDuCaiGouPaiMing : NotificationContent
    {
        public ComerNianDuCaiGouPaiMing()
        {

        }
        protected override void Init()
        {
            base.Init();

            nc = new ComerNainDuCaiGouMaiMingConfig(DBServerType.SybaseASE, "ComerERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            string[] title = { "年度", "厂商代号", "厂商简称", "上月采购金额(万元)", "上月排名", "年度采购金额(万元)", "年度排名", "同期采购金额(万元)", "同期排名", "去年采购金额(万元)", "去年排名" };
            int[] width = { 80, 150, 150, 100, 70, 150, 70, 150, 70, 150, 70 };
            this.content = GetContent(nc.GetDataTable("ndcgpm"), title, width);
            if (nc.GetDataTable("ndcgpm").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
        //一表两字
        protected override string GetContent(DataTable tbl, string[] title, int[] width)
        {
            if (title != null && width != null && title.Length != width.Length)
            {
                return "指定的标题与栏位宽度设定不一致";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(GetContentHead());
            sb.Append("<p class='subject'> 上海柯茂机械有限公司</p>");
            sb.Append("<p class='subject'> 本年度采购厂商进货排名前30</p>");
            sb.Append("<p>制表时间" + DateTime.Now.ToString("yyyy年MM月dd日") + "</p>");
            sb.Append(GetHTMLTable(tbl, title, width));
            while (tbl.Rows.Count > 5)
            {
                tbl.Rows.RemoveAt(tbl.Rows.Count - 1);
            }
            //DateTime dt = new DateTime();
            sb.Append("<br/><br/><br/>");
            sb.Append("<p class='subject'> 上海柯茂机械有限公司</p>");
            sb.Append("<p class='subject'> 本年度采购厂商进货排名前5</p>");
            sb.Append("<p>制表时间" + DateTime.Now.ToString("yyyy年MM月dd日") + "</p>");
            sb.Append(GetHTMLTable(tbl, title, width));

            sb.Append(GetContentFooter());
            return sb.ToString();
        }
    }
}
