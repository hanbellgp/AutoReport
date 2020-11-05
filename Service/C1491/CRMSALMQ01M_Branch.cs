using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace C1491
{
    class CRMSALMQ01M_Branch : Hanbell.AutoReport.Config.NotificationContent
    {
        protected override void Init()
        {
            base.Init();
            nc = new CRMSALMQ01M_BranchConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetDataTable("tbcrmsalmq01m").Rows.Count > 0)
            {
                this.content = GetContentHead() + "<br/><br/><br/><br/>" + GetContentFooter();

                DataTableToExcel(nc.GetDataTable("tbcrmsalmq01m"), GetReportName(this.ToString()), true);
                AddNotify(new MailNotify());
            }

        }
    }
}
