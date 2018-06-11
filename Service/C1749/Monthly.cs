using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class Monthly : NotificationContent
    {
        public Monthly() 
        { 

        }

        protected override void Init()
        {
            base.Init();

            nc = new StatisticalReportConfig(DBServerType.SybaseASE, "CRM", this.ToString());

            nc.InitData();
            nc.ConfigData();
            DataTable dt = addERPdata();
            dt.DefaultView.RowFilter = "BQ023C <>''";
            dt = dt.DefaultView.ToTable();
            this.content = GetContentHead() + GetContentFooter();
            if (dt.Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "集团免费服务金额责任归属月统计表" + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss") + ".xlsx";
                //DataTableToExcel(nc.GetDataTable("SRtlb"), fileFullName, true);
                DataTableToExcel(dt, fileFullName, true);
                AddNotify(new MailNotify());

            }
        }

        protected DataTable addERPdata() {
            return null;
        }
    }
}
