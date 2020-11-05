using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class BscShipment : NotificationContent
    {
        public BscShipment() { }

        protected override void Init()
        {
            base.Init();
            
            nc = new BscShipmentConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            DataTable tlb = nc.GetDataTable("testtlb");
            
            string[] title = { "产品别", "出货台数", "出货金额", "订单台数", "订单金额", "日期"};
            int[] width = {100,100,100,100,100,100};
            this.content = GetContent(tlb, title, width);
            if (tlb.Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }

        }
    }
}
