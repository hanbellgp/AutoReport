using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class HZ_JS014Config : NotificationConfig
    {
        public HZ_JS014Config() { }

        public HZ_JS014Config(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new HZ_JS014DS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT canu,aim,DateDiff(DAY,  getdate(),time1)as delaydays,depat ");
            sb.Append(" FROM HZ_JS014,ProcessInstance ");
            sb.Append(" WHERE HZ_JS014.processSerialNumber = ProcessInstance.serialNumber ");
            sb.Append(" AND DateDiff(DAY,  getdate(),time1) <= 15 AND  DateDiff(DAY,  getdate(),time1)>= 0 ");
            sb.Append(" AND ProcessInstance.currentState <> '3' ");
            Fill(sb.ToString(), this.ds, "tbl");
        }

    }
}
