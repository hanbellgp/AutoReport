using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class TCHUConfig : NotificationConfig
    {
        public TCHUConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSTCHU();
            //this.reportList.Add(new BxdmxReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(notification, this.ToString());
        }


        public override void InitData()
        {
            string sqlstr = @"select x.*
from 
(
SELECT     
     a.shpno ,       
    a.shpdate ,
	 a.depno,  
	f.depname,  
       a.cusno ,       
    a.coin ,     
      a.ratio ,   
        b.trseq ,   
        b.cdrno ,        
   b.ctrseq ,      
     b.itnbr ,    
       b.itdsc ,     
      b.spdsc ,      
     b.itnbrcus ,     
      b.unpris ,     
      b.armqy ,      
     b.shpamts ,  
         d.cusna ,   
      space(4) as cp_lcoin,   
        a.mancode , 
			g.username,               
  case substring(s.judco,5,1) when '1' then unmsr1 when '3' then s.unmsr2 when '5'
 then s.unmsr3 end as cp_armunmsr   ,
c.cuspono,c.tocdrno,e.dmark1
 FROM   cdrhad a, cdrdta b, cdrhmas c, cdrcus d, cdrdmas e, invmas  s,misdept f,secuser g
   WHERE ( a.facno = b.facno ) and  a.depno=f.depno    and g.userno=a.mancode  and  
( a.shpno = b.shpno ) and       
   ( a.cusno = d.cusno ) and     
     ( b.itnbr = s.itnbr )    
AND (
convert(varchar(6),a.shpdate,112)  = convert(varchar(6),dateadd(month,-1,getdate()),112)
and a.houtsta = 'Y') and a.cusno not in ('SGD00088','SJS00254','SSD00107')
and c.cdrno=e.cdrno and b.ctrseq=e.trseq and b.cdrno = c.cdrno
and a.depno='1T100'

union all
SELECT     
     a.shpno ,       
    a.shpdate ,
	 a.depno,  
	f.depname,  
       a.cusno ,       
    a.coin ,     
      a.ratio ,   
        b.trseq ,   
        b.cdrno ,        
   b.ctrseq ,      
     b.itnbr ,    
       b.itdsc ,     
      b.spdsc ,      
     b.itnbrcus ,     
      b.unpris ,     
      b.armqy ,      
     b.shpamts ,  
         d.cusna ,   
      space(4) as cp_lcoin,   
        a.mancode , 
			g.username,               
  case substring(s.judco,5,1) when '1' then unmsr1 when '3' then s.unmsr2 when '5'
 then s.unmsr3 end as cp_armunmsr   ,
space(4) as cuspono,space(8) as tocdrno,a.hmark2
 FROM   cdrhad a, cdrdta b, cdrcus d,   invmas  s,misdept f,secuser g
   WHERE ( a.facno = b.facno ) and  a.depno=f.depno    and g.userno=a.mancode  and  
( a.shpno = b.shpno ) and       
   ( a.cusno = d.cusno ) and     
     ( b.itnbr = s.itnbr )    
AND (
convert(varchar(6),a.shpdate,112)  = convert(varchar(6),dateadd(month,-1,getdate()),112)

 and a.houtsta = 'Y') 
and a.depno=''
and b.cdrno='9'

union all
SELECT     
     a.bakno ,       
    a.bakdate ,
	 a.depno,  
	f.depname,  
       a.cusno ,       
    a.coin ,     
      a.ratio ,   
        b.trseq ,   
        b.cdrno ,        
   b.strseq ,      
     b.itnbr ,    
       b.itdsc ,     
      b.spdsc ,      
     b.itnbrcus ,     
      b.unpris*(-1) ,     
      b.bshpqy1*(-1) ,      
     b.bakamts*(-1) ,  
         d.cusna ,   
      space(4) as cp_lcoin,   
        a.mancode , 
			g.username,               
  case substring(s.judco,5,1) when '1' then unmsr1 when '3' then s.unmsr2 when '5'
 then s.unmsr3 end as cp_armunmsr   ,
c.cuspono,c.tocdrno ,e.dmark1
 FROM   cdrbhad a, cdrbdta b, cdrhmas c, cdrcus d,  cdrdmas e,invmas  s,misdept f,secuser g
   WHERE ( a.facno = b.facno ) and  a.depno=f.depno    and g.userno=a.mancode  and  
( a.bakno = b.bakno ) and       
   ( a.cusno = d.cusno ) and     
     ( b.itnbr = s.itnbr )    
AND (
convert(varchar(6),a.bakdate,112)  = convert(varchar(6),dateadd(month,-1,getdate()),112)
and a.baksta = 'Y')  and a.cusno not in ('SGD00088','SJS00254','SSD00107')
and  c.cdrno=e.cdrno and b.ctrseq=e.trseq and b.cdrno = c.cdrno
and a.depno=''

) x
where 
x.itnbr in ( 
select itnbr from invmas where itcls in('3876','3879','3880','3B76','3B79','3B80','3C76','3C79','3C80','3K76','3K79','3K80',
'3Q76','3Q79','3Q80','3R76','3R79','3R80','3S76','3S79','3S80','3576','3579','3580','3A76','3A79','3A80','3J76','3J79','3J80',
'3M76','3M79','3M80') 
)     
";

            Fill(sqlstr, ds, "TCHU");


        }

        ////移除数据表中多余字段
        //public override void ConfigData()
        //{
        //    ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq"]);
        //    ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq2"]);
        //}


    }
}
