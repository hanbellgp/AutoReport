using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class ProductiveTurnover:NotificationContent
    {
        public ProductiveTurnover() { }

        protected override void Init()
        {
            base.Init();
            nc = new ProductiveTurnoverConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            DataTable dt = nc.GetDataTable("dbtlb");
            if (dt.Rows.Count > 0 && dt.Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "生产性物料周转天数报表" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                DataTableToExcel(dt, fileFullName, true);
                AddNotify(new MailNotify());
            }

        }
    }
}
