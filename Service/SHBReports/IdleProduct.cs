using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class IdleProduct:NotificationContent
    {

        public IdleProduct()
        {
        }

        protected override void Init()
        {
            base.Init();

            this.nc = new IdleProductConfig(DBServerType.SybaseASE,"SHBERP",this.ToString());
            this.nc.InitData();
            this.nc.ConfigData();

            this.content = GetContentHead() + "<br/><br/><br/><br/>" + GetContentFooter();

            if (nc.GetDataTable("tblresult").Rows.Count > 0)
            {
                string fileFullName =Base.GetServiceInstallPath() + "\\Data\\" + "库存机无订单" + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss")+".xls";
                DataTableToExcel(nc.GetDataTable("tblresult"), GetReportName(this.ToString()), true);
                DataTableToExcel(nc.GetDataTable("tblstock"),fileFullName , true);
                AddNotify(new MailNotify());
            }

        }



    }
}
