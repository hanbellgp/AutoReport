using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Config;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class StatisticalReportConfig : NotificationConfig
    {
        public StatisticalReportConfig(DBServerType dbType, string connName, string notification)
        {
             PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
             this.ds = new StatisticalReportDS();
             this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
//            string sqlStr = @"SELECT  h.hmark2 as hmark2,h.kfno as kfno,h.trno as trno,d.trseq as trseq,h.resno as resno,e.cusds,d.itnbr as itnbr,
//         v.itdsc,d.varnr as varnr,d.trnqy1 as trnqy1,a.tramt as tramt,'' as BQ023C,'' as BQ133C,'' as propotion,'' as CUSTOMER ,'' as MY008 
//from invdtah d,invhadh h,invtrnh a,invmas v,miscode e  where d.facno=h.facno and d.prono =h.prono and d.trno = h.trno
//                and  a.trno = d.trno and d.trseq = a.trseq and d.facno = a.facno and a.prono =d.prono and d.itnbr = v.itnbr and e.code = h.resno
//                and a.facno='C' and a.prono='1' and (a.trtype='IAF' or a.trtype='IAG') and h.resno IN ('1002','1001','1003','1004','1013','1014','003')
//                and e.ckind = '1A' AND (h.trdate >='20180101' and h.trdate <= '20180501')  order by h.kfno";
//            Fill(sqlStr, ds, "SRtlb");

            string sqlStr = @"SELECT vip.facno,vip.prono, vip.hmark2 as hmark2,vip.kfno as kfno,vip.trno as trno,vip.trseq as trseq,vip.resno as resno,vip.cusds,vip.itnbr as itnbr,
            vip.itdsc,vip.varnr as varnr,vip.trnqy1 as trnqy1,'' as tramt,'' as BQ023C,'' as BQ133C,'' as propotion,'' as CUSTOMER ,'' as MY008 from
            (
             SELECT h.facno,h.prono,h.trno, h.hmark2,h.kfno,h.trno,d.trseq,h.resno,e.cusds,d.itnbr,
            v.itdsc,d.varnr,d.trnqy1
            from invdtah d,invhadh h,invmas v,miscode e
            where d.facno=h.facno and d.prono =h.prono and d.trno = h.trno
           and e.code = h.resno
           and d.itnbr = v.itnbr and e.ckind = '1A'
          and d.facno='C' and d.prono='1' and h.facno='C' and h.prono='1' and (h.trtype in ('IAF' ,'IAG'))
           and h.resno IN ('1002','1001','1003','1004','1013','1014','0003')
          AND (h.trdate >='20180101' and h.trdate <= '20180501')
          ) vip order by vip.kfno";

            Fill(sqlStr, ds, "SRtlb");

            string tramtSql = @"select a.facno,a.prono,a.trno,a.tramt from  invtrnh a where  a.facno='C' and a.prono='1'
            and (a.trtype in ('IAF' ,'IAG')) and a.trdate >='20180101' and a.trdate <= '20180501'";

            Fill(tramtSql, ds, "tramttlb");

            foreach (DataRow item in ds.Tables["SRtlb"].Rows)
            {
                foreach (DataRow row in ds.Tables["tramttlb"].Rows)
                {
                    if (item["facno"].ToString() == row["facno"].ToString() && item["prono"].ToString() == row["prono"].ToString() && item["trno"].ToString() == row["trno"].ToString())
                    {
                        item["tramt"] = row["tramt"];
                    }
                }
            }
        }
    }
}
