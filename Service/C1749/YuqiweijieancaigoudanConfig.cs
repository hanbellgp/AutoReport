using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class YuqiweijieancaigoudanConfig:NotificationConfig
    {
        public YuqiweijieancaigoudanConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new YuqiweijieancaigoudanDS();

            this.args = Base.GetParameter(notification, this.ToString());
        }
        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select  a.pono,a.trseq,a.itnbr,c.itdsc , b.vdrno ,b.buyer,a.askdate,(a.poqy1-a.okqy1)as '未交数1',(a.poqy2-a.okqy2) as '未交数2',");
            sb.Append("c.jityn,a.poqy1,a.poqy2,a.okqy1,a.okqy2,a.dposta,s.cdesc  ");
            sb.Append("from(select  pono,trseq,itnbr,askdate,poqy1,poqy2,accqy1,okqy1,okqy2,dposta from purdta ");
            sb.Append("where facno =  '{0}' and prono = '1' and datediff(day,askdate,getdate()) >=1 and dposta < '95' and (poqy1-okqy1>0 or poqy2-okqy2>0) ");
            sb.Append("and pono in (select pono from purhad where facno = '{0}' and prono = '1' and hmark1 ='YW')) a ,purhad b , invmas c,miscode s ");
            sb.Append("where a.itnbr=c.itnbr and b.pono=a.pono and b.facno='{0}' and b.prono='1' and c.jityn='Y' and b.hmark1*=s.code  and s.ckind = '2A' ");
            sb.Append("union ");
            sb.Append("select  a.pono,a.trseq,a.itnbr,c.itdsc , b.vdrno ,b.buyer,a.askdate,(a.poqy1-a.okqy1)as '未交数1',(a.poqy2-a.okqy2) as '未交数2',");
            sb.Append("c.jityn,a.poqy1,a.poqy2,a.okqy1,a.okqy2,a.dposta,s.cdesc ");
            sb.Append("from(select  pono,trseq,itnbr,askdate,poqy1,poqy2,accqy1,okqy1,okqy2,dposta from purdta ");
            sb.Append("where facno =  '{0}' and prono = '1' and datediff(day,askdate,getdate()) >=1 and dposta < '95' and (poqy1-okqy1>0 or poqy2-okqy2>0) ");
            sb.Append("and pono in (select pono from purhad where facno = '{0}' and prono = '1' and hmark1 ='YW')) a ,purhad b , invmas c,miscode s ");
            sb.Append("where a.itnbr=c.itnbr and b.pono=a.pono and b.facno='{0}' and b.prono='1' and c.jityn='N' and b.hmark1 *=s.code  and s.ckind = '2A' ");
            Fill(String.Format(sb.ToString(), args["facno"]), ds, "tbl");
        }
    }
}
