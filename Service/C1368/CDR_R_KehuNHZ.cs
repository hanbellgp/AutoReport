using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{

    public class CDR_R_KehuNHZ : NotificationContent
    {

        public CDR_R_KehuNHZ()
        {

        }

        protected override void Init()
        {
            base.Init();
            nc = new CDR_R_KehuNHZConfig(Core.DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetDataTable("tblresult").Rows.Count > 0)
            {
                this.content = GetContentHead() + "<br/><br/><br/><br/>" + GetContentFooter();
                DataTableToExcel(nc.GetDataTable("tblresult"), GetReportName(this.ToString()), true);
                AddNotify(new MailNotify());
            }

        }



    }
}
