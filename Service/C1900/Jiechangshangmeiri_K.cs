using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class Jiechangshangmeiri_K : NotificationContent
    {
        public Jiechangshangmeiri_K()
        { 
        }
        protected override void Init()
        {
            base.Init();
            nc = new JiechangshangmeiriConfig_K(DBServerType.SybaseASE, "ComerERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            this.content = GetContentHead() + GetContentFooter();

            if (nc.GetDataTable("tblresult4").Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "柯茂借厂商每日报表" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                DataTableToExcel(nc.GetDataTable("tblresult4"), fileFullName, true);
                AddNotify(new MailNotify());

            }
        }
    }
}
