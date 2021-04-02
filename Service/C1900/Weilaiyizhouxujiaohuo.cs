using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class Weilaiyizhouxujiaohuo : NotificationContent
    {
        public Weilaiyizhouxujiaohuo()
        { 
        }
        protected override void Init()
        {
            base.Init();
            nc = new WeilaiyizhouxujiaohuoConfig(DBServerType.SybaseASE, "ComerERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            this.content = GetContentHead() + GetContentFooter();
         
            if (nc.GetDataTable("tblresult1").Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "柯茂未来一周需交货明细" + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss") + ".xlsx";
                DataTableToExcel(nc.GetDataTable("tblresult1"), fileFullName, true);
                AddNotify(new MailNotify());

            }
        }
    }
}
