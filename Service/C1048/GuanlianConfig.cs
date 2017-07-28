using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class GuanlianConfig : NotificationConfig
    {
        public GuanlianConfig(DBServerType dbType, string connName)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSGuanlian();
            //this.reportList.Add(new BxdmxReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(this.ToString());
        }


        public override void InitData()
        {
                string sqlstr = @" select x.* from  ( select  a.*,c.cusno,e.cusna, b.armqy,b.unpris,b.armqy*b.unpris as ddcose from 
        (SELECT cdrhad.shpno ,cdrhad.shpdate ,cdrhad.depno,  misdept.depname,  cdrhad.cusno   , cdrhad.coin , cdrhad.ratio , cdrdta.trseq , cdrdta.cdrno ,  cdrdta.ctrseq ,      
        cdrdta.itnbr , cdrdta.itdsc , cdrdta.spdsc , cdrdta.itnbrcus , cdrdta.unpris , cdrdta.armqy , cdrdta.shpamts , cdrcus.cusna , space(4) as cp_lcoin,   
        cdrhad.mancode , secuser.username, case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5' then invmas.unmsr3 end as cp_armunmsr   ,
        cdrhmas.cuspono FROM cdrdta ,  cdrhad ,  cdrcus ,  invmas  ,cdrhmas ,misdept,secuser
        WHERE ( cdrhad.facno = cdrdta.facno ) and  cdrhad.depno=misdept.depno    and secuser.userno=cdrhad.mancode  and  
        ( cdrhad.shpno = cdrdta.shpno ) and  ( cdrhad.cusno = cdrcus.cusno ) and  ( cdrdta.itnbr = invmas.itnbr )  AND (cdrhad.facno = 'C' and 
        convert(varchar(6),cdrhad.shpdate,112)  < convert(varchar(6),getdate(),112) and   convert(varchar(4),cdrhad.shpdate,112)  = convert(varchar(4),getdate(),112)
        and cdrhad.houtsta = 'Y') and cdrhmas.cdrno = cdrdta.cdrno  and  (left(cdrhmas.cuspono,2)  ='CG') and cdrhad.cusno ='SGD00088' 
        ) a left join gzerp..cdrdmas b on a.cuspono= b.cdrno and a.ctrseq = b.trseq  left join gzerp..cdrhmas c on a.cuspono = c.cdrno
        ,gzerp..cdrcus e  where c.cusno=e.cusno
        union all
        select a.*,c.cusno,e.cusna, b.armqy,b.unpris,b.armqy*b.unpris as ddcose from 
        (SELECT  cdrhad.shpno , cdrhad.shpdate , cdrhad.depno,  misdept.depname,  cdrhad.cusno , cdrhad.coin ,  cdrhad.ratio , cdrdta.trseq ,   
        cdrdta.cdrno ,  cdrdta.ctrseq , cdrdta.itnbr , cdrdta.itdsc ,  cdrdta.spdsc ,  cdrdta.itnbrcus ,  cdrdta.unpris ,  cdrdta.armqy ,  cdrdta.shpamts ,  
        cdrcus.cusna , space(4) as cp_lcoin, cdrhad.mancode ,secuser.username, case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5'
        then invmas.unmsr3 end as cp_armunmsr   ,cdrhmas.cuspono   FROM cdrdta ,  cdrhad ,  cdrcus ,  invmas  ,cdrhmas ,misdept,secuser
        WHERE ( cdrhad.facno = cdrdta.facno ) and cdrhad.depno=misdept.depno    and secuser.userno=cdrhad.mancode   and        
        ( cdrhad.shpno = cdrdta.shpno ) and   ( cdrhad.cusno = cdrcus.cusno ) and  ( cdrdta.itnbr = invmas.itnbr )  AND (cdrhad.facno = 'C' 
        and  convert(varchar(4),cdrhad.shpdate ,112) = convert(varchar(4),getdate(),112) 
        and   convert(varchar(6),cdrhad.shpdate,112) < convert(varchar(6),getdate(),112) 
        and cdrhad.houtsta = 'Y') and cdrhmas.cdrno = cdrdta.cdrno  and  (left(cdrhmas.cuspono,2)  ='CN')
        and cdrhad.cusno ='SJS00254' ) a left join njerp..cdrdmas b on a.cuspono= b.cdrno and a.ctrseq = b.trseq
        left join njerp..cdrhmas c on a.cuspono = c.cdrno,njerp..cdrcus e  where c.cusno=e.cusno
        union all
        select a.*,c.cusno,e.cusna, b.armqy,b.unpris,b.armqy*b.unpris as ddcose
        from (SELECT  cdrhad.shpno , cdrhad.shpdate ,cdrhad.depno,  misdept.depname, cdrhad.cusno ,  cdrhad.coin , cdrhad.ratio , cdrdta.trseq ,   
        cdrdta.cdrno ,  cdrdta.ctrseq ,  cdrdta.itnbr , cdrdta.itdsc ,  cdrdta.spdsc ,  cdrdta.itnbrcus ,  cdrdta.unpris ,  cdrdta.armqy ,      
        cdrdta.shpamts , cdrcus.cusna ,   space(4) as cp_lcoin,   cdrhad.mancode , secuser.username,               
        case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5' then invmas.unmsr3 end as cp_armunmsr   ,
        cdrhmas.cuspono  FROM cdrdta ,  cdrhad ,  cdrcus ,  invmas  ,cdrhmas ,misdept,secuser
        WHERE ( cdrhad.facno = cdrdta.facno ) and  cdrhad.depno=misdept.depno    and secuser.userno=cdrhad.mancode  and    
        ( cdrhad.shpno = cdrdta.shpno ) and   ( cdrhad.cusno = cdrcus.cusno ) and  ( cdrdta.itnbr = invmas.itnbr )    
        AND (cdrhad.facno = 'C' and convert(varchar(6),cdrhad.shpdate,112) < convert(varchar(6),getdate(),112)
        and  convert(varchar(4),cdrhad.shpdate,112) = convert(varchar(4),getdate(),112)  and cdrhad.houtsta = 'Y') 
        and cdrhmas.cdrno = cdrdta.cdrno  and  (left(cdrhmas.cuspono,2)  ='CJ') and cdrhad.cusno ='SSD00107' 
        ) a left join jnerp..cdrdmas b on a.cuspono= b.cdrno and a.ctrseq = b.trseq  left join jnerp..cdrhmas c on a.cuspono = c.cdrno
        ,jnerp..cdrcus e  where c.cusno=e.cusno
        union all
        select a.*,'','',0,0,0  from( SELECT   cdrhad.shpno , cdrhad.shpdate , cdrhad.depno,  misdept.depname,    cdrhad.cusno , cdrhad.coin ,  cdrhad.ratio ,   
        cdrdta.trseq ,   cdrdta.cdrno ,  cdrdta.ctrseq ,  cdrdta.itnbr ,  cdrdta.itdsc ,  cdrdta.spdsc ,   cdrdta.itnbrcus ,  cdrdta.unpris ,  cdrdta.armqy ,      
        cdrdta.shpamts , cdrcus.cusna ,   space(4) as cp_lcoin,   cdrhad.mancode , secuser.username,               
        case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5'  then invmas.unmsr3 end as cp_armunmsr   ,
        cdrhmas.cuspono  FROM cdrdta ,  cdrhad ,  cdrcus ,  invmas  ,cdrhmas ,misdept,secuser
        WHERE ( cdrhad.facno = cdrdta.facno ) and  cdrhad.depno=misdept.depno    and secuser.userno=cdrhad.mancode  and          
        ( cdrhad.shpno = cdrdta.shpno ) and    ( cdrhad.cusno = cdrcus.cusno ) and   ( cdrdta.itnbr = invmas.itnbr )    
        AND (cdrhad.facno = 'C' and  convert(varchar(6),cdrhad.shpdate,112) < convert(varchar(6),getdate(),112)
        and  convert(varchar(4),cdrhad.shpdate,112) = convert(varchar(4),getdate(),112)  and cdrhad.houtsta = 'Y') 
        and cdrhmas.cdrno = cdrdta.cdrno  and  (left(cdrhmas.cuspono,2) not in ( 'CJ','CN','CG') or cdrhmas.cuspono is null )) a
        union all
        select a.*,'','',0,0,0  from( SELECT  cdrhad.shpno ,  cdrhad.shpdate ,cdrhad.depno, misdept.depname,  cdrhad.cusno ,  cdrhad.coin ,     
        cdrhad.ratio ,   cdrdta.trseq ,   cdrdta.cdrno ,  cdrdta.ctrseq ,  cdrdta.itnbr , cdrdta.itdsc ,  cdrdta.spdsc ,  cdrdta.itnbrcus ,     
        cdrdta.unpris ,  cdrdta.armqy , cdrdta.shpamts , cdrcus.cusna ,  space(4) as cp_lcoin,  cdrhad.mancode , secuser.username,               
        case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5' then invmas.unmsr3 end as cp_armunmsr   , '' as cuspono
        FROM cdrdta ,  cdrhad ,  cdrcus ,  invmas  ,misdept,secuser
        WHERE ( cdrhad.facno = cdrdta.facno ) and  cdrhad.depno=misdept.depno    and secuser.userno=cdrhad.mancode  and    
        ( cdrhad.shpno = cdrdta.shpno ) and  ( cdrhad.cusno = cdrcus.cusno ) and  ( cdrdta.itnbr = invmas.itnbr )    
        AND (cdrhad.facno = 'C' and convert(varchar(6),cdrhad.shpdate,112) < convert(varchar(6),getdate(),112)
        and  convert(varchar(4),cdrhad.shpdate,112) = convert(varchar(4),getdate(),112)  and cdrhad.houtsta = 'Y')  and  cdrdta.cdrno not like 'CC%') a ) x
        where x.depno not in ('1G110') and x.depno in ('1B100','1C000','1C100','1D000','1D100','1E000','1E100','1F000','1F100')
        and x.itnbr in ( select itnbr from invmas where itcls in('3015','3176','3177','3179','3180','3276','3279','3280','3376','3379','3380',
        '3476','3480','3479','3676','3679','3680','3H76','3H79','3H80') )  ";

                Fill(sqlstr, ds, "result");

            
        }

        ////移除数据表中多余字段
        //public override void ConfigData()
        //{
        //    ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq"]);
        //    ds.Tables["Fddxgb"].Columns.Remove(ds.Tables["Fddxgb"].Columns["trseq2"]);
        //}
        

    }
}

