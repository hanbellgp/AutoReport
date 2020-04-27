using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;

namespace KNB.config
{
    class NegativeStockConfig : NotificationConfig
    {
        public NegativeStockConfig(DBServerType dbType, string connName, string notification) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new NegativeStockDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT invmas.itcls ,invbal.itnbr ,invmas.itdsc ,invmas.unmsr1 , ");
            sb.Append(" invwh.wareh ,invwh.whdsc ,invbal.onhand1 ,invbal.onhand2 ");
            sb.Append(" FROM invbal ,invmas ,invwh ,invcls ");
            sb.Append(" WHERE ( invbal.itnbr = invmas.itnbr ) and  ");
            sb.Append(" ( invbal.facno = invwh.facno ) AND ( invbal.prono = invwh.prono ) and   ");
            sb.Append(" ( invbal.wareh = invwh.wareh ) AND ( invcls.itcls = invmas.itcls )  ");
            sb.Append(" AND (invbal.facno='C' and invbal.prono='1')  ");
            sb.Append(" AND (invbal.onhand1<0 OR invbal.onhand2<0)   ");
            Fill(sb.ToString(), ds, "tlb");
        }

    }
}
