using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class ZjzlConfig : NotificationConfig
    {
        public ZjzlConfig(DBServerType dbType, string connName,  string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSZjzl();
            this.reportList.Add(new ZjzlReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(this.ToString());
        }


        public override void InitData()
        {
            string sqlstr = @"select a.protype ,a.num1 ,b.num2 ,(a.num1+b.num2) as numall from
(select a.protype ,count(a.itnbr)  as num1
from (
select a.wareh '库号',w.whdsc '库别',
 (case when a.itcls in('3176','3177','3179','3180','3276','3279','3280','3376','3379','3380','3476','3480','3479','3676','3679','3680','3H76','3H79','3H80','3083')  then 'R制冷'
when a.itnbr in (select itnbr from invmas where itcls in('3576','3579','3580','4052','3A76','3A79','3A80') and itnbr<> '35302-H5233-08') then 'A机组'
when a.itcls in('3876','3879','3880','3B76','3B79','3B80','3C76','3C79','3C80','3012','3Q80')  then 'A机体'
when a.itcls in ('3776','3780','3D76','3D80','3E76','3E79','3E80','3F76','3F80','3G76','3G79','4079','4052','6053') then 'P真空'
else '其他' end) as protype,a.itnbr ,i.itdsc '品名',
varnr '制造号码',convert(varchar(6),lindate,112) '入库日期',
datediff(day,lindate,getdate()) '账龄(天)',
a.itcls+'-'+c.clsdsc '品号大类' 
from invbat a,invwh w,invmas i,invcls c,secuser s
where a.facno='C' 
and a.prono='1' 
and s.userno=w.wclerk 
and w.wareh=a.wareh 
and i.itnbr=a.itnbr 
and c.itcls=a.itcls 
and  a.itcls in (
'4079','3081','3083','3176','3177','3179','3180','3276','3279','3280',
'3376','3379','3380','3476','3480','3479','3576','3579','3580','3676',
'3679','3680','3776','3780','3876','3879','3880','3976','3979','3980',
'4052','3A76','3A79','3A80','3B76','3B79','3B80','3C76','3C79','3C80',
'3D76','3D80','3E76','3E79','3E80','3F76','3F80','3G76','3G79','3H76',
'3H79','3H80','3J76','3J79','3J80','3K76','3K79','3K80') 
and a.onhand1-a.preqy1>0 
and datediff(month,a.lindate,getdate())>6 
and datediff(day,lindate,getdate())<=365
) a  group by a.protype) a ,
(select b.protype,count(b.itnbr) as num2 from (
select a.wareh '库号',w.whdsc '库别',
 (case when a.itcls in('3176','3177','3179','3180','3276','3279','3280','3376','3379','3380','3476','3480','3479','3676','3679','3680','3H76','3H79','3H80','3083')  then 'R制冷'
when a.itnbr in (select itnbr from invmas where itcls in('3576','3579','3580','4052','3A76','3A79','3A80') and itnbr<> '35302-H5233-08') then 'A机组'
when a.itcls in('3876','3879','3880','3B76','3B79','3B80','3C76','3C79','3C80','3012','3Q80')  then 'A机体'
when a.itcls in ('3776','3780','3D76','3D80','3E76','3E79','3E80','3F76','3F80','3G76','3G79','4079','4052','6053') then 'P真空'
else '其他' end) as protype,a.itnbr ,i.itdsc '品名',
varnr '制造号码',convert(varchar(6),lindate,112) '入库日期',
datediff(day,lindate,getdate()) '账龄(天)',
a.itcls+'-'+c.clsdsc '品号大类' 
from invbat a,invwh w,invmas i,invcls c,secuser s
where a.facno='C' 
and a.prono='1' 
and s.userno=w.wclerk 
and w.wareh=a.wareh 
and i.itnbr=a.itnbr 
and c.itcls=a.itcls 
and  a.itcls in (
'4079','3081','3083','3176','3177','3179','3180','3276','3279','3280',
'3376','3379','3380','3476','3480','3479','3576','3579','3580','3676',
'3679','3680','3776','3780','3876','3879','3880','3976','3979','3980',
'4052','3A76','3A79','3A80','3B76','3B79','3B80','3C76','3C79','3C80',
'3D76','3D80','3E76','3E79','3E80','3F76','3F80','3G76','3G79','3H76',
'3H79','3H80','3J76','3J79','3J80','3K76','3K79','3K80') 
and a.onhand1-a.preqy1>0 
and datediff(month,a.lindate,getdate())>6 
and datediff(day,lindate,getdate())>365
) b group by b.protype) b
where a.protype=b.protype ";

            Fill(sqlstr, ds, "tblresult");


        }



    }
}
