using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class SEIASBill:NotificationContent
    {
        public SEIASBill() { }
        protected override void Init()
        {
            base.Init();
            nc = new SEIASBillConfig(DBServerType.SybaseASE,"SHBERP",this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetDataTable("SEIASTlb").Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "SE及IAS单据表" + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss") + ".xlsx";
                DataTableToExcel(nc.GetDataTable("SEIASTlb"), fileFullName, true);
                AddNotify(new MailNotify());

            }
        }
    }
    
}
