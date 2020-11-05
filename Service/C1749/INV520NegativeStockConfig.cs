using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class INV520NegativeStockConfig:NotificationConfig
    {
        public INV520NegativeStockConfig(DBServerType dbType, string connName, string notification) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new InvbalAgingReportDS();
            this.reportList.Add(new AccountReceivableDelayReport());
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  invmas.itcls , invbal.itnbr , invmas.itdsc , invmas.unmsr1  , ");
            sb.Append(" invwh.wareh ,invwh.whdsc ,  invbal.onhand1,invbal.onhand2 ");
            sb.Append(" FROM invbal ,invmas ,invwh ,invcls ");
            sb.Append(" WHERE  ( invbal.itnbr = invmas.itnbr ) and ");
            sb.Append(" ( invbal.facno = invwh.facno ) and ");
            sb.Append(" ( invbal.prono = invwh.prono ) and ");
            sb.Append(" ( invbal.wareh = invwh.wareh ) and ");
            sb.Append(" ( invcls.itcls = invmas.itcls) and ");
            sb.Append(" (invbal.facno='V' and invbal.prono='1') and ");
            sb.Append(" invbal.onhand1<0 ");
            Fill(sb.ToString(), ds, "tlb");
        }

    }
}
