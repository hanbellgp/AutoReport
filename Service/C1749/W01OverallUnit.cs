using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class W01OverallUnit:NotificationContent
    {
        public W01OverallUnit() { }
        protected override void Init()
        {
            base.Init();
            nc = new W01OverallUnitConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            this.nc.InitData();
            this.nc.ConfigData();
            this.content = GetContentHead() + GetContentFooter();
            if (nc.GetDataTable("dbtlb").Rows.Count > 0)
            {
                string fileFullName1 = Base.GetServiceInstallPath() + "\\Data\\" + "W01成品库存数" + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss") + ".xlsx";
                DataTableToExcel(nc.GetDataTable("dbtlb"), fileFullName1, true);
                AddNotify(new MailNotify());

            }
        }
    }
}
