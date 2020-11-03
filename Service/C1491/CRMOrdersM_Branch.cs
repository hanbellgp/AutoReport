using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace C1491
{
    class CRMOrdersM_Branch : Hanbell.AutoReport.Config.NotificationContent
    {
        protected override void Init()
        {
            base.Init();
            nc = new CRMOrdersM_BranchConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetDataTable("tbcrmorders").Rows.Count > 0)
            {
                this.content = GetContentHead() + "<br/><br/><br/><br/>" + GetContentFooter();

                DataTableToExcel(nc.GetDataTable("tbcrmorders"), GetReportName(this.ToString()), true);
                AddNotify(new MailNotify());
            }

        }
    }
   
}
