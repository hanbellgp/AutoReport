using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

namespace Hanbell.AutoReport.Config
{
    class IdleStock_ZjComer : IdleStock_Comer
    {
        public IdleStock_ZjComer() { }

        protected override void Init()
        {
            SetMailHead();
            this.nc = new IdleStockConfig(DBServerType.SybaseASE, "ZjComerERP", this.ToString());
            this.nc.InitData();
            this.nc.ConfigData();

            if (nc.GetReportList().Count > 0)
            {
                this.SetAttachment();
            }

            this.content = GetContentHead() + "<br/><br/><br/><br/>" + GetContentFooter();

            AddNotify(new MailNotify());

        }
    }

}
