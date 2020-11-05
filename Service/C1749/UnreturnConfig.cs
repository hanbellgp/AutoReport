using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class UnreturnConfig : NotificationConfig
    {
        public UnreturnConfig(DBServerType dbType, string ConnName, string notifaction)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(ConnName));
            this.ds = new UnreturnDS();
            this.reportList.Add(new UnreturnReport());
            this.args = Base.GetParameter(notifaction, this.ToString());
        }
        public override void InitData()
        {
            String sqlStr = @"SELECT  cdrlnhad.trno as trno ,cdrlnhad.objtype as objtype ,cdrlnhad.cusno as cusno ,
(SELECT cdesc FROM miscode WHERE cdrlnhad.cusno = miscode.code AND miscode.ckind = cdrlnhad.objtype) AS cdesc,cdrlnhad.depno as depno,
(SELECT cdesc FROM miscode WHERE cdrlnhad.depno = miscode.code AND miscode.ckind = 'GE') AS deptname,cdrlnhad.headperson as headperson,
(SELECT cdesc FROM miscode WHERE miscode.ckind = '9E' AND miscode.code = cdrlnhad.headperson) AS username,cdrlnhad.trdate as trdate,
cdrlndta.prebkdate as prebkdate,cdrlndta.itnbr as itnbr,invmas.itdsc as itdsc,cdrlndta.trnqy1 as trnqy1,cdrlndta.retqy1 as retqy1,(cdrlndta.trnqy1 - cdrlndta.retqy1) AS unretqy1,
( CASE SUBSTRING(invmas.judco, 2, 1)WHEN '1' THEN unmsr1 WHEN '3' THEN unmsr2 ELSE unmsr1 END ) AS unmsr,
DATEDIFF(day, cdrlndta.prebkdate, GETDATE()) AS delaydays  FROM cdrlndta ,cdrlnhad ,invmas ,invcls 
WHERE ( cdrlnhad.facno = cdrlndta.facno )AND ( cdrlnhad.trno = cdrlndta.trno )AND cdrlndta.itnbr = invmas.itnbr 
AND cdrlndta.status IN ( 'Y', 'R' ) AND invmas.itcls = invcls.itcls
AND DATEDIFF(day, GETDATE(), cdrlndta.prebkdate) <= 15 AND ( cdrlndta.trnqy1 - cdrlndta.retqy1 ) > 0 AND cdrlnhad.status <> 'W' and cdrlnhad.headperson <> ''";
            Fill(sqlStr,ds, "tlb");


            StringBuilder sb = new StringBuilder();
            //sb.Append(" select objtype,depno,cdesc,isnull(sum(cost),0) as cost,0 as actor from ( ");
            //sb.Append(" SELECT cdrlndta.facno,  cdrlnhad.objtype,  cdrlnhad.depno,  invcls.itclscode,  invmas.itcls,  invcls.clsdsc, ");
            //sb.Append(" (SELECT cdesc FROM miscode WHERE cdrlnhad.depno = miscode.code AND miscode.ckind = 'GE') AS cdesc,cdrlndta.itnbr,  invmas.itdsc, ");
            //sb.Append(" sum ((trnqy1 - isnull((select sum(brpqy1) from cdrbrdta, cdrbrhad where cdrbrdta.facno=cdrbrhad.facno and cdrbrdta.brtrno=cdrbrhad.brtrno  ");
            //sb.Append(" and cdrbrdta.itnbr=cdrlndta.itnbr and cdrbrdta.lntrno=cdrlnhad.trno ");
            //sb.Append(" and cdrbrdta.lntrseq=cdrlndta.trseq and left(convert(char(8), cdrbrhad.brdate, 112), 6)<=left(convert(char(8), dateadd(month, -1, getdate()), 112), 6) and cdrbrhad.status='Y'), 0 )) ");
            //sb.Append(" *(case substring(invmas.judco, 2, 1) when '1' then 1 when '3' then invmas.rate2 else 1 end )) as cp_unretqy1, ");
            //sb.Append(" ( select cast( (invpri.unittotcst/ invpri.unittotqy) as decimal(9, 2)) from invpri where invpri.itnbr=cdrlndta.itnbr and invpri.yearmon=left(convert(char(8), dateadd(month, -1, getdate()), 112), 6) )* ");
            //sb.Append(" ( sum ((trnqy1 - isnull((select sum(brpqy1) from cdrbrdta, cdrbrhad where cdrbrdta.facno=cdrbrhad.facno and cdrbrdta.brtrno=cdrbrhad.brtrno ");
            //sb.Append(" and cdrbrdta.itnbr=cdrlndta.itnbr and cdrbrdta.lntrno=cdrlnhad.trno ");
            //sb.Append(" and cdrbrdta.lntrseq=cdrlndta.trseq and left(convert(char(8), cdrbrhad.brdate, 112), 6)<=left(convert(char(8), dateadd(month, -1, getdate()), 112), 6) and cdrbrhad.status='Y'), 0 )) ");
            //sb.Append(" *(case substring(invmas.judco, 2, 1) when '1' then 1 when '3' then invmas.rate2 else 1 end )) ) as cost ");
            //sb.Append(" FROM cdrlndta,  cdrlnhad,  invmas,  invcls ");
            //sb.Append(" WHERE ( cdrlnhad.facno=cdrlndta.facno ) and ");
            //sb.Append(" ( cdrlnhad.trno=cdrlndta.trno ) and cdrlndta.itnbr=invmas.itnbr and cdrlndta.status in ('Y', 'R' ) and invmas.itcls=invcls.itcls ");
            //sb.Append(" and left(convert(char(8), cdrlnhad.trdate, 112), 6)<=left(convert(char(8), dateadd(month, -1, getdate()), 112), 6) and cdrlndta.facno='C' and cdrlnhad.status<>'W' ");
            //sb.Append(" GROUP BY cdrlndta.facno,  cdrlnhad.objtype,  cdrlnhad.depno,  invcls.itclscode,  invmas.itcls,  invcls.clsdsc, cdrlndta.itnbr,  invmas.itdsc ");
            //sb.Append(" having sum (trnqy1 - isnull((select sum(brpqy1) from cdrbrdta, cdrbrhad where cdrbrdta.facno=cdrbrhad.facno and cdrbrdta.brtrno=cdrbrhad.brtrno ");
            //sb.Append(" and cdrbrdta.itnbr=cdrlndta.itnbr and cdrbrdta.lntrno=cdrlnhad.trno ");
            //sb.Append(" and cdrbrdta.lntrseq=cdrlndta.trseq and left(convert(char(8), cdrbrhad.brdate, 112), 6)<=left(convert(char(8), dateadd(month, -1, getdate()), 112), 6) and cdrbrhad.status='Y'), 0 )) <>0  ");
            //sb.Append(" ) as a where objtype='CA' GROUP BY objtype,depno,cdesc ");
            //sb.Append(" ORDER BY objtype,depno,cdesc ");

            // change for 20200302
            sb.Append(" select objtype,depno,cdesc,isnull(sum(cost),0) as cost,0 as actor from (  ");
            sb.Append(" SELECT cdrlndta.facno,  cdrlnhad.objtype,  cdrlnhad.depno,  invcls.itclscode,  invmas.itcls,  invcls.clsdsc,  ");
            sb.Append(" (SELECT cdesc FROM miscode WHERE cdrlnhad.depno = miscode.code AND miscode.ckind = 'GE') AS cdesc,cdrlndta.itnbr,  invmas.itdsc,  ");
            sb.Append(" sum ((trnqy1 - isnull((select sum(brpqy1) from cdrbrdta, cdrbrhad  ");
            sb.Append(" where cdrbrdta.facno=cdrbrhad.facno AND cdrbrdta.prono = cdrbrhad.prono and cdrbrdta.brtrno=cdrbrhad.brtrno  ");
            sb.Append(" AND cdrbrdta.facno = cdrlndta.facno AND cdrbrdta.prono = cdrlndta.prono and cdrbrdta.lntrno=cdrlnhad.trno  ");
            sb.Append(" and cdrbrdta.lntrseq=cdrlndta.trseq  ");
            sb.Append(" AND cdrbrdta.facno = 'C' AND cdrbrdta.prono = '1'  ");
            sb.Append(" and left(convert(char(8), cdrbrhad.brdate, 112), 6)<=left(convert(char(8), dateadd(month, -1, getdate()), 112), 6) and cdrbrhad.status='Y'), 0 ))  ");
            sb.Append(" *(case substring(invmas.judco, 2, 1) when '1' then 1 when '3' then invmas.rate2 else 1 end )) as cp_unretqy1,  ");
            sb.Append(" ( select cast( (invpri.unittotcst/ invpri.unittotqy) as decimal(9, 2)) from invpri  ");
            sb.Append(" where invpri.itnbr=cdrlndta.itnbr and invpri.yearmon=left(convert(char(8), dateadd(month, -1, getdate()), 112), 6) )*  ");
            sb.Append(" ( sum ((trnqy1 - isnull((select sum(brpqy1) from cdrbrdta, cdrbrhad  ");
            sb.Append(" where cdrbrdta.facno=cdrbrhad.facno AND cdrbrdta.prono = cdrbrhad.prono and cdrbrdta.brtrno=cdrbrhad.brtrno  ");
            sb.Append(" AND cdrbrdta.facno = cdrlndta.facno AND cdrbrdta.prono = cdrlndta.prono and cdrbrdta.lntrno=cdrlnhad.trno  ");
            sb.Append(" and cdrbrdta.lntrseq=cdrlndta.trseq  ");
            sb.Append(" AND cdrbrdta.facno = 'C' AND cdrbrdta.prono = '1'  ");
            sb.Append(" and left(convert(char(8), cdrbrhad.brdate, 112), 6)<=left(convert(char(8), dateadd(month, -1, getdate()), 112), 6) and cdrbrhad.status='Y'), 0 ))  ");
            sb.Append(" *(case substring(invmas.judco, 2, 1) when '1' then 1 when '3' then invmas.rate2 else 1 end )) ) as cost  ");
            sb.Append(" FROM cdrlndta,  cdrlnhad,  invmas,  invcls  ");
            sb.Append(" WHERE ( cdrlnhad.facno=cdrlndta.facno ) and ( cdrlnhad.prono=cdrlndta.prono ) AND  ");
            sb.Append(" ( cdrlnhad.trno=cdrlndta.trno ) and cdrlndta.itnbr=invmas.itnbr and invmas.itcls=invcls.itcls  ");
            sb.Append(" and cdrlndta.facno='C' AND cdrlndta.prono = '1' and cdrlnhad.status<>'W' ");
            sb.Append(" and cdrlndta.status in ('Y', 'R' ) ");
            sb.Append(" and left(convert(char(8), cdrlnhad.trdate, 112), 6)<=left(convert(char(8), dateadd(month, -1, getdate()), 112), 6)  ");
            sb.Append(" GROUP BY cdrlndta.facno,  cdrlnhad.objtype,  cdrlnhad.depno,  invcls.itclscode,  invmas.itcls,  invcls.clsdsc, cdrlndta.itnbr,  invmas.itdsc  ");
            sb.Append(" having sum (trnqy1 - isnull((select sum(brpqy1) from cdrbrdta, cdrbrhad  ");
            sb.Append(" where cdrbrdta.facno=cdrbrhad.facno AND cdrbrdta.prono = cdrbrhad.prono and cdrbrdta.brtrno=cdrbrhad.brtrno  ");
            sb.Append(" and cdrbrdta.itnbr=cdrlndta.itnbr and cdrbrdta.lntrno=cdrlnhad.trno  ");
            sb.Append(" and cdrbrdta.lntrseq=cdrlndta.trseq AND cdrbrdta.facno = 'C' AND cdrbrdta.prono = '1'  ");
            sb.Append(" and left(convert(char(8), cdrbrhad.brdate, 112), 6)<=left(convert(char(8), dateadd(month, -1, getdate()), 112), 6) and cdrbrhad.status='Y'), 0 )) <>0  ");
            sb.Append(" ) as a where objtype='CA' GROUP BY objtype,depno,cdesc  ");
            sb.Append(" ORDER BY objtype,depno,cdesc  ");
            Fill(sb.ToString(), ds, "unreturn");
        }
        
    }
}
