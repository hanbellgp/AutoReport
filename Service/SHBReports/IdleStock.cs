using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class IdleStock: NotificationContent
    {

        public IdleStock()
        {
        }

        protected override void Init()
        {
            base.Init();
            this.nc = new IdleStockConfig(Core.DBServerType.SybaseASE, "SHBERP", this.ToString());
            this.nc.InitData();
            this.nc.ConfigData();

            if (nc.GetReportList().Count > 0)
            {
                SetAttachment();
            }

            this.content = GetContentHead() + "<br/><br/><br/><br/>" + GetContentFooter();

            AddNotify(new MailNotify());

        }

    }
}
