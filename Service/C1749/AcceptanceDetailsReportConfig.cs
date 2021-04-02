using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class AcceptanceDetailsReportConfig:NotificationConfig
    {
        public AcceptanceDetailsReportConfig(DBServerType dbType, string connName, string notification) 
        {
            PrepareDBUtil(dbType,Base.GetDBConnectionString(connName));
            this.ds = new AcceptanceDetailsReportDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            //每月月底发送本月的验收明细
            sb.Append(" select a.acceptno,a.vdrno,p.vdrna,a.cfmuserno,b.username,a.acceptdate from puracd a,secuser b,purvdr p  ");
            sb.Append(" where b.userno = a.cfmuserno AND a.vdrno = p.vdrno and  ");
            sb.Append(" a.facno = 'C' AND a.prono = '1' AND convert(VARCHAR(6),a.acceptdate,112)=convert(VARCHAR(6),getdate(),112) ");
            Fill(sb.ToString(), ds, "dbtlb");
        }
    }
}
