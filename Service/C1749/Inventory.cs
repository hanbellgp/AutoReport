using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class Inventory : NotificationContent
    {
        public Inventory() { }

        protected override void Init()
        {
            base.Init();
            nc = new InventoryConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            string[] title = { "公司别","产品别", "现库存", "目标库存", "差异", "0-6月库存", "7-12月库存", "1-2年库存", "2-3年库存", "3年以上", "单位" };
            int[] width = { 100,100, 100, 100, 100, 100, 100, 100, 100, 100, 100 };
            this.content = GetContent(nc.GetDataTable("zbtlb1"), title, width);
            DataTable dt = nc.GetDataTable("mxtlb");

            if (nc.GetDataTable("zbtlb1").Rows.Count > 0 && dt.Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "成品库库存数量明细表" + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss") + ".xlsx";
                DataTableToExcel(dt, fileFullName, true);
                AddNotify(new MailNotify());
            }
        }
    }

}
