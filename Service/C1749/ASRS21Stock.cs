using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class ASRS21Stock:NotificationContent
    {
        public ASRS21Stock() { }
        protected override void Init()
        {
            base.Init();
            nc = new ASRS21StockConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            if (nc.GetReportList().Count > 0)
            {
                SetAttachment();
            }
            DataTable dt = nc.GetDataTable("tbl");
            this.content = GetContentHead() + GetContentFooter();
            if (dt.Rows.Count > 0 && dt.Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "ASRS21库存明细" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                DataTableToExcel(dt, fileFullName, true);
                AddNotify(new MailNotify());
            }
        }
    }
}
