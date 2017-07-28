using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class CHUConfig : NotificationConfig
    {
        public CHUConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSCHU();
            //this.reportList.Add(new BxdmxReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(this.ToString());
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
 FROM   gzerp..cdrhad a, gzerp..cdrdta b, gzerp..cdrhmas c, gzerp..cdrcus d, gzerp..cdrdmas e,  invmas  s,gzerp..misdept f,gzerp..secuser g
   WHERE ( a.facno = b.facno ) and  a.depno=f.depno    and g.userno=a.mancode  and  
( a.shpno = b.shpno ) and       
   ( a.cusno = d.cusno ) and     
     ( b.itnbr = s.itnbr )    
AND (
convert(varchar(6),a.shpdate,112)  = convert(varchar(6),dateadd(month,-1,getdate()),112)

 and a.houtsta = 'Y') 
and c.cdrno=e.cdrno and b.ctrseq=e.trseq and b.cdrno = c.cdrno
and a.depno<>'1E210' and a.depno<>'1D200' and a.depno<>'1C200' and a.depno<>'1A000' and a.depno<>'1B200'
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
c.cuspono,c.tocdrno,e.dmark1
 FROM   njerp..cdrhad a, njerp..cdrdta b, njerp..cdrhmas c, njerp..cdrcus d,njerp..cdrdmas e,  invmas  s,njerp..misdept f,njerp..secuser g
   WHERE ( a.facno = b.facno ) and  a.depno=f.depno    and g.userno=a.mancode  and  
( a.shpno = b.shpno ) and       
   ( a.cusno = d.cusno ) and     
     ( b.itnbr = s.itnbr )    
AND (
convert(varchar(6),a.shpdate,112)  = convert(varchar(6),dateadd(month,-1,getdate()),112)

 and a.houtsta = 'Y') 
and c.cdrno=e.cdrno and b.ctrseq=e.trseq and b.cdrno = c.cdrno
and a.depno<>'1E210' and a.depno<>'1D200' and a.depno<>'1C200' and a.depno<>'1A000' and a.depno<>'1B200'
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
c.cuspono,c.tocdrno,e.dmark1
 FROM   jnerp..cdrhad a, jnerp..cdrdta b, jnerp..cdrhmas c, jnerp..cdrcus d,jnerp..cdrdmas e,  invmas  s,jnerp..misdept f,jnerp..secuser g
   WHERE ( a.facno = b.facno ) and  a.depno=f.depno    and g.userno=a.mancode  and  
( a.shpno = b.shpno ) and       
   ( a.cusno = d.cusno ) and     
     ( b.itnbr = s.itnbr )    
AND (
convert(varchar(6),a.shpdate,112) = convert(varchar(6),dateadd(month,-1,getdate()),112)

 and a.houtsta = 'Y') 
and c.cdrno=e.cdrno and b.ctrseq=e.trseq and b.cdrno = c.cdrno
and a.depno<>'1E210' and a.depno<>'1D200' and a.depno<>'1C200' and a.depno<>'1A000' and a.depno<>'1B200'
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
and a.depno<>'1E210' and a.depno<>'1D200' and a.depno<>'1C200' and a.depno<>'1A000' and a.depno<>'1B200'

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
      b.barmqy*(-1) ,      
     b.bakamts*(-1) ,  
         d.cusna ,   
      space(4) as cp_lcoin,   
        a.mancode , 
			g.username,               
  case substring(s.judco,5,1) when '1' then unmsr1 when '3' then s.unmsr2 when '5'
 then s.unmsr3 end as cp_armunmsr   ,
c.cuspono,c.tocdrno,e.dmark1
 FROM   gzerp..cdrbhad a, gzerp..cdrbdta b, gzerp..cdrhmas c, gzerp..cdrcus d, gzerp..cdrdmas e, gzerp..invmas  s,gzerp..misdept f,gzerp..secuser g
   WHERE ( a.facno = b.facno ) and  a.depno=f.depno    and g.userno=a.mancode  and  
( a.bakno = b.bakno ) and       
   ( a.cusno = d.cusno ) and     
     ( b.itnbr = s.itnbr )    
AND (
convert(varchar(6),a.bakdate,112) = convert(varchar(6),dateadd(month,-1,getdate()),112)

 and a.baksta = 'Y') 
and c.cdrno=e.cdrno and b.ctrseq=e.trseq and b.cdrno = c.cdrno
and a.depno<>'1E210' and a.depno<>'1D200' and a.depno<>'1C200' and a.depno<>'1A000' and a.depno<>'1B200'

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
      b.barmqy*(-1) ,      
     b.bakamts*(-1) ,  
         d.cusna ,   
      space(4) as cp_lcoin,   
        a.mancode , 
			g.username,               
  case substring(s.judco,5,1) when '1' then unmsr1 when '3' then s.unmsr2 when '5'
 then s.unmsr3 end as cp_armunmsr   ,
c.cuspono,c.tocdrno,e.dmark1
 FROM   njerp..cdrbhad a, njerp..cdrbdta b, njerp..cdrhmas c, njerp..cdrcus d, njerp..cdrdmas e, njerp..invmas  s,njerp..misdept f,njerp..secuser g
   WHERE ( a.facno = b.facno ) and  a.depno=f.depno    and g.userno=a.mancode  and  
( a.bakno = b.bakno ) and       
   ( a.cusno = d.cusno ) and     
     ( b.itnbr = s.itnbr )    
AND (
convert(varchar(6),a.bakdate,112) = convert(varchar(6),dateadd(month,-1,getdate()),112)

 and a.baksta = 'Y') 
and  c.cdrno=e.cdrno and b.ctrseq=e.trseq and b.cdrno = c.cdrno
and a.depno<>'1E210' and a.depno<>'1D200' and a.depno<>'1C200' and a.depno<>'1A000' and a.depno<>'1B200'

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
      b.barmqy*(-1) ,      
     b.bakamts*(-1) ,  
         d.cusna ,   
      space(4) as cp_lcoin,   
        a.mancode , 
			g.username,               
  case substring(s.judco,5,1) when '1' then unmsr1 when '3' then s.unmsr2 when '5'
 then s.unmsr3 end as cp_armunmsr   ,
c.cuspono,c.tocdrno,e.dmark1
 FROM   jnerp..cdrbhad a, jnerp..cdrbdta b, jnerp..cdrhmas c, jnerp..cdrcus d, jnerp..cdrdmas e, jnerp..invmas s,jnerp..misdept f,jnerp..secuser g
   WHERE ( a.facno = b.facno ) and  a.depno=f.depno    and g.userno=a.mancode  and  
( a.bakno = b.bakno ) and       
   ( a.cusno = d.cusno ) and     
     ( b.itnbr = s.itnbr )    
AND (
convert(varchar(6),a.bakdate,112) = convert(varchar(6),dateadd(month,-1,getdate()),112)

 and a.baksta = 'Y') 
and  c.cdrno=e.cdrno and b.ctrseq=e.trseq and b.cdrno = c.cdrno
and a.depno<>'1E210' and a.depno<>'1D200' and a.depno<>'1C200' and a.depno<>'1A000' and a.depno<>'1B200'

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
      b.barmqy*(-1) ,      
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
and a.depno<>'1E210' and a.depno<>'1D200' and a.depno<>'1C200' and a.depno<>'1A000' and a.depno<>'1B200'

) x
where 
x.itnbr in ( 
select itnbr from invmas where itcls in('3015','3176','3177','3179','3180','3276','3279','3280','3376','3379','3380','3476','3480','3479','3676','3679','3680','3H76','3H79','3H80') 
)  ";

            Fill(sqlstr, ds, "CHU");

            
        }

        ////移除数据表中多余字段
        //public override void ConfigData()
        //{
        //    ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq"]);
        //    ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq2"]);
        //}
        

    }
}
