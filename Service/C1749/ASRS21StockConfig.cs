using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class ASRS21StockConfig:NotificationConfig
    {
        public ASRS21StockConfig(DBServerType dbType, string ConnName, string notifaction) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(ConnName));
            this.ds = new ASRS21StockDS();
            this.args = Base.GetParameter(notifaction, this.ToString());
        }
        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select d.wareh,d.loc,d.itnbr, s.itdsc,plt_qty as onhand1 from  ");
            sb.Append(" asrs_tb_loc_dtl d ,invmas s ");
            sb.Append(" where d.itnbr=s.itnbr and d.wareh = 'ASRS21'");
            Fill(sb.ToString(), ds, "tbl");
        }
    }
}
