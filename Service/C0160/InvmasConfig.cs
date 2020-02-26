using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;

namespace C0160
{
    public class InvmasConfig : NotificationConfig
    {

        public InvmasConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new InvmasDS();
            this.args = Base.GetParameter(this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select m.itnbr,m.itdsc,m.spdsc,m.itcls,c.clsdsc,m.itclscode,");
            sb.Append("case c.itclscode ");
            sb.Append(" when '1' then '1405' ");
            sb.Append(" when '2' then '1413' ");
            sb.Append(" when '3' then '1403' ");
            sb.Append(" when '4' then '1403' ");
            sb.Append(" when '8' then '费用或固资' ");
            sb.Append(" when 'A' then '1411' ");
            sb.Append(" when 'B' then '1412' ");
            sb.Append(" else '' end as accno,");
            sb.Append(" case c.itclscode  ");
            sb.Append(" when '1' then '成品' ");
            sb.Append(" when '2' then '半成品' ");
            sb.Append(" when '3' then '原料' ");
            sb.Append(" when '4' then '物料' ");
            sb.Append(" when '8' then '列管资产' ");
            sb.Append(" when 'A' then '包装物' ");
            sb.Append(" when 'B' then '低值易耗品' ");
            sb.Append(" else '' end as accna ");
            sb.Append("from invmas m,invcls c where m.itcls = c.itcls and  convert(char(8),m.indate,112) = convert(char(8),dateadd(day,-1,getDate()),112)");
            Fill(sb.ToString(), ds, "tbl");
        }
    }
}
