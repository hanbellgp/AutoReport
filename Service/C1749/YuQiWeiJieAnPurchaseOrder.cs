using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class YuQiWeiJieAnPurchaseOrder : NotificationContent
    {
        public YuQiWeiJieAnPurchaseOrder() { 

        }

        protected override void Init()
        {
            base.Init();
            //nc = new CRM_WeiTuiWuLiaoConfig(Core.DBServerType.MSSQL, "CRM", this.ToString());
            nc = new YuQiWeiJieAnPurchaseOrderConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            if (nc.GetReportList() != null)
            {

            }

            //string[] title = { "厂商", "厂商简称", "品号", "品名", "采购单", "姓名", "原预计交期", "预计交期", "未交数1", "未交数2", "已点收", "采购回复", "逾期次数", "原因回复" };
            //int[] width = { 150, 150, 200, 200, 200, 150, 200, 200, 100, 100, 100, 150, 150, 150 };

            this.content = GetContentHead() + GetContentFooter();
            if (nc.GetDataTable("YQWJAPO").Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "研发物料逾期未结案采购单" + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss") + ".xls";
                DataTableToExcel(nc.GetDataTable("YQWJAPO"), fileFullName, true);
                AddNotify(new MailNotify());
            }
        }
    }
}
