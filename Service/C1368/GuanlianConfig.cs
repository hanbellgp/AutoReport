using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class GuanlianConfig : NotificationConfig
    {
        public GuanlianConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSGuanlian();
            this.reportList.Add(new GuanlianReport());
            //this.reportList.Add(new MisWorkReport2());
            this.args = Base.GetParameter(notification, this.ToString());
        }


        public override void InitData()
        {
//                string sqlstr = @" select x.* from  ( select  a.*,c.cusno,e.cusna, b.armqy,b.unpris,b.armqy*b.unpris as ddcose from 
//        (SELECT cdrhad.shpno ,cdrhad.shpdate ,cdrhad.depno,  misdept.depname,  cdrhad.cusno   , cdrhad.coin , cdrhad.ratio , cdrdta.trseq , cdrdta.cdrno ,  cdrdta.ctrseq ,      
//        cdrdta.itnbr , cdrdta.itdsc , cdrdta.spdsc , cdrdta.itnbrcus , cdrdta.unpris , cdrdta.armqy , cdrdta.shpamts , cdrcus.cusna , space(4) as cp_lcoin,   
//        cdrhad.mancode , secuser.username, case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5' then invmas.unmsr3 end as cp_armunmsr   ,
//        cdrhmas.cuspono FROM cdrdta ,  cdrhad ,  cdrcus ,  invmas  ,cdrhmas ,misdept,secuser
//        WHERE ( cdrhad.facno = cdrdta.facno ) and  cdrhad.depno=misdept.depno    and secuser.userno=cdrhad.mancode  and  
//        ( cdrhad.shpno = cdrdta.shpno ) and  ( cdrhad.cusno = cdrcus.cusno ) and  ( cdrdta.itnbr = invmas.itnbr )  AND (cdrhad.facno = 'C' and 
//        convert(varchar(6),cdrhad.shpdate,112)  < convert(varchar(6),getdate(),112) and   convert(varchar(4),cdrhad.shpdate,112)  = convert(varchar(4),getdate(),112)
//        and cdrhad.houtsta = 'Y') and cdrhmas.cdrno = cdrdta.cdrno  and  (left(cdrhmas.cuspono,2)  ='CG') and cdrhad.cusno ='SGD00088' 
//        ) a left join gzerp..cdrdmas b on a.cuspono= b.cdrno and a.ctrseq = b.trseq  left join gzerp..cdrhmas c on a.cuspono = c.cdrno
//        ,gzerp..cdrcus e  where c.cusno=e.cusno
//        union all
//        select a.*,c.cusno,e.cusna, b.armqy,b.unpris,b.armqy*b.unpris as ddcose from 
//        (SELECT  cdrhad.shpno , cdrhad.shpdate , cdrhad.depno,  misdept.depname,  cdrhad.cusno , cdrhad.coin ,  cdrhad.ratio , cdrdta.trseq ,   
//        cdrdta.cdrno ,  cdrdta.ctrseq , cdrdta.itnbr , cdrdta.itdsc ,  cdrdta.spdsc ,  cdrdta.itnbrcus ,  cdrdta.unpris ,  cdrdta.armqy ,  cdrdta.shpamts ,  
//        cdrcus.cusna , space(4) as cp_lcoin, cdrhad.mancode ,secuser.username, case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5'
//        then invmas.unmsr3 end as cp_armunmsr   ,cdrhmas.cuspono   FROM cdrdta ,  cdrhad ,  cdrcus ,  invmas  ,cdrhmas ,misdept,secuser
//        WHERE ( cdrhad.facno = cdrdta.facno ) and cdrhad.depno=misdept.depno    and secuser.userno=cdrhad.mancode   and        
//        ( cdrhad.shpno = cdrdta.shpno ) and   ( cdrhad.cusno = cdrcus.cusno ) and  ( cdrdta.itnbr = invmas.itnbr )  AND (cdrhad.facno = 'C' 
//        and  convert(varchar(4),cdrhad.shpdate ,112) = convert(varchar(4),getdate(),112) 
//        and   convert(varchar(6),cdrhad.shpdate,112) < convert(varchar(6),getdate(),112) 
//        and cdrhad.houtsta = 'Y') and cdrhmas.cdrno = cdrdta.cdrno  and  (left(cdrhmas.cuspono,2)  ='CN')
//        and cdrhad.cusno ='SJS00254' ) a left join njerp..cdrdmas b on a.cuspono= b.cdrno and a.ctrseq = b.trseq
//        left join njerp..cdrhmas c on a.cuspono = c.cdrno,njerp..cdrcus e  where c.cusno=e.cusno
//        union all
//        select a.*,c.cusno,e.cusna, b.armqy,b.unpris,b.armqy*b.unpris as ddcose
//        from (SELECT  cdrhad.shpno , cdrhad.shpdate ,cdrhad.depno,  misdept.depname, cdrhad.cusno ,  cdrhad.coin , cdrhad.ratio , cdrdta.trseq ,   
//        cdrdta.cdrno ,  cdrdta.ctrseq ,  cdrdta.itnbr , cdrdta.itdsc ,  cdrdta.spdsc ,  cdrdta.itnbrcus ,  cdrdta.unpris ,  cdrdta.armqy ,      
//        cdrdta.shpamts , cdrcus.cusna ,   space(4) as cp_lcoin,   cdrhad.mancode , secuser.username,               
//        case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5' then invmas.unmsr3 end as cp_armunmsr   ,
//        cdrhmas.cuspono  FROM cdrdta ,  cdrhad ,  cdrcus ,  invmas  ,cdrhmas ,misdept,secuser
//        WHERE ( cdrhad.facno = cdrdta.facno ) and  cdrhad.depno=misdept.depno    and secuser.userno=cdrhad.mancode  and    
//        ( cdrhad.shpno = cdrdta.shpno ) and   ( cdrhad.cusno = cdrcus.cusno ) and  ( cdrdta.itnbr = invmas.itnbr )    
//        AND (cdrhad.facno = 'C' and convert(varchar(6),cdrhad.shpdate,112) < convert(varchar(6),getdate(),112)
//        and  convert(varchar(4),cdrhad.shpdate,112) = convert(varchar(4),getdate(),112)  and cdrhad.houtsta = 'Y') 
//        and cdrhmas.cdrno = cdrdta.cdrno  and  (left(cdrhmas.cuspono,2)  ='CJ') and cdrhad.cusno ='SSD00107' 
//        ) a left join jnerp..cdrdmas b on a.cuspono= b.cdrno and a.ctrseq = b.trseq  left join jnerp..cdrhmas c on a.cuspono = c.cdrno
//        ,jnerp..cdrcus e  where c.cusno=e.cusno
//        union all
//        select a.*,'','',0,0,0  from( SELECT   cdrhad.shpno , cdrhad.shpdate , cdrhad.depno,  misdept.depname,    cdrhad.cusno , cdrhad.coin ,  cdrhad.ratio ,   
//        cdrdta.trseq ,   cdrdta.cdrno ,  cdrdta.ctrseq ,  cdrdta.itnbr ,  cdrdta.itdsc ,  cdrdta.spdsc ,   cdrdta.itnbrcus ,  cdrdta.unpris ,  cdrdta.armqy ,      
//        cdrdta.shpamts , cdrcus.cusna ,   space(4) as cp_lcoin,   cdrhad.mancode , secuser.username,               
//        case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5'  then invmas.unmsr3 end as cp_armunmsr   ,
//        cdrhmas.cuspono  FROM cdrdta ,  cdrhad ,  cdrcus ,  invmas  ,cdrhmas ,misdept,secuser
//        WHERE ( cdrhad.facno = cdrdta.facno ) and  cdrhad.depno=misdept.depno    and secuser.userno=cdrhad.mancode  and          
//        ( cdrhad.shpno = cdrdta.shpno ) and    ( cdrhad.cusno = cdrcus.cusno ) and   ( cdrdta.itnbr = invmas.itnbr )    
//        AND (cdrhad.facno = 'C' and  convert(varchar(6),cdrhad.shpdate,112) < convert(varchar(6),getdate(),112)
//        and  convert(varchar(4),cdrhad.shpdate,112) = convert(varchar(4),getdate(),112)  and cdrhad.houtsta = 'Y') 
//        and cdrhmas.cdrno = cdrdta.cdrno  and  (left(cdrhmas.cuspono,2) not in ( 'CJ','CN','CG') or cdrhmas.cuspono is null )) a
//        union all
//        select a.*,'','',0,0,0  from( SELECT  cdrhad.shpno ,  cdrhad.shpdate ,cdrhad.depno, misdept.depname,  cdrhad.cusno ,  cdrhad.coin ,     
//        cdrhad.ratio ,   cdrdta.trseq ,   cdrdta.cdrno ,  cdrdta.ctrseq ,  cdrdta.itnbr , cdrdta.itdsc ,  cdrdta.spdsc ,  cdrdta.itnbrcus ,     
//        cdrdta.unpris ,  cdrdta.armqy , cdrdta.shpamts , cdrcus.cusna ,  space(4) as cp_lcoin,  cdrhad.mancode , secuser.username,               
//        case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5' then invmas.unmsr3 end as cp_armunmsr   , '' as cuspono
//        FROM cdrdta ,  cdrhad ,  cdrcus ,  invmas  ,misdept,secuser
//        WHERE ( cdrhad.facno = cdrdta.facno ) and  cdrhad.depno=misdept.depno    and secuser.userno=cdrhad.mancode  and    
//        ( cdrhad.shpno = cdrdta.shpno ) and  ( cdrhad.cusno = cdrcus.cusno ) and  ( cdrdta.itnbr = invmas.itnbr )    
//        AND (cdrhad.facno = 'C' and convert(varchar(6),cdrhad.shpdate,112) < convert(varchar(6),getdate(),112)
//        and  convert(varchar(4),cdrhad.shpdate,112) = convert(varchar(4),getdate(),112)  and cdrhad.houtsta = 'Y')  and  cdrdta.cdrno not like 'CC%') a ) x
//        where x.depno not in ('1G110') and x.depno in ('1B100','1C000','1C100','1D000','1D100','1E000','1E100','1F000','1F100')
//        and x.itnbr in ( select itnbr from invmas where itcls in('3015','3176','3177','3179','3180','3276','3279','3280','3376','3379','3380',
//        '3476','3480','3479','3676','3679','3680','3H76','3H79','3H80') )  ";

//            string sqlstr = @"select  x.* from  (
//select  a.*,c.cusno,e.cusna, b.armqy,b.unpris,b.armqy*b.unpris as ddcose ,b.dmark1,
//(case when a.n_code_DC in ('L') then '冷冻'
//      when a.n_code_DC in ('R') then '空调'
//      when a.n_code_DC in ('H') then '热泵'
//      end )  as protype,s.address1 AS address1,v.address1 AS address2,(e.address1+e.address2+e.address3+e.address4+e.address5) AS address3
//from
//        (SELECT cdrhad.shpno ,cdrhad.shpdate ,cdrhad.depno,  misdept.depname,  cdrhad.cusno   , cdrhad.coin , cdrhad.ratio , cdrdta.trseq , cdrdta.cdrno ,  cdrdta.ctrseq ,
//        cdrdta.itnbr , cdrdta.itdsc , cdrdta.spdsc , cdrdta.itnbrcus , cdrdta.unpris , cdrdta.armqy , cdrdta.shpamts , cdrcus.cusna , space(4) as cp_lcoin,
//        cdrhad.mancode , secuser.username, case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5' then invmas.unmsr3 end as cp_armunmsr   ,
//        cdrhmas.cuspono,cdrdta.n_code_DC
//        FROM  cdrhad
//        LEFT JOIN cdrdta
//        LEFT JOIN invmas ON cdrdta.itnbr = invmas.itnbr ON cdrhad.facno = cdrdta.facno AND cdrhad.shpno = cdrdta.shpno
//        LEFT JOIN cdrhmas ON cdrhad.facno = cdrhmas.facno AND cdrhmas.cdrno = cdrdta.cdrno
//        LEFT JOIN cdrcus ON cdrhad.cusno = cdrcus.cusno
//        LEFT JOIN misdept ON cdrhad.depno=misdept.depno
//        LEFT JOIN secuser ON secuser.userno=cdrhad.mancode
//        WHERE
//        cdrhad.facno = 'C' AND convert(varchar(6),cdrhad.shpdate,112)  <= convert(varchar(6),dateadd(month,-1,getdate()),112)
//		  AND convert(varchar(4),cdrhad.shpdate,112)  = convert(varchar(4),dateadd(month,-1,getdate()),112)
//		  and cdrhad.houtsta = 'Y' and left(cdrhmas.cuspono,2)  ='CG' and cdrhad.cusno ='SGD00088'
//        ) a
//        LEFT JOIN gzerp..cdrdmas b on a.cuspono= b.cdrno and a.ctrseq = b.trseq
//        LEFT JOIN gzerp..cdrhmas c on a.cuspono = c.cdrno
//        LEFT JOIN gzerp..cdrscus s ON c.cusno = s.cusno AND c.shptrseq = s.trseq
//        LEFT JOIN gzerp..cdrivo v ON c.cusno = v.cusno AND c.ivotrseq = v.trseq
//        ,gzerp..cdrcus e  where c.cusno=e.cusno
//
//	 UNION ALL
//
//select  a.*,c.cusno,e.cusna, b.armqy,b.unpris,b.armqy*b.unpris as ddcose ,b.dmark1,
//(case when a.n_code_DC in ('L') then '冷冻'
//      when a.n_code_DC in ('R') then '空调'
//      when a.n_code_DC in ('H') then '热泵'
//      end )  as protype,s.address1 AS address1,v.address1 AS address2,(e.address1+e.address2+e.address3+e.address4+e.address5) AS address3
// FROM
//        (SELECT cdrhad.shpno ,cdrhad.shpdate ,cdrhad.depno,  misdept.depname,  cdrhad.cusno   , cdrhad.coin , cdrhad.ratio , cdrdta.trseq , cdrdta.cdrno ,  cdrdta.ctrseq ,
//        cdrdta.itnbr , cdrdta.itdsc , cdrdta.spdsc , cdrdta.itnbrcus , cdrdta.unpris , cdrdta.armqy , cdrdta.shpamts , cdrcus.cusna , space(4) as cp_lcoin,
//        cdrhad.mancode , secuser.username, case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5' then invmas.unmsr3 end as cp_armunmsr   ,
//        cdrhmas.cuspono,cdrdta.n_code_DC
//        FROM  cdrhad
//        LEFT JOIN cdrdta
//        LEFT JOIN invmas ON cdrdta.itnbr = invmas.itnbr ON cdrhad.facno = cdrdta.facno AND cdrhad.shpno = cdrdta.shpno
//        LEFT JOIN cdrhmas ON cdrhad.facno = cdrhmas.facno AND cdrhmas.cdrno = cdrdta.cdrno
//        LEFT JOIN cdrcus ON cdrhad.cusno = cdrcus.cusno
//        LEFT JOIN misdept ON cdrhad.depno=misdept.depno
//        LEFT JOIN secuser ON secuser.userno=cdrhad.mancode
//        WHERE
//        cdrhad.facno = 'C' AND convert(varchar(6),cdrhad.shpdate,112)  <= convert(varchar(6),dateadd(month,-1,getdate()),112)
//		  AND convert(varchar(4),cdrhad.shpdate,112)  = convert(varchar(4),dateadd(month,-1,getdate()),112)
//		  and cdrhad.houtsta = 'Y' and  (left(cdrhmas.cuspono,3)  ='CC4') and cdrhad.cusno ='SCQ00146'
//        ) a
//        LEFT JOIN cqerp..cdrdmas b on a.cuspono= b.cdrno and a.ctrseq = b.trseq
//        LEFT JOIN cqerp..cdrhmas c on a.cuspono = c.cdrno
//        LEFT JOIN cqerp..cdrscus s ON c.cusno = s.cusno AND c.shptrseq = s.trseq
//        LEFT JOIN cqerp..cdrivo v ON c.cusno = v.cusno AND c.ivotrseq = v.trseq
//        ,cqerp..cdrcus e  where c.cusno=e.cusno
//        UNION ALL
//
//        select a.*,c.cusno,e.cusna, b.armqy,b.unpris,b.armqy*b.unpris as ddcose  ,b.dmark1,
//(case when a.n_code_DC in ('L') then '冷冻'
//      when a.n_code_DC in ('R') then '空调'
//      when a.n_code_DC in ('H') then '热泵'
//      end )  as protype,s.address1 AS address1,v.address1 AS address2,(e.address1+e.address2+e.address3+e.address4+e.address5) AS address3
// from
//        (SELECT  cdrhad.shpno , cdrhad.shpdate , cdrhad.depno,  misdept.depname,  cdrhad.cusno , cdrhad.coin ,  cdrhad.ratio , cdrdta.trseq ,
//        cdrdta.cdrno ,  cdrdta.ctrseq , cdrdta.itnbr , cdrdta.itdsc ,  cdrdta.spdsc ,  cdrdta.itnbrcus ,  cdrdta.unpris ,  cdrdta.armqy ,  cdrdta.shpamts ,
//        cdrcus.cusna , space(4) as cp_lcoin, cdrhad.mancode ,secuser.username, case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5'
//        then invmas.unmsr3 end as cp_armunmsr   ,cdrhmas.cuspono,cdrdta.n_code_DC
//        FROM  cdrhad
//        LEFT JOIN cdrdta
//        LEFT JOIN invmas ON cdrdta.itnbr = invmas.itnbr ON cdrhad.facno = cdrdta.facno AND cdrhad.shpno = cdrdta.shpno
//        LEFT JOIN cdrhmas ON cdrhad.facno = cdrhmas.facno AND cdrhmas.cdrno = cdrdta.cdrno
//        LEFT JOIN cdrcus ON cdrhad.cusno = cdrcus.cusno
//        LEFT JOIN misdept ON cdrhad.depno=misdept.depno
//        LEFT JOIN secuser ON secuser.userno=cdrhad.mancode
//        WHERE
//        cdrhad.facno = 'C' AND convert(varchar(6),cdrhad.shpdate,112)  <= convert(varchar(6),dateadd(month,-1,getdate()),112)
//		  AND convert(varchar(4),cdrhad.shpdate,112)  = convert(varchar(4),dateadd(month,-1,getdate()),112)
//		  and cdrhad.houtsta = 'Y' and  (left(cdrhmas.cuspono,2)  ='CN') and cdrhad.cusno ='SJS00254'
//        ) a
//        LEFT JOIN njerp..cdrdmas b on a.cuspono= b.cdrno and a.ctrseq = b.trseq
//        LEFT JOIN njerp..cdrhmas c on a.cuspono = c.cdrno
//        LEFT JOIN njerp..cdrscus s ON c.cusno = s.cusno AND c.shptrseq = s.trseq
//        LEFT JOIN njerp..cdrivo v ON c.cusno = v.cusno AND c.ivotrseq = v.trseq
//        ,njerp..cdrcus e where c.cusno=e.cusno
//        UNION ALL
//
//        select a.*,c.cusno,e.cusna, b.armqy,b.unpris,b.armqy*b.unpris as ddcose ,b.dmark1,
//(case when a.n_code_DC in ('L') then '冷冻'
//      when a.n_code_DC in ('R') then '空调'
//      when a.n_code_DC in ('H') then '热泵'
//      end )  as protype,s.address1 AS address1,v.address1 AS address2,(e.address1+e.address2+e.address3+e.address4+e.address5) AS address3
//        from (SELECT  cdrhad.shpno , cdrhad.shpdate ,cdrhad.depno,  misdept.depname, cdrhad.cusno ,  cdrhad.coin , cdrhad.ratio , cdrdta.trseq ,
//        cdrdta.cdrno ,  cdrdta.ctrseq ,  cdrdta.itnbr , cdrdta.itdsc ,  cdrdta.spdsc ,  cdrdta.itnbrcus ,  cdrdta.unpris ,  cdrdta.armqy ,
//        cdrdta.shpamts , cdrcus.cusna ,   space(4) as cp_lcoin,   cdrhad.mancode , secuser.username,
//        case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5' then invmas.unmsr3 end as cp_armunmsr   ,
//        cdrhmas.cuspono,cdrdta.n_code_DC
//        FROM  cdrhad
//        LEFT JOIN cdrdta
//        LEFT JOIN invmas ON cdrdta.itnbr = invmas.itnbr ON cdrhad.facno = cdrdta.facno AND cdrhad.shpno = cdrdta.shpno
//        LEFT JOIN cdrhmas ON cdrhad.facno = cdrhmas.facno AND cdrhmas.cdrno = cdrdta.cdrno
//        LEFT JOIN cdrcus ON cdrhad.cusno = cdrcus.cusno
//        LEFT JOIN misdept ON cdrhad.depno=misdept.depno
//        LEFT JOIN secuser ON secuser.userno=cdrhad.mancode
//        WHERE
//        cdrhad.facno = 'C' AND convert(varchar(6),cdrhad.shpdate,112)  <= convert(varchar(6),dateadd(month,-1,getdate()),112)
//		  AND convert(varchar(4),cdrhad.shpdate,112)  = convert(varchar(4),dateadd(month,-1,getdate()),112)
//		  and cdrhad.houtsta = 'Y' and  (left(cdrhmas.cuspono,2)  ='CJ') and cdrhad.cusno ='SSD00107'
//        ) a
//        left join jnerp..cdrdmas b on a.cuspono= b.cdrno and a.ctrseq = b.trseq
//        left join jnerp..cdrhmas c on a.cuspono = c.cdrno
//        LEFT JOIN jnerp..cdrscus s ON c.cusno = s.cusno AND c.shptrseq = s.trseq
//        LEFT JOIN jnerp..cdrivo v ON c.cusno = v.cusno AND c.ivotrseq = v.trseq
//        ,jnerp..cdrcus e  where c.cusno=e.cusno
//
//        UNION ALL
//		SELECT   cdrhad.shpno , cdrhad.shpdate , cdrhad.depno,  misdept.depname,    cdrhad.cusno , cdrhad.coin ,  cdrhad.ratio ,
//        cdrdta.trseq ,   cdrdta.cdrno ,  cdrdta.ctrseq ,  cdrdta.itnbr ,  cdrdta.itdsc ,  cdrdta.spdsc ,   cdrdta.itnbrcus ,  cdrdta.unpris ,  cdrdta.armqy ,
//        cdrdta.shpamts , cdrcus.cusna ,   space(4) as cp_lcoin,   cdrhad.mancode , secuser.username,
//        case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5'  then invmas.unmsr3 end as cp_armunmsr   ,
//        '' as cuspono,cdrdta.n_code_DC,'' ,'',0,0 ,0,cdrdmas.dmark1,
//		(case when cdrdta.n_code_DC in ('L') then '冷冻' when cdrdta.n_code_DC in ('R') then '空调' when cdrdta.n_code_DC in ('H') then '热泵' end )  as protype,
//		s.address1 AS address1,v.address1 AS address2,(cdrcus.address1+cdrcus.address2+cdrcus.address3+cdrcus.address4+cdrcus.address5) AS address3
//        FROM  cdrhad
//        LEFT JOIN cdrdta
//        LEFT JOIN invmas ON cdrdta.itnbr = invmas.itnbr
//        LEFT JOIN cdrdmas ON cdrdmas.facno = cdrdta.facno AND cdrdta.cdrno = cdrdmas.cdrno AND cdrdta.ctrseq = cdrdmas.trseq
//        ON cdrdta.facno= cdrdta.facno  AND cdrhad.shpno = cdrdta.shpno
//        LEFT JOIN cdrcus ON cdrhad.cusno = cdrcus.cusno
//        LEFT JOIN misdept ON cdrhad.depno=misdept.depno
//        LEFT JOIN secuser ON secuser.userno=cdrhad.mancode
//        LEFT JOIN cdrscus s ON cdrhad.cusno = s.cusno AND cdrhad.shptrseq = s.trseq
//        LEFT JOIN cdrivo v ON cdrhad.cusno = v.cusno AND cdrhad.ivotrseq = v.trseq
//        WHERE
//        cdrhad.facno = 'C' AND convert(varchar(6),cdrhad.shpdate,112)  <= convert(varchar(6),dateadd(month,-1,getdate()),112)
//		AND convert(varchar(4),cdrhad.shpdate,112)  = convert(varchar(4),dateadd(month,-1,getdate()),112)
//		and cdrhad.houtsta = 'Y'
//		AND cdrhad.cusno not in ('SSD00107','SGD00088','SJS00254','SCQ00146','KZJ00029')
//		AND cdrdta.itnbrcus<> ''
//        UNION ALL
//
//		SELECT  cdrhad.shpno ,  cdrhad.shpdate ,cdrhad.depno, misdept.depname,  cdrhad.cusno ,  cdrhad.coin ,
//        cdrhad.ratio ,   cdrdta.trseq ,   cdrdta.cdrno ,  cdrdta.ctrseq ,  cdrdta.itnbr , cdrdta.itdsc ,  cdrdta.spdsc ,  cdrdta.itnbrcus ,
//        cdrdta.unpris ,  cdrdta.armqy , cdrdta.shpamts , cdrcus.cusna ,  space(4) as cp_lcoin,  cdrhad.mancode , secuser.username,
//        case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5' then invmas.unmsr3 end as cp_armunmsr   ,
//         '' as cuspono,cdrdta.n_code_DC,'','',0,0,0 ,cdrdmas.dmark1,
//		(case when cdrdta.n_code_DC in ('L') then '冷冻' when cdrdta.n_code_DC in ('R') then '空调' when cdrdta.n_code_DC in ('H') then '热泵' end )  as protype,
//		s.address1 AS address1,v.address1 AS address2,(cdrcus.address1+cdrcus.address2+cdrcus.address3+cdrcus.address4+cdrcus.address5) AS address3
//        FROM  cdrhad
//        LEFT JOIN cdrdta
//        LEFT JOIN invmas ON cdrdta.itnbr = invmas.itnbr
//        LEFT JOIN cdrdmas ON cdrdmas.facno = cdrdta.facno AND cdrdta.cdrno = cdrdmas.cdrno AND cdrdta.ctrseq = cdrdmas.trseq
//        ON cdrdta.facno= cdrdta.facno  AND cdrhad.shpno = cdrdta.shpno
//        LEFT JOIN cdrcus ON cdrhad.cusno = cdrcus.cusno
//        LEFT JOIN misdept ON cdrhad.depno=misdept.depno
//        LEFT JOIN secuser ON secuser.userno=cdrhad.mancode
//        LEFT JOIN cdrscus s ON cdrhad.cusno = s.cusno AND cdrhad.shptrseq = s.trseq
//        LEFT JOIN cdrivo v ON cdrhad.cusno = v.cusno AND cdrhad.ivotrseq = v.trseq
//        WHERE
//        cdrhad.facno = 'C' and convert(varchar(6),cdrhad.shpdate,112) <= convert(varchar(6),dateadd(month,-1,getdate()),112)
//        and  convert(varchar(4),cdrhad.shpdate,112) = convert(varchar(4),dateadd(month,-1,getdate()),112)
//		 and cdrhad.houtsta = 'Y'  and cdrdta.cdrno not like 'CC%'
//		 ) x
//        where x.depno not in ('1G110') and x.depno in ( '1D000','1B100','1E100','1D100','1C100','1C000','1C700','1E000','1B000','1F100','1F000','1F500' ,'1V000','1V100','1T100')
//        and (x.itnbr in ( select itnbr from invmas where itcls in('3176','3177','3179','3180','3276','3279','3280','3083') )
//         or x.itnbr in (select itnbr from invmas where itcls in ('3176','3177','3179','3180','3676','3679','3680','3015')  AND itnbr<>'3142-GBR9018'))
//";


            string sqlstr = @"select  x.* from  (
 select  a.*,c.cusno,e.cusna, b.armqy,b.unpris,b.armqy*b.unpris as ddcose ,b.dmark1,
 (case when a.n_code_DC in ('L') then '冷冻'
      when a.n_code_DC in ('R') then '空调'
      when a.n_code_DC in ('H') then '热泵'
      end )  as protype,s.address1 AS address1,v.address1 AS address2,(e.address1+e.address2+e.address3+e.address4+e.address5) AS address3
from
        (SELECT cdrhad.shpno ,cdrhad.shpdate ,cdrhad.depno,  misdept.depname,  cdrhad.cusno   , cdrhad.coin , cdrhad.ratio , cdrdta.trseq , cdrdta.cdrno ,  cdrdta.ctrseq ,
        cdrdta.itnbr , cdrdta.itdsc , cdrdta.spdsc , cdrdta.itnbrcus , cdrdta.unpris , cdrdta.armqy , cdrdta.shpamts , cdrcus.cusna , space(4) as cp_lcoin,
        cdrhad.mancode , secuser.username, case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5' then invmas.unmsr3 end as cp_armunmsr   ,
        cdrhmas.cuspono,cdrdta.n_code_DC,cdrhad.shptrseq,cdrhad.ivotrseq
        FROM  cdrhad
        LEFT JOIN cdrdta
        LEFT JOIN invmas ON cdrdta.itnbr = invmas.itnbr ON cdrhad.facno = cdrdta.facno AND cdrhad.shpno = cdrdta.shpno
        LEFT JOIN cdrhmas ON cdrhad.facno = cdrhmas.facno AND cdrhmas.cdrno = cdrdta.cdrno
        LEFT JOIN cdrcus ON cdrhad.cusno = cdrcus.cusno
        LEFT JOIN misdept ON cdrhad.depno=misdept.depno
        LEFT JOIN secuser ON secuser.userno=cdrhad.mancode
        WHERE
        cdrhad.facno = 'C' AND convert(varchar(6),cdrhad.shpdate,112)  <= convert(varchar(6),dateadd(month,-1,getdate()),112)
		  AND convert(varchar(4),cdrhad.shpdate,112)  = convert(varchar(4),dateadd(month,-1,getdate()),112)
		  and cdrhad.houtsta = 'Y' and left(cdrhmas.cuspono,2)  ='CG' and cdrhad.cusno ='SGD00088'
        ) a
        LEFT JOIN gzerp..cdrdmas b on a.cuspono= b.cdrno and a.ctrseq = b.trseq
        LEFT JOIN gzerp..cdrhmas c on a.cuspono = c.cdrno
        LEFT JOIN gzerp..cdrscus s ON c.cusno = s.cusno AND c.shptrseq = s.trseq
        LEFT JOIN gzerp..cdrivo v ON c.cusno = v.cusno AND c.ivotrseq = v.trseq
        ,gzerp..cdrcus e  where c.cusno=e.cusno
	 UNION ALL
select  a.*,c.cusno,e.cusna, b.armqy,b.unpris,b.armqy*b.unpris as ddcose ,b.dmark1,
(case when a.n_code_DC in ('L') then '冷冻'
      when a.n_code_DC in ('R') then '空调'
      when a.n_code_DC in ('H') then '热泵'
      end )  as protype,s.address1 AS address1,v.address1 AS address2,(e.address1+e.address2+e.address3+e.address4+e.address5) AS address3
 FROM
        (SELECT cdrhad.shpno ,cdrhad.shpdate ,cdrhad.depno,  misdept.depname,  cdrhad.cusno   , cdrhad.coin , cdrhad.ratio , cdrdta.trseq , cdrdta.cdrno ,  cdrdta.ctrseq ,
        cdrdta.itnbr , cdrdta.itdsc , cdrdta.spdsc , cdrdta.itnbrcus , cdrdta.unpris , cdrdta.armqy , cdrdta.shpamts , cdrcus.cusna , space(4) as cp_lcoin,
        cdrhad.mancode , secuser.username, case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5' then invmas.unmsr3 end as cp_armunmsr   ,
        cdrhmas.cuspono,cdrdta.n_code_DC,cdrhad.shptrseq,cdrhad.ivotrseq
        FROM  cdrhad
        LEFT JOIN cdrdta
        LEFT JOIN invmas ON cdrdta.itnbr = invmas.itnbr ON cdrhad.facno = cdrdta.facno AND cdrhad.shpno = cdrdta.shpno
        LEFT JOIN cdrhmas ON cdrhad.facno = cdrhmas.facno AND cdrhmas.cdrno = cdrdta.cdrno
        LEFT JOIN cdrcus ON cdrhad.cusno = cdrcus.cusno
        LEFT JOIN misdept ON cdrhad.depno=misdept.depno
        LEFT JOIN secuser ON secuser.userno=cdrhad.mancode
        WHERE
        cdrhad.facno = 'C' AND convert(varchar(6),cdrhad.shpdate,112)  <= convert(varchar(6),dateadd(month,-1,getdate()),112)
		  AND convert(varchar(4),cdrhad.shpdate,112)  = convert(varchar(4),dateadd(month,-1,getdate()),112)
		  and cdrhad.houtsta = 'Y' and  (left(cdrhmas.cuspono,3)  ='CC4') and cdrhad.cusno ='SCQ00146'
        ) a
        LEFT JOIN cqerp..cdrdmas b on a.cuspono= b.cdrno and a.ctrseq = b.trseq
        LEFT JOIN cqerp..cdrhmas c on a.cuspono = c.cdrno
        LEFT JOIN cdrscus s ON a.cusno = s.cusno AND a.shptrseq = s.trseq
        LEFT JOIN cqerp..cdrivo v ON c.cusno = v.cusno AND c.ivotrseq = v.trseq
        ,cqerp..cdrcus e  where c.cusno=e.cusno
        UNION ALL

        select a.*,c.cusno,e.cusna, b.armqy,b.unpris,b.armqy*b.unpris as ddcose  ,b.dmark1,
(case when a.n_code_DC in ('L') then '冷冻'
      when a.n_code_DC in ('R') then '空调'
      when a.n_code_DC in ('H') then '热泵'
      end )  as protype,s.address1 AS address1,v.address1 AS address2,(e.address1+e.address2+e.address3+e.address4+e.address5) AS address3
 from
        (SELECT  cdrhad.shpno , cdrhad.shpdate , cdrhad.depno,  misdept.depname,  cdrhad.cusno , cdrhad.coin ,  cdrhad.ratio , cdrdta.trseq ,
        cdrdta.cdrno ,  cdrdta.ctrseq , cdrdta.itnbr , cdrdta.itdsc ,  cdrdta.spdsc ,  cdrdta.itnbrcus ,  cdrdta.unpris ,  cdrdta.armqy ,  cdrdta.shpamts ,
        cdrcus.cusna , space(4) as cp_lcoin, cdrhad.mancode ,secuser.username, case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5'
        then invmas.unmsr3 end as cp_armunmsr   ,cdrhmas.cuspono,cdrdta.n_code_DC,cdrhad.shptrseq,cdrhad.ivotrseq
        FROM  cdrhad
        LEFT JOIN cdrdta
        LEFT JOIN invmas ON cdrdta.itnbr = invmas.itnbr ON cdrhad.facno = cdrdta.facno AND cdrhad.shpno = cdrdta.shpno
        LEFT JOIN cdrhmas ON cdrhad.facno = cdrhmas.facno AND cdrhmas.cdrno = cdrdta.cdrno
        LEFT JOIN cdrcus ON cdrhad.cusno = cdrcus.cusno
        LEFT JOIN misdept ON cdrhad.depno=misdept.depno
        LEFT JOIN secuser ON secuser.userno=cdrhad.mancode
        WHERE
        cdrhad.facno = 'C' AND convert(varchar(6),cdrhad.shpdate,112)  <= convert(varchar(6),dateadd(month,-1,getdate()),112)
		  AND convert(varchar(4),cdrhad.shpdate,112)  = convert(varchar(4),dateadd(month,-1,getdate()),112)
		  and cdrhad.houtsta = 'Y' and  (left(cdrhmas.cuspono,2)  ='CN') and cdrhad.cusno ='SJS00254'
        ) a
        LEFT JOIN njerp..cdrdmas b on a.cuspono= b.cdrno and a.ctrseq = b.trseq
        LEFT JOIN njerp..cdrhmas c on a.cuspono = c.cdrno
        LEFT JOIN cdrscus s ON a.cusno = s.cusno AND a.shptrseq = s.trseq
        LEFT JOIN njerp..cdrivo v ON c.cusno = v.cusno AND c.ivotrseq = v.trseq
        ,njerp..cdrcus e where c.cusno=e.cusno
        UNION ALL

        select a.*,c.cusno,e.cusna, b.armqy,b.unpris,b.armqy*b.unpris as ddcose ,b.dmark1,
(case when a.n_code_DC in ('L') then '冷冻'
      when a.n_code_DC in ('R') then '空调'
      when a.n_code_DC in ('H') then '热泵'
      end )  as protype,s.address1 AS address1,v.address1 AS address2,(e.address1+e.address2+e.address3+e.address4+e.address5) AS address3
        from (SELECT  cdrhad.shpno , cdrhad.shpdate ,cdrhad.depno,  misdept.depname, cdrhad.cusno ,  cdrhad.coin , cdrhad.ratio , cdrdta.trseq ,
        cdrdta.cdrno ,  cdrdta.ctrseq ,  cdrdta.itnbr , cdrdta.itdsc ,  cdrdta.spdsc ,  cdrdta.itnbrcus ,  cdrdta.unpris ,  cdrdta.armqy ,
        cdrdta.shpamts , cdrcus.cusna ,   space(4) as cp_lcoin,   cdrhad.mancode , secuser.username,
        case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5' then invmas.unmsr3 end as cp_armunmsr   ,
        cdrhmas.cuspono,cdrdta.n_code_DC,cdrhad.shptrseq,cdrhad.ivotrseq
        FROM  cdrhad
        LEFT JOIN cdrdta
        LEFT JOIN invmas ON cdrdta.itnbr = invmas.itnbr ON cdrhad.facno = cdrdta.facno AND cdrhad.shpno = cdrdta.shpno
        LEFT JOIN cdrhmas ON cdrhad.facno = cdrhmas.facno AND cdrhmas.cdrno = cdrdta.cdrno
        LEFT JOIN cdrcus ON cdrhad.cusno = cdrcus.cusno
        LEFT JOIN misdept ON cdrhad.depno=misdept.depno
        LEFT JOIN secuser ON secuser.userno=cdrhad.mancode
        WHERE
        cdrhad.facno = 'C' AND convert(varchar(6),cdrhad.shpdate,112)  <= convert(varchar(6),dateadd(month,-1,getdate()),112)
		  AND convert(varchar(4),cdrhad.shpdate,112)  = convert(varchar(4),dateadd(month,-1,getdate()),112)
		  and cdrhad.houtsta = 'Y' and  (left(cdrhmas.cuspono,2)  ='CJ') and cdrhad.cusno ='SSD00107'
        ) a
        left join jnerp..cdrdmas b on a.cuspono= b.cdrno and a.ctrseq = b.trseq
        left join jnerp..cdrhmas c on a.cuspono = c.cdrno
        LEFT JOIN cdrscus s ON a.cusno = s.cusno AND a.shptrseq = s.trseq
        LEFT JOIN jnerp..cdrivo v ON c.cusno = v.cusno AND c.ivotrseq = v.trseq
        ,jnerp..cdrcus e  where c.cusno=e.cusno

        UNION ALL
		SELECT   cdrhad.shpno , cdrhad.shpdate , cdrhad.depno,  misdept.depname,    cdrhad.cusno , cdrhad.coin ,  cdrhad.ratio ,
        cdrdta.trseq ,   cdrdta.cdrno ,  cdrdta.ctrseq ,  cdrdta.itnbr ,  cdrdta.itdsc ,  cdrdta.spdsc ,   cdrdta.itnbrcus ,  cdrdta.unpris ,  cdrdta.armqy ,
        cdrdta.shpamts , cdrcus.cusna ,   space(4) as cp_lcoin,   cdrhad.mancode , secuser.username,
        case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5'  then invmas.unmsr3 end as cp_armunmsr   ,
        '' as cuspono,cdrdta.n_code_DC,cdrhad.shptrseq,cdrhad.ivotrseq,
      '' ,'',0,0 ,0,cdrdmas.dmark1,
		(case when cdrdta.n_code_DC in ('L') then '冷冻' when cdrdta.n_code_DC in ('R') then '空调' when cdrdta.n_code_DC in ('H') then '热泵' end )  as protype,
		s.address1 AS address1,v.address1 AS address2,(cdrcus.address1+cdrcus.address2+cdrcus.address3+cdrcus.address4+cdrcus.address5) AS address3
        FROM  cdrhad
        LEFT JOIN cdrdta
        LEFT JOIN invmas ON cdrdta.itnbr = invmas.itnbr
        LEFT JOIN cdrdmas ON cdrdmas.facno = cdrdta.facno AND cdrdta.cdrno = cdrdmas.cdrno AND cdrdta.ctrseq = cdrdmas.trseq
        ON cdrdta.facno= cdrdta.facno  AND cdrhad.shpno = cdrdta.shpno
        LEFT JOIN cdrcus ON cdrhad.cusno = cdrcus.cusno
        LEFT JOIN misdept ON cdrhad.depno=misdept.depno
        LEFT JOIN secuser ON secuser.userno=cdrhad.mancode
        LEFT JOIN cdrscus s ON cdrhad.cusno = s.cusno AND cdrhad.shptrseq = s.trseq
        LEFT JOIN cdrivo v ON cdrhad.cusno = v.cusno AND cdrhad.ivotrseq = v.trseq
        WHERE
        cdrhad.facno = 'C' AND convert(varchar(6),cdrhad.shpdate,112)  <= convert(varchar(6),dateadd(month,-1,getdate()),112)
		AND convert(varchar(4),cdrhad.shpdate,112)  = convert(varchar(4),dateadd(month,-1,getdate()),112)
		and cdrhad.houtsta = 'Y'
		AND cdrhad.cusno not in ('SSD00107','SGD00088','SJS00254','SCQ00146','KZJ00029')
		AND cdrdta.itnbrcus<> ''
        UNION ALL

		SELECT  cdrhad.shpno ,  cdrhad.shpdate ,cdrhad.depno, misdept.depname,  cdrhad.cusno ,  cdrhad.coin ,
        cdrhad.ratio ,   cdrdta.trseq ,   cdrdta.cdrno ,  cdrdta.ctrseq ,  cdrdta.itnbr , cdrdta.itdsc ,  cdrdta.spdsc ,  cdrdta.itnbrcus ,
        cdrdta.unpris ,  cdrdta.armqy , cdrdta.shpamts , cdrcus.cusna ,  space(4) as cp_lcoin,  cdrhad.mancode , secuser.username,
        case substring(invmas.judco,5,1) when '1' then unmsr1 when '3' then invmas.unmsr2 when '5' then invmas.unmsr3 end as cp_armunmsr   ,
         '' as cuspono,cdrdta.n_code_DC,cdrhad.shptrseq,cdrhad.ivotrseq,'','',0,0,0 ,cdrdmas.dmark1,
		(case when cdrdta.n_code_DC in ('L') then '冷冻' when cdrdta.n_code_DC in ('R') then '空调' when cdrdta.n_code_DC in ('H') then '热泵' end )  as protype,
		s.address1 AS address1,v.address1 AS address2,(cdrcus.address1+cdrcus.address2+cdrcus.address3+cdrcus.address4+cdrcus.address5) AS address3
        FROM  cdrhad
        LEFT JOIN cdrdta
        LEFT JOIN invmas ON cdrdta.itnbr = invmas.itnbr
        LEFT JOIN cdrdmas ON cdrdmas.facno = cdrdta.facno AND cdrdta.cdrno = cdrdmas.cdrno AND cdrdta.ctrseq = cdrdmas.trseq
        ON cdrdta.facno= cdrdta.facno  AND cdrhad.shpno = cdrdta.shpno
        LEFT JOIN cdrcus ON cdrhad.cusno = cdrcus.cusno
        LEFT JOIN misdept ON cdrhad.depno=misdept.depno
        LEFT JOIN secuser ON secuser.userno=cdrhad.mancode
        LEFT JOIN cdrscus s ON cdrhad.cusno = s.cusno AND cdrhad.shptrseq = s.trseq
        LEFT JOIN cdrivo v ON cdrhad.cusno = v.cusno AND cdrhad.ivotrseq = v.trseq
        WHERE
        cdrhad.facno = 'C' and convert(varchar(6),cdrhad.shpdate,112) <= convert(varchar(6),dateadd(month,-1,getdate()),112)
        and  convert(varchar(4),cdrhad.shpdate,112) = convert(varchar(4),dateadd(month,-1,getdate()),112)
		 and cdrhad.houtsta = 'Y'  and cdrdta.cdrno not like 'CC%'
		 ) x
        where x.depno not in ('1G110') and x.depno in ( '1D000','1B100','1E100','1D100','1C100','1C000','1C700','1E000','1B000','1F100','1F000','1F500' ,'1V000','1V100','1T100','1F700')
        and (x.itnbr in ( select itnbr from invmas where itcls in('3176','3177','3179','3180','3276','3279','3280','3083') )
         or x.itnbr in (select itnbr from invmas where itcls in ('3176','3177','3179','3180','3676','3679','3680','3015')  AND itnbr<>'3142-GBR9018'))
";

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

