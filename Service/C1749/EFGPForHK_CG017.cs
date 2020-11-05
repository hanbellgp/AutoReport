using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class EFGPForHK_CG017:NotificationContent
    {
        public EFGPForHK_CG017() { }

        protected override void Init()
        {
            base.Init();
            this.nc = new EFGPForHK_CG017Config(DBServerType.MSSQL, "EFGP", this.ToString());
            this.nc.InitData();
            this.nc.ConfigData();
            this.content = GetContentHead() + GetContentFooter();
            if (nc.GetDataTable("addtlb").Rows.Count > 0 && nc.GetDataTable("updatetlb").Rows.Count > 0)
            {
                string fileFullName1 = Base.GetServiceInstallPath() + "\\Data\\" + "厂商资料新增明细表" + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss") + ".xlsx";
                DataTableToExcel(nc.GetDataTable("addtlb"), fileFullName1, true);
                string fileFullName2 = Base.GetServiceInstallPath() + "\\Data\\" + "厂商资料修改明细表" + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss") + ".xlsx";
                DataTableToExcel(nc.GetDataTable("updatetlb"), fileFullName2, true);
                AddNotify(new MailNotify());

            }

        }



    }
}
