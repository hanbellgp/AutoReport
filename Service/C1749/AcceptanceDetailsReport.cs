using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class AcceptanceDetailsReport:NotificationContent
    {
        public AcceptanceDetailsReport() { }

        protected override void Init()
        {
            base.Init();
            nc = new AcceptanceDetailsReportConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            DataTable dt = nc.GetDataTable("dbtlb");
            if (dt.Rows.Count > 0 && dt.Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "PUR530验收明细报表" + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss") + ".xlsx";
                DataTableToExcel(dt, fileFullName, true);
                AddNotify(new MailNotify());
            }

        }
    }
}
