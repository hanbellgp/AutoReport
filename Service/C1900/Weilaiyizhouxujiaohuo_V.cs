using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class Weilaiyizhouxujiaohuo_V : NotificationContent
    {
        public Weilaiyizhouxujiaohuo_V()
        { 
        }
        protected override void Init()
        {
            base.Init();
            nc = new WeilaiyizhouxujiaohuoConfig_V(DBServerType.SybaseASE, "VHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            this.content = GetContentHead() + GetContentFooter();

            if (nc.GetDataTable("tblresult1").Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "越南未来一周需交货明细" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                DataTableToExcel(nc.GetDataTable("tblresult1"), fileFullName, true);
                AddNotify(new MailNotify());

            }
        }
    }
}
