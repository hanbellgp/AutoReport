using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class Heyuedanjiaweilailianggeyue_K : NotificationContent
    {
        public Heyuedanjiaweilailianggeyue_K()
        {
        }
        protected override void Init()
        {
            base.Init();
            nc = new HeyuedanjiaweilailianggeyueConfig_K(DBServerType.SybaseASE, "ComerERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            this.content = GetContentHead() + GetContentFooter();

            if (nc.GetDataTable("tblresult6").Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "柯茂合约单价未来2个月内到期统计表" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                DataTableToExcel(nc.GetDataTable("tblresult6"), fileFullName, true);
                AddNotify(new MailNotify());

            }
        }
    }
}
