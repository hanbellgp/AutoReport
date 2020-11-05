using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class InvbalAgingReport:NotificationContent
    {
        public InvbalAgingReport() 
        {
            
        }

        protected override void Init()
        {
            base.Init();
            nc = new InvbalAgingReportConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            this.nc.InitData();
            this.nc.ConfigData();
            this.content = GetContentHead() + GetContentFooter();
            if (nc.GetDataTable("tlb").Rows.Count > 0)
            {
                string fileFullName1 = Base.GetServiceInstallPath() + "\\Data\\" + "原物料库铸加物料账龄天数报表" + DateTime.Now.ToString("yyyy-MM") + ".xlsx";
                DataTableToExcel(nc.GetDataTable("tlb"), fileFullName1, true);
                AddNotify(new MailNotify());
            }
        }
    }
}
