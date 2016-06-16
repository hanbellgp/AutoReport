using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class Dianshouweiyanshou_zhuwei:NotificationContent
    {
        public Dianshouweiyanshou_zhuwei()
        {
        }

        protected override void Init()
        {
            base.Init();

            nc = new Dianshouweiyanshou_zhuweiConfig(Core.DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetReportList().Count > 0)
            {
                SetAttachment();
            }

            this.content = GetContent(nc.GetDataTable("tblresult"),null);

            if (nc.GetDataTable("tblresult").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }


    }
}
