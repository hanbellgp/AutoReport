using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;

namespace C0160
{
    public class NegativeStock : NotificationContent
    {

        public NegativeStock()
        {
        }

        protected override void Init()
        {
            base.Init();

            nc = new NegativeStockConfig(Hanbell.AutoReport.Core.DBServerType.SybaseASE, "KNBERP");
            nc.InitData();

            if (nc.GetReportList() != null)
            {
                SetAttachment();
            }
            if (nc.GetDataTable("tbl").Rows.Count > 0)
            {
                this.content = GetContent(nc.GetDataTable("tbl"),null);
                AddNotify(new MailNotify());
            }
        }
    }
}
