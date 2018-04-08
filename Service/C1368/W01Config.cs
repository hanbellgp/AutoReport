﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class W01Config : NotificationConfig
    {
        public W01Config(DBServerType dbType, string connName,string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSW01();
            //this.reportList.Add(new BxdmxReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(this.ToString());
        }


        public override void InitData()
        {
            string sqlstr = @"select h.protype,
f.trnqys1,g.trnqys2,
cast( cast( (sum(f.trnqys1)-sum(g.trnqys2))/sum(f.trnqys1) as decimal(16,4) ) * 100 as decimal(16,2) )  as rate1 ,
h.trnqys3,i.trnqys4, 
cast( cast( (sum(h.trnqys3)-sum(i.trnqys4))/sum(h.trnqys3) as decimal(16,4) ) * 100 as decimal(16,2) )  as rate2
 from
(select a.protype,sum(a.trnqys1) as trnqys1
from
(select 
sum(trnqys) as trnqys1,itcls,
    (case when itcls in ('3176','3177','3179','3180','3276','3279','3280','3083')
          then 'R制冷'
          when itcls in ('3576','3579','3580','4052','3676','3679','3680')
          then 'A机组'
          when itcls in('3886','3889','3890','3876','3879','3880','3976','3979','3980')
          then 'A机体'
          when itcls in('3776','3780','4079','4052','6053','3779','3A76','3A79','3A80')
          then 'P真空'  end )  as protype 
from invmon
where yearmon=left(convert(varchar(8),DATEADD(month,-1,getdate()),112),6) 
and trtype='ZZZ' and wareh in ('W01' ,'EW01')  and trnqys>0
group by itcls ) a
group by a.protype) f,
(select b.protype,sum(b.trnqys2) as trnqys2
from
(select sum(trnqys) as trnqys2 ,itcls,
    (case when itcls in ('3176','3177','3179','3180','3276','3279','3280','3083')
          then 'R制冷'
          when itcls in ('3576','3579','3580','4052','3676','3679','3680')
          then 'A机组'
          when itcls in('3886','3889','3890','3876','3879','3880','3976','3979','3980')
          then 'A机体'
          when itcls in('3776','3780','4079','4052','6053','3779','3A76','3A79','3A80')
          then 'P真空'  end )  as protype 
from invmonh
where yearmon=left(convert(varchar(8),DATEADD(month,-13,getdate()),112),6) 
and trtype='ZZZ' and wareh in ('W01' ,'EW01') and trnqys>0
group by itcls
) b 
group by b.protype) g,
(select d.protype,sum(d.trnqys3) as trnqys3
from
(select sum(trnqys) as trnqys3 ,itcls,
    (case when itcls in ('3176','3177','3179','3180','3276','3279','3280','3083')
          then 'R制冷'
          when itcls in ('3576','3579','3580','4052','3676','3679','3680')
          then 'A机组'
          when itcls in('3886','3889','3890','3876','3879','3880','3976','3979','3980')
          then 'A机体'
          when itcls in('3776','3780','4079','4052','6053','3779','3A76','3A79','3A80')
          then 'P真空'  end  )  as protype 
from invmon
where yearmon=left(convert(varchar(8),DATEADD(month,-1,getdate()),112),6) 
and trtype='TR1' and wareh in ('W01' ,'EW01') and trnqys>0
group by itcls
) d 
group by d.protype) h,

(select e.protype,sum(e.trnqys4) as trnqys4
from
(select sum(trnqys) as trnqys4 ,itcls,
    (case when itcls in ('3176','3177','3179','3180','3276','3279','3280','3083')
          then 'R制冷'
          when itcls in ('3576','3579','3580','4052','3676','3679','3680')
          then 'A机组'
          when itcls in('3886','3889','3890','3876','3879','3880','3976','3979','3980')
          then 'A机体'
          when itcls in('3776','3780','4079','4052','6053','3779','3A76','3A79','3A80')
          then 'P真空'  end  )  as protype 
from invmonh
where yearmon=left(convert(varchar(8),DATEADD(month,-13,getdate()),112),6) 
and trtype='TR1' and wareh in ('W01' ,'EW01') and trnqys>0
group by itcls
) e 
group by e.protype) i
where f.protype=g.protype
 and f.protype=h.protype and f.protype=i.protype
group by h.protype,i.protype,g.protype,f.protype


union all 



select h.protype,
f.trnqys1,g.trnqys2,
cast( cast( (sum(f.trnqys1)-sum(g.trnqys2))/sum(f.trnqys1) as decimal(16,4) ) * 100 as decimal(16,2) )  as rate1 ,
h.trnqys3,i.trnqys4, 
cast( cast( (sum(h.trnqys3)-sum(i.trnqys4))/sum(h.trnqys3) as decimal(16,4) ) * 100 as decimal(16,2) )  as rate2
 from
(select a.protype,sum(a.trnqys1) as trnqys1
from
(select 
sum(trnqys) as trnqys1,itcls,
    '柯茂' as protype 
from comererp..invmon
where yearmon=left(convert(varchar(8),DATEADD(month,-1,getdate()),112),6) 
and trtype='ZZZ' and wareh in ('W01' ,'EW01')  and trnqys>0
and itcls in ('3H76','3H79','3H80','3W76','3W79','3W80')
group by itcls ) a
group by a.protype) f,
(select b.protype,sum(b.trnqys2) as trnqys2
from
(select sum(trnqys) as trnqys2 ,itcls,
    '柯茂'  as protype 
from comererp..invmonh
where yearmon=left(convert(varchar(8),DATEADD(month,-13,getdate()),112),6) 
and trtype='ZZZ' and wareh in ('W01' ,'EW01') and trnqys>0
and itcls in ('3H76','3H79','3H80','3W76','3W79','3W80')
group by itcls
) b 
group by b.protype) g,
(select d.protype,sum(d.trnqys3) as trnqys3
from
(select sum(trnqys) as trnqys3 ,itcls,
    '柯茂'  as protype 
from comererp..invmon
where yearmon=left(convert(varchar(8),DATEADD(month,-1,getdate()),112),6) 
and trtype='TR1' and wareh in ('W01' ,'EW01') and trnqys>0
and itcls in ('3H76','3H79','3H80','3W76','3W79','3W80')
group by itcls
) d 
group by d.protype) h,

(select e.protype,sum(e.trnqys4) as trnqys4
from
(select sum(trnqys) as trnqys4 ,itcls,
    '柯茂'  as protype 
from comererp..invmonh
where yearmon=left(convert(varchar(8),DATEADD(month,-13,getdate()),112),6) 
and trtype='TR1' and wareh in ('W01' ,'EW01') and trnqys>0
and itcls in ('3H76','3H79','3H80','3W76','3W79','3W80')
group by itcls
) e 
group by e.protype) i
where f.protype=g.protype
 and f.protype=h.protype and f.protype=i.protype
group by h.protype,i.protype,g.protype,f.protype


union all 



select h.protype,
f.trnqys1,g.trnqys2,
cast( cast( (sum(f.trnqys1)-sum(g.trnqys2))/sum(f.trnqys1) as decimal(16,4) ) * 100 as decimal(16,2) )  as rate1 ,
h.trnqys3,i.trnqys4, 
cast( cast( (sum(h.trnqys3)-sum(i.trnqys4))/sum(h.trnqys3) as decimal(16,4) ) * 100 as decimal(16,2) )  as rate2
 from
(select a.protype,sum(a.trnqys1) as trnqys1
from
(select 
sum(trnqys) as trnqys1,itcls,
    '汉声' as protype 
from hansonerp..invmon
where yearmon=left(convert(varchar(8),DATEADD(month,-1,getdate()),112),6) 
and trtype='ZZZ' and wareh in ('1001')  and trnqys>0
and itclscode='1'
group by itcls ) a
group by a.protype) f,
(select b.protype,sum(b.trnqys2) as trnqys2
from
(select sum(trnqys) as trnqys2 ,itcls,
    '汉声'  as protype 
from hansonerp..invmonh
where yearmon=left(convert(varchar(8),DATEADD(month,-13,getdate()),112),6) 
and trtype='ZZZ' and wareh in ('1001') and trnqys>0
and itclscode='1'
group by itcls
) b 
group by b.protype) g,
(select d.protype,sum(d.trnqys3) as trnqys3
from
(select sum(trnqys) as trnqys3 ,itcls,
    '汉声'  as protype 
from hansonerp..invmon
where yearmon=left(convert(varchar(8),DATEADD(month,-1,getdate()),112),6) 
and trtype='TR1' and wareh in ('1001') and trnqys>0
and itclscode='1'
group by itcls
) d 
group by d.protype) h,

(select e.protype,sum(e.trnqys4) as trnqys4
from
(select sum(trnqys) as trnqys4 ,itcls,
    '汉声'  as protype 
from hansonerp..invmonh
where yearmon=left(convert(varchar(8),DATEADD(month,-13,getdate()),112),6) 
and trtype='TR1' and wareh in ('1001') and trnqys>0
and itclscode='1'
group by itcls
) e 
group by e.protype) i
where f.protype=g.protype
 and f.protype=h.protype and f.protype=i.protype
group by h.protype,i.protype,g.protype,f.protype
";

            Fill(sqlstr, ds, "W01");


        }

        ////移除数据表中多余字段
        //public override void ConfigData()
        //{
        //    ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq"]);
        //    ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq2"]);
        //}


    }
}
