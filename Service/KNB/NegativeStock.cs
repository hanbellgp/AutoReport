using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;
using System.Data;

namespace KNB.config
{
    class NegativeStock : NotificationContent
    {
        public NegativeStock() { }

        protected override void Init()
        {
            base.Init();
            nc = new NegativeStockConfig(DBServerType.SybaseASE, "KNBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            this.content = GetContentHead() + GetContentFooter();
            DataTable dt = nc.GetDataTable("tlb");
            if (dt.Rows.Count > 0 && dt.Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "负库存明细报表" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                DataTableToExcel(dt, fileFullName, true);
                AddNotify(new MailNotify());
            }

        }

    }
}
