using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;

namespace C0160
{
    public class NegativeStockConfig : NotificationConfig
    {

        public NegativeStockConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new NegativeStockDS();
            this.args = Base.GetParameter(this.ToString());
        }

        public override void InitData()
        {
            Fill("SELECT  b.wareh,w.whdsc,b.itnbr,m.itdsc,m.spdsc,c.itcls,c.clsdsc,b.onhand1 FROM invbal b ,invmas m, invcls c,invwh w " +
                "WHERE b.wareh=w.wareh AND b.itnbr=m.itnbr AND m.itcls = c.itcls AND b.onhand1 < 0.0 ORDER BY b.wareh,b.itnbr ", ds, "tbl");
        }

    }
}
