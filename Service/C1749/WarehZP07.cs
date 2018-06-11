using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class WarehZP07:NotificationContent
    {
        public WarehZP07() 
        {
        }

        protected override void Init()
        {
            base.Init();
            nc = new WarehZP07Config(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            string[] title = { "库号", "品号", "品名", "数量", "单位"};
            int[] width = { 100, 100, 100, 100, 100};
            this.content = GetContent(nc.GetDataTable("ZP07tlb"), title, width);
            if (nc.GetDataTable("ZP07tlb").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
            
        }
    }
}
