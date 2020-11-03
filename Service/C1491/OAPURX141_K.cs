using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace C1491
{
    class OAPURX141_K : Hanbell.AutoReport.Config.NotificationContent
    {
        protected override void Init()
        {
            base.Init();
            this.nc = new OAPURX141Config(DBServerType.MSSQL, "EFGP", this.ToString());
            this.nc.InitData();
            this.nc.ConfigData();

            if (nc.GetDataTable("tbresult").Rows.Count > 0)
            {
                this.content = this.GetContent(nc.GetDataTable("tbresult"), null, null);
                DataTableToExcel(nc.GetDataTable("tbresult"), GetReportName(this.ToString()), true);
                AddNotify(new MailNotify());
            }

        }
    }
}
