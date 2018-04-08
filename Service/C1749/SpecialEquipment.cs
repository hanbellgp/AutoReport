using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class SpecialEquipment : NotificationContent
    {
        public SpecialEquipment() { 
        
        }
        
        protected override void Init()
        {
            base.Init();
            nc = new SpecialEquipmentConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();


            string[] title = { "厂商代号", "厂商简称", "点收日期", "件号", "品名", "数量" };
            int[] width = { 150, 150, 150, 150, 150, 150 };
            this.content = GetContent(nc.GetDataTable("tlbequipment"), title, width);
            if (nc.GetDataTable("tlbequipment").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
