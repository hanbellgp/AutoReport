using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class StockAndInProcess_S:NotificationContent
    {
        public StockAndInProcess_S() { }

        protected override void Init()
        {
            base.Init();
            nc = new StockAndInProcessConfig_S(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            string[] title = { "机型码", "件号", "FSFL涡旋领料站", "FSSC涡旋试车站", "FSJY涡旋入库检验站", "EW01", "合计"};
            int[] width = { 120, 120, 150, 150, 150, 80, 80 };
            this.content = GetContent(nc.GetDataTable("tbl"), title, width);
            if (nc.GetDataTable("tbl").Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
    }
}
