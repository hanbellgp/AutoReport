using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class Yuqiweijieancaigoudan_K : NotificationContent
    {
        public Yuqiweijieancaigoudan_K()
        { 
        }
        protected override void Init()
        {
            base.Init();
            nc = new YuqiweijieancaigoudanConfig_K(DBServerType.SybaseASE, "ComerERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            this.content = GetContentHead() + GetContentFooter();

            if (nc.GetDataTable("tblresult2").Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "柯茂逾期未结案采购单明细" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                DataTableToExcel(nc.GetDataTable("tblresult2"), fileFullName, true);
                AddNotify(new MailNotify());

            }
        }
    }
}
