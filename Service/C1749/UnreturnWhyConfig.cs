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
    class UnreturnWhyConfig : NotificationConfig
    {
        public UnreturnWhyConfig(DBServerType dbType, string ConnName, string notifaction)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(ConnName));
            this.ds = new UnreturnDS();
            this.args = Base.GetParameter(notifaction, this.ToString());
        }
        public override void InitData()
        {
            //按借出原因
            StringBuilder sbwhy = new StringBuilder();
            sbwhy.Append(" select objtype,cdesc,isnull(sum(cost),0) as cost,0 as actor from ( ");
            sbwhy.Append(" SELECT cdrlndta.facno,  cdrlnhad.objtype,  cdrlnhad.depno,  invcls.itclscode,  invmas.itcls,  invcls.clsdsc, ");
            sbwhy.Append("  miscode.cdesc,cdrlndta.itnbr,  invmas.itdsc,  sum ((trnqy1 - isnull((select sum(brpqy1) ");
            sbwhy.Append("  from cdrbrdta, cdrbrhad where cdrbrdta.facno=cdrbrhad.facno and cdrbrdta.brtrno=cdrbrhad.brtrno  ");
            sbwhy.Append(" and cdrbrdta.itnbr=cdrlndta.itnbr and cdrbrdta.lntrno=cdrlnhad.trno ");
            sbwhy.Append(" and cdrbrdta.lntrseq=cdrlndta.trseq and left(convert(char(8), cdrbrhad.brdate, 112), 6)<=left(convert(char(8), dateadd(month, -1, getdate()), 112), 6) and cdrbrhad.status='Y'), 0 )) ");
            sbwhy.Append(" *(case substring(invmas.judco, 2, 1) when '1' then 1 when '3' then invmas.rate2 else 1 end )) as cp_unretqy1, ");
            sbwhy.Append(" ( select cast( (invpri.unittotcst/ invpri.unittotqy) as decimal(9, 2)) from invpri where invpri.itnbr=cdrlndta.itnbr and invpri.yearmon=left(convert(char(8), dateadd(month, -1, getdate()), 112), 6) )* ");
            sbwhy.Append(" ( sum ((trnqy1 - isnull((select sum(brpqy1) from cdrbrdta, cdrbrhad where cdrbrdta.facno=cdrbrhad.facno and cdrbrdta.brtrno=cdrbrhad.brtrno ");
            sbwhy.Append(" and cdrbrdta.itnbr=cdrlndta.itnbr and cdrbrdta.lntrno=cdrlnhad.trno ");
            sbwhy.Append(" and cdrbrdta.lntrseq=cdrlndta.trseq and left(convert(char(8), cdrbrhad.brdate, 112), 6)<=left(convert(char(8), dateadd(month, -1, getdate()), 112), 6) and cdrbrhad.status='Y'), 0 )) ");
            sbwhy.Append(" *(case substring(invmas.judco, 2, 1) when '1' then 1 when '3' then invmas.rate2 else 1 end )) ) as cost ");
            sbwhy.Append(" FROM cdrlndta, cdrlnhad LEFT JOIN miscode on miscode.code=cdrlnhad.resno and miscode.ckind = 'IL',  invmas,  invcls ");
            sbwhy.Append(" WHERE ( cdrlnhad.facno=cdrlndta.facno ) and ");
            sbwhy.Append(" ( cdrlnhad.trno=cdrlndta.trno ) and cdrlndta.itnbr=invmas.itnbr and cdrlndta.status in ('Y', 'R' ) and invmas.itcls=invcls.itcls ");
            sbwhy.Append(" and left(convert(char(8), cdrlnhad.trdate, 112), 6)<=left(convert(char(8), dateadd(month, -1, getdate()), 112), 6) and cdrlndta.facno='C' and cdrlnhad.status<>'W' ");
            sbwhy.Append(" GROUP BY cdrlndta.facno,  cdrlnhad.objtype,  cdrlnhad.depno,  invcls.itclscode,  invmas.itcls,  invcls.clsdsc,miscode.cdesc,   cdrlndta.itnbr,  invmas.itdsc ");
            sbwhy.Append(" having sum (trnqy1 - isnull((select sum(brpqy1) from cdrbrdta, cdrbrhad where cdrbrdta.facno=cdrbrhad.facno and cdrbrdta.brtrno=cdrbrhad.brtrno ");
            sbwhy.Append(" and cdrbrdta.itnbr=cdrlndta.itnbr and cdrbrdta.lntrno=cdrlnhad.trno ");
            sbwhy.Append(" and cdrbrdta.lntrseq=cdrlndta.trseq and left(convert(char(8), cdrbrhad.brdate, 112), 6)<=left(convert(char(8), dateadd(month, -1, getdate()), 112), 6) and cdrbrhad.status='Y'), 0 )) <>0  ");
            sbwhy.Append(" ) as a where objtype='CA' GROUP BY objtype,cdesc ");
            sbwhy.Append(" ORDER BY objtype,cdesc ");

            Fill(sbwhy.ToString(), ds, "unreturn_why");
        }
    }
}
