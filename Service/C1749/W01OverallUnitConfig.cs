using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class W01OverallUnitConfig:NotificationConfig
    {
        public W01OverallUnitConfig(DBServerType dbType, string connName, string notification) {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new W01OverallUnitDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
                //定于每月初发上月的数据
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT yearmon,trtype,iocode,facno,prono,wareh,itnbr,fixnr,varnr,itcls,itclscode,trnqy1  ");
            sb.Append(" from invmon_bat WHERE facno = 'C' AND prono = '1' and trnqy1 > 0 AND  wareh = 'W01' and trtype = 'ZZZ' ");
            sb.Append(" and yearmon = convert(varchar(6),dateadd(month,-1,getdate()),112) ");
            Fill(sb.ToString(), ds, "dbtlb");
        }
    }
}
