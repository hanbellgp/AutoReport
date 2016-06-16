using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.VHB
{
    public class IdleStock_VHB:IdleStock
    {

        public IdleStock_VHB()
        {
        }

        protected override void Init()
        {
            SetMailHead();
            this.nc = new IdleStockConfig(Core.DBServerType.SybaseASE, "VHBERP", this.ToString());
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
