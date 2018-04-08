using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class StandCostAbnormality : NotificationContent
    {
        public StandCostAbnormality()
        {
        }

        protected override void Init()
        {
            base.Init();

            this.nc = new StandCostAbnormalityConfig(Core.DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();

            if (nc.GetDataTable("tblsc").Rows.Count > 0)
            {

                string[] title = { "大类", "来源", "件号", "品名", "规格", "进货", "托工", "料", "工", "费", "托" };
                int[] width = { 50, 40, 150, 180, 180, 80, 80, 80, 80, 80, 80 };
                this.content = GetContent(nc.GetDataTable("tblsc"), title, width);
                string filename = GetReportName(this.ToString());
                DataTableToExcel(this.nc.GetDataTable("tblsc"), filename, true);
                AddNotify(new MailNotify());

            }


        }



    }
}
