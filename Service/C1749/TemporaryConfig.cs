using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class TemporaryConfig : NotificationConfig
    {
        public TemporaryConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new BscShipmentDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            StringBuilder sbday = new StringBuilder();
            //取當日的數據
            sbday.Append(" select protype,sum(shpnum) as shpnum, Convert(decimal(18,2),sum(shpamts)/10000) as shpamts,sum(ordnum) as ordnum, Convert(decimal(18,2),sum(ordamts)/10000) as ordamts,'11' as soday ");
            sbday.Append(" from bsc_groupshipment where year(soday)=2018 and month(soday)=11 and soday<='20181129' GROUP BY protype ");
            Fill(sbday.ToString(), ds, "jtdaytlb");

            StringBuilder sbmonth = new StringBuilder();
            //取當日的數據
            sbmonth.Append(" select protype,sum(shpnum) as shpnum, Convert(decimal(18,2),sum(shpamts)/10000) as shpamts,sum(ordnum) as ordnum,Convert(decimal(18,2),sum(ordamts)/10000) as ordamts , '11' as soday ");
            sbmonth.Append(" from bsc_groupshipment where year(soday)=2018 and month(soday)<=10 GROUP BY protype ");
            Fill(sbmonth.ToString(), ds, "jtmonthtlb");
        }

        
    }
}
