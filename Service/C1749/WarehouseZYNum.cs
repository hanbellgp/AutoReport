using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class WarehouseZYNum:NotificationContent
    {
        public WarehouseZYNum() { }

        protected override void Init()
        {
            base.Init();
            //占用储位数
            nc = new WarehouseNumConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            string[] title = { "库号", "占用储位数" };
            int[] width = { 150, 120 };
            this.content = GetContent(nc.GetDataTable("zytlb"), title, width);
            if (nc.GetDataTable("zytlb").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
