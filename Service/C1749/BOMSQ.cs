using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class BOMSQ:NotificationContent
    {
        public BOMSQ() { }

        protected override void Init()
        {
            base.Init();

            nc = new BOMSQConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            string[] title = { "整机件号", "新料", "品名", "旧料", "品名", "使用比率","设变编号","输入人员","设变类型","确认日期" };
            int[] width = { 150, 150, 150, 150, 150, 100,120,100,100,150 };
            this.content = GetContent(nc.GetDataTable("bomtlb"), title, width);
            if (nc.GetDataTable("bomtlb").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
