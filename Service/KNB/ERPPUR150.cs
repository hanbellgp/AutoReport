using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;

namespace KNB.config
{
    class ERPPUR150:NotificationContent
    {
        public ERPPUR150() { }
        protected override void Init()
        {
            base.Init();
            nc = new ERPPUR150Config(DBServerType.SybaseASE, "KNBERP");
            nc.InitData();
            nc.ConfigData();
            if (nc.GetReportList() != null)
            {
                string[] title = { "公司别", "厂商代号", "厂商简称", "品号", "品名", "到期时间" };
                int[] width = { 100, 120, 150, 140, 150, 120 };
                this.content = GetContentHead() + "<br/><br/><br/><br/>" + GetContentFooter();
                this.content = GetContent(nc.GetDataTable("tbresult"), title, width);
                AddNotify(new MailNotify());
            }
        }
    }
}
