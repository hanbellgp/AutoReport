using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace C1491
{
    class CRMREPPALogConfig : Hanbell.AutoReport.Config.NotificationConfig
    {
        public CRMREPPALogConfig() { }

        public CRMREPPALogConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new CRMREPPALogDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            throw new NotImplementedException();
        }
    }
}
