using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class AccountReceivableDelayForSalerConfig : AccountReceivableDelayConfig
    {

        public AccountReceivableDelayForSalerConfig(DBServerType dbType, string connName,string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSAccountReceivableDelay();
            this.reportList.Add(new AccountReceivableDelayReport());
            this.args = Base.GetParameter(notification,this.ToString());
        }

    }
}
