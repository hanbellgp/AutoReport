using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class BadAcceptance : NotificationContent
    {
        public BadAcceptance() { 
        
        }

        protected override void Init()
        {
            base.Init();

            nc = new BadAcceptanceConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            string[] title = { "点收编号", "品号", "品名", "点收数量", "验收数量", "不良数" };
            int[] width = { 150, 100, 100, 100, 100,100 };
            this.content = GetContent(nc.GetDataTable("tlbBad"), title, width);
            if (nc.GetDataTable("tlbBad").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
