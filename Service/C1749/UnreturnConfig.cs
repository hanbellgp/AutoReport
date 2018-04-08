using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    class UnreturnConfig : NotificationConfig
    {
        public UnreturnConfig(DBServerType dbType, string ConnName, string notifaction)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(ConnName));
            this.ds = new UnreturnDS();
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
        }
    }
}
