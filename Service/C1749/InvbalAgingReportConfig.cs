using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class InvbalAgingReportConfig:NotificationConfig
    {
        public InvbalAgingReportConfig(DBServerType dbType, string connName, string notification) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new InvbalAgingReportDS();
            this.reportList.Add(new AccountReceivableDelayReport());
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT invbal.wareh,invbal.itnbr,invmas.itdsc,invbal.onhand1,invbal.lindate,datediff(dd,invbal.lindate,getdate()) as aging FROM invbal ");
            sb.Append(" LEFT JOIN invmas ON invbal.itnbr = invmas.itnbr AND invbal.itcls = invmas.itcls ");
            sb.Append(" WHERE invbal.facno = 'C' AND invbal.prono = '1' ");
            sb.Append(" AND invbal.itcls IN {0} ");
            sb.Append(" AND invbal.wareh in {1} ");
            sb.Append(" AND invbal.onhand1>0 ");
            sb.Append(" and datediff(dd,invbal.lindate,getdate())>3 ");
            sb.Append(" order by invbal.lindate asc ");

            Fill(String.Format(sb.ToString(), args["itcls"],args["wareh"]), ds, "tlb");
        }
    }
}
