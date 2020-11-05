using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class INV520NegativeStock_V:NotificationContent
    {
        public INV520NegativeStock_V() { }

        protected override void Init()
        {
            base.Init();
            nc = new INV520NegativeStockConfig(DBServerType.SybaseASE, "VHBERP", this.ToString());
            this.nc.InitData();
            this.nc.ConfigData();
            this.content = GetContentHead() + GetContentFooter();
            if (nc.GetDataTable("tlb").Rows.Count > 0)
            {
                string fileFullName1 = Base.GetServiceInstallPath() + "\\Data\\" + "INV520负库存明细表" + DateTime.Now.ToString("yyyy-MM") + ".xlsx";
                DataTableToExcel(nc.GetDataTable("tlb"), fileFullName1, true);
                AddNotify(new MailNotify());
            }
        }
    }

}
