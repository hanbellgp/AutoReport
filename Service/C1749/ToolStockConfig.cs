using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class ToolStockConfig:NotificationConfig
    {
        public ToolStockConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType,Base.GetDBConnectionString(connName));
            this.ds = new ToolStockDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select b.itnbr,m.itdsc,m.spdsc,b.wareh,b.trdate,b.lindate,datediff(dd,b.trdate,getdate()) AS 'dzday',b.onhand1 from invbal b,invmas m  where   ");
            sb.Append(" b.itnbr = m.itnbr AND facno = 'C' AND prono = '1' ");
            sb.Append(" AND wareh in ('ASRSD1','ASRSD2','CT03') AND b.onhand1>0 and datediff(dd,loudate,getdate())>30  ");
            sb.Append(" ORDER BY b.trdate desc  ");
            Fill(sb.ToString(), ds, "dbtlb");
        }
    }
}
