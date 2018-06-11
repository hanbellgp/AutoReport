using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class MonthlyConfig : NotificationConfig
    {
        public MonthlyConfig(DBServerType dbType, string connName, string notification) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new MonthlyDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            string crmsql = @"";

            Fill(crmsql,ds,"");
        }
    }
}
