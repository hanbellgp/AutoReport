using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class BadAcceptanceConfig : NotificationConfig
    {
        public BadAcceptanceConfig() { 
        
        }
        public BadAcceptanceConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new BadAcceptanceDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            String sqlStr = @"select a.acceptno as acceptno,a.itnbr as itnbr,s.itdsc as itdsc,a.accqy1 as accqy1,a.okqy1 as okqy1,a.badqy1 as badqy1 from puracd a ,invmas s 
                            where datediff(mm,a.recivedate,getdate())=1 and  s.itnbr = a.itnbr and a.badqy1 > 0";
            Fill(sqlStr, ds, "tlbBad");
        }
    }
}
