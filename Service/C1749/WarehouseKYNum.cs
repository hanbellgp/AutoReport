using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class WarehouseKYNum:NotificationContent
    {
        public WarehouseKYNum() { }
        protected override void Init()
        {
            base.Init();
            //空余储位数
            nc = new WarehouseNumConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            string[] title = { "线别", "空余储位数" };
            int[] width = { 150, 120 };
            this.content = GetContent(nc.GetDataTable("kytlb"), title, width);
            if (nc.GetDataTable("kytlb").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
