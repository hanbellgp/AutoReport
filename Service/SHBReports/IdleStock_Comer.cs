using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class IdleStock_Comer:IdleStock
    {
        public IdleStock_Comer()
        {
        }

        protected override void Init()
        {
            SetMailHead();
            this.nc = new IdleStockConfig(Core.DBServerType.SybaseASE, "ComerERP", this.ToString());
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
