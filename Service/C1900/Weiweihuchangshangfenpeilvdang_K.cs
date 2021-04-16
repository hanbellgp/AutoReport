using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class Weiweihuchangshangfenpeilvdang_K : NotificationContent
    {
        public Weiweihuchangshangfenpeilvdang_K()
        { 
        }
        protected override void Init()
        {
            base.Init();
            nc = new WeiweihuchangshangfenpeilvdangConfig_K(DBServerType.SybaseASE, "ComerERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            this.content = GetContentHead() + GetContentFooter();

            if (nc.GetDataTable("tblresult5").Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "柯茂未维护厂商分配率档报表" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                DataTableToExcel(nc.GetDataTable("tblresult5"), fileFullName, true);
                AddNotify(new MailNotify());

            }
        }
    }
}
