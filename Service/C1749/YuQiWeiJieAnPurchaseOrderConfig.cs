using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class YuQiWeiJieAnPurchaseOrderConfig : NotificationConfig
    {
        public YuQiWeiJieAnPurchaseOrderConfig(DBServerType dbType, string connName, string notification) {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new YQWJAPurchaseOrder();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            String sqlstr = @"
select distinct  A.vdrno as vdrno,A.vdrna as vdrna,A.itnbr as itnbr,A.itdsc as itdsc ,A.pono,A.username  ,
A.askdate ,A.askdateo,A.poqy1, A.okqy1 ,wjs1 , '' as cghf, '' as yqcs, '' as yyhf,
                case A.dposta when '10' then '未确认'
                    when '20' then '已确认'
                    when '30' then '已套印采购单'
                    when '40' then '已点收'
                    when '50' then '已验收'
                    when '95' then '自动结案'
                    when '98' then '人工结案'
                    when '99' then '作废'
                    else  ''end
from (
       select  b.vdrno ,v.vdrna ,a.itnbr ,c.itdsc ,a.pono ,d.username ,a.askdate ,a.askdateo ,
a.poqy1 ,a.okqy1, (a.poqy1-a.okqy1) as wjs1, a.dposta ,a.trseq,b.buyer,a.dposta
from (
    select  pono,trseq,itnbr,askdate,poqy1,okqy1,dposta,askdateo ,badqy1,stqy1,dposta
from purdta
where askdateo>='20160101' and
 dposta < '95' and (poqy1-okqy1>0)
and pono in
(select pono from purhad where facno = 'C' and prono = '1'
 and ( posrc in ('4','2') or posrc in ('1','3') )  )
 ) a,purhad b , invmas c, secuser d ,miscode s,purvdr v
  where  b.buyer =d.userno and a.itnbr=c.itnbr and b.pono=a.pono
 and b.hmark1*=s.code  and s.ckind = '2A' and b.vdrno=v.vdrno and c.jityn = 'N'
     ) A where A.itnbr like 'B%' or A.itnbr like '%GB%'
UNION
     select distinct f.vdrno as vdrno,f.vdrna as vdrna,f.itnbr as itnbr, purdnam.itdsc as itdsc,f.pono,f.username as username,
    f.askdate as askdate,f.askdateo as askdateo,f.poqy1, f.okqy1,f.wjs1 , '' as cghf, '' as yqcs, '' as yyhf,
                      case f.dposta when '10' then '未确认'
                    when '20' then '已确认'
                    when '30' then '已套印采购单'
                    when '40' then '已点收'
                    when '50' then '已验收'
                    when '95' then '自动结案'
                    when '98' then '人工结案'
                    when '99' then '作废'
                    else  ''end
    from (
           select b.vdrno ,v.vdrna ,a.itnbr ,c.itdsc  ,b.paycode, a.pono ,d.username  ,a.askdate ,a.askdateo ,a.dposta,
            a.poqy1 ,a.okqy1,a.accqy1,(a.poqy1-a.okqy1) as wjs1,a.srcno,a.trseq,a.dposta
    from (select  t.pono,t.trseq,t.itnbr,t.askdate,t.poqy1,okqy1,t.accqy1,t.dposta,t.askdateo ,badqy1,stqy1,purdtamap.srcno,t.dposta
    from purdta t LEFT JOIN   purdtamap on purdtamap.pono =t.pono  and purdtamap.trseq =t.trseq
    where askdateo>='20160101' and  dposta < '95' and (poqy1-okqy1>0) and t.pono in
            (select pono from purhad where facno = 'C' and prono = '1' )) a
      , purhad b , invmas c, secuser d ,miscode s,purvdr v
            where b.buyer =d.userno and a.itnbr*=c.itnbr and b.pono=a.pono  and b.facno='C' and b.prono='1' and
                  b.hmark1*=s.code and b.vdrno=v.vdrno and c.jityn = 'N') f  LEFT JOIN  purdnam on f.pono = purdnam.pono and f.trseq = purdnam.trseq
    where  ( f.itnbr = '9')

";
            Fill(sqlstr, ds, "YQWJAPO");
        }
    }
}
