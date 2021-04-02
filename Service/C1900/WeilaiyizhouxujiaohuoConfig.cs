using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class WeilaiyizhouxujiaohuoConfig : NotificationConfig
    {
        public WeilaiyizhouxujiaohuoConfig()
        { 
        }
        public WeilaiyizhouxujiaohuoConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new WeilaiyizhouxujiaohuoDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select h.pono,d.trseq,d.itnbr,c.itdsc,h.vdrno,m.cdesc,h.buyer,  convert(varchar(10),d.askdate,111) as askdate, ");
            sb.Append(" (d.poqy1-d.okqy1) as '未交数1',(d.poqy2-d.okqy2) as '未交数2' ,c.jityn,d.poqy1,d.poqy2,d.okqy1,d.okqy2,d.dposta ");
            sb.Append("from purdta d,purhad h , invmas c, miscode m WHERE h.facno = d.facno  AND h.prono = d.prono ");
            sb.Append("AND h.pono = d.pono  and d.itnbr=c.itnbr  and h.vdrno=m.code ");
            sb.Append("and h.facno =  '{0}' ");
            sb.Append("and h.prono = '1' ");
            sb.Append("and datediff(day,d.askdate,getdate()) >=-10  and d.dposta < '95' ");
            sb.Append("and (d.poqy1-d.okqy1>0 or d.poqy2-d.okqy2>0) ");
            sb.Append("and c.jityn='N' and m.ckind='PJ'");

            Fill(String.Format(sb.ToString(),args["facno"]), ds, "tblresult1");
        }
    }
}
