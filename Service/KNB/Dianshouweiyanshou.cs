using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;

namespace KNB.config
{
    class Dianshouweiyanshou : NotificationContent
    {
        public Dianshouweiyanshou() { }

        protected override void Init()
        {
            base.Init();

            nc = new DianshouweiyanshouConfig(Hanbell.AutoReport.Core.DBServerType.SybaseASE, "KNBERP");
            nc.InitData();

            if (nc.GetReportList() != null)
            {
                SetAttachment();
            }
            if (nc.GetDataTable("tbl").Rows.Count > 0)
            {
                string[] title = { "点收编号", "厂商编号", "厂商简称", "大类", "品号", "品名", "点收数量", "点收时间", "延误小时数", "备注"};
                int[] width = { 100, 100, 100, 60, 120, 120, 60, 100,60,150 };
                this.content = GetContent(nc.GetDataTable("tbl"), title, width);
                AddNotify(new MailNotify());
            }
        }
    }
}
