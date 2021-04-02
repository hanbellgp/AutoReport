using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class Yuqiweijieancaigoudan_E:NotificationContent
    {
        public Yuqiweijieancaigoudan_E() { }

        protected override void Init()
        {
            base.Init();
            nc = new YuqiweijieancaigoudanConfig(DBServerType.SybaseASE, "ZjComerERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            this.content = GetContentHead() + GetContentFooter();

            if (nc.GetDataTable("tbl").Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "浙江柯茂逾期未结案采购单明细" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                DataTableToExcel(nc.GetDataTable("tbl"), fileFullName, true);
                AddNotify(new MailNotify());

            }
        }
    }
}
