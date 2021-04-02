using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class Jiechangshangbaobiao_K : NotificationContent
    {
        public Jiechangshangbaobiao_K()
        { 
        }
        protected override void Init()
        {
            base.Init();
            nc = new JiechangshangbaobiaoConfig_K(DBServerType.SybaseASE, "ComerERP", this.ToString());//ComerERP
            nc.InitData();
            nc.ConfigData();
            this.content = GetContentHead() + GetContentFooter();

            if (nc.GetDataTable("tblresult3").Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "柯茂借厂商报表" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                DataTableToExcel(nc.GetDataTable("tblresult3"), fileFullName, true);
                AddNotify(new MailNotify());

            }
        }
    }
}
