using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class Yidianshouweiyanshou_E:NotificationContent
    {
        public Yidianshouweiyanshou_E() { }
        protected override void Init()
        {
            base.Init();
            nc = new YidianshouweiyanshouConfig_K(Core.DBServerType.SybaseASE, "ZjComerERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList().Count > 0)
            {
                SetAttachment();
            }

            this.content = GetContent(nc.GetDataTable("tblresult7"), null);

            if (nc.GetDataTable("tblresult7").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
