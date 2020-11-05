using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class VarianceReportConfig : NotificationConfig
    {
        public VarianceReportConfig(DBServerType dbType, string ConnName, string notifaction) 
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(ConnName));
            this.ds = new VarianceReportDS();
            this.args = Base.GetParameter(notifaction, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select  facno, sum(CASE when convert(VARCHAR(6),soday,112) = convert(VARCHAR(6),getdate(),112) and soday=convert(char,dateadd(DD,-1,getdate()),112) then shpnum end ) as num1, ");
            sb.Append(" 0 as num2 , ");
            sb.Append(" 0 as cy1, ");
            sb.Append(" sum(CASE when convert(VARCHAR(6),soday,112) = convert(VARCHAR(6),getdate(),112) and soday<=convert(char,dateadd(DD,-1,getdate()),112) then shpnum end ) as num3, ");
            sb.Append(" 0 as num4, ");
            sb.Append(" 0 as cy2   from  bsc_groupshipment ");
            sb.Append(" GROUP BY facno ");
            Fill(sb.ToString(), ds, "shbqyt");

            sb.Remove(0, sb.Length);//清空StringBuilder（）数据

            sb.Append(" select  facno,  ");
            sb.Append(" convert(DECIMAL,sum(CASE when convert(VARCHAR(6),soday,112) = convert(VARCHAR(6),getdate(),112) and soday=convert(char,dateadd(DD,-1,getdate()),112) then shpamts end )/(case facno when 'V' then 1000000 else 10000 end  ),0) as num1, ");
            sb.Append(" 0 as num2 , ");
            sb.Append(" 0 as cy1, ");
            sb.Append(" convert(DECIMAL,sum(CASE when convert(VARCHAR(6),soday,112) = convert(VARCHAR(6),getdate(),112) and soday<=convert(char,dateadd(DD,-1,getdate()),112) then shpamts end )/(case facno when 'V' then 1000000 else 10000 end  ),0) as num3, ");
            sb.Append(" 0 as num4, ");
            sb.Append(" 0 as cy2   from  bsc_groupshipment ");
            sb.Append(" GROUP BY facno ");
            Fill(sb.ToString(), ds, "shbamt");

        }
    }
}
