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
            //总表 把附表的所有数据加在这个table中 
            //StringBuilder sqlStr = new StringBuilder();
            ////上海汉钟ERP数据
            //sqlStr.Append("SELECT vip.facno,vip.prono, vip.hmark2 as hmark2,vip.kfno as kfno,vip.trno as trno,vip.resno as resno,vip.cusds,");
            //sqlStr.Append(" vip.varnr as varnr,vip.itnbr,vip.itdsc,vip.trnqy1 as trnqy1,'' as tramt,'' as BQ023C,'' as BQ504C,'' as propotion,'' as CUSTOMER ,'' as MY008 ,'' as total,'' as BQ501 from ");
            //sqlStr.Append(" ( SELECT  h.facno,h.prono,h.trno, h.hmark2,h.kfno,h.trno,d.trseq,h.resno,e.cusds,d.itnbr, v.itdsc,d.varnr,d.trnqy1");
            //sqlStr.Append("  from invdtah d,invhadh h,invmas v,miscode e  where d.facno=h.facno and d.prono =h.prono and d.trno = h.trno ");
            //sqlStr.Append(" and e.code = h.resno  and d.itnbr = v.itnbr and e.ckind = '1A'  and d.facno='C' and d.prono='1' and h.facno='C' and h.prono='1' and (h.trtype in ('IAF' ,'IAG'))");
            ////sqlStr.Append(" and h.resno IN ('1002','1001','1003','1004','1013','1014','0003') and h.kfno <> '' AND year(h.trdate)=2018 AND datediff(mm,h.trdate,getdate())<=12  ) vip GROUP BY vip.kfno ;");
            //sqlStr.Append(" and h.resno IN ('1002','1001','1003','1004','1013','1014','0003') and h.kfno <> '' AND h.trdate>='20180101' AND h.trdate<='20190228'  ) vip GROUP BY vip.kfno ;");

            //柯茂ERP数据
            //sqlStr.Append("SELECT vip.facno,vip.prono, vip.hmark2 as hmark2,vip.kfno as kfno,vip.trno as trno,vip.resno as resno,vip.cusds,");
            //sqlStr.Append(" vip.varnr as varnr,vip.itnbr,vip.itdsc,vip.trnqy1 as trnqy1,'' as tramt,'' as BQ023C,'' as BQ504C,'' as propotion,'' as CUSTOMER ,'' as MY008 ,'' as total,'' as BQ501 from ");
            //sqlStr.Append(" ( SELECT  h.facno,h.prono,h.trno, h.hmark2,h.kfno,h.trno,d.trseq,h.resno,e.cusds,d.itnbr, v.itdsc,d.varnr,d.trnqy1");
            //sqlStr.Append("  from invdtah d,invhadh h,invmas v,miscode e  where d.facno=h.facno and d.prono =h.prono and d.trno = h.trno ");
            //sqlStr.Append(" and e.code = h.resno  and d.itnbr = v.itnbr and e.ckind = '1A'  and d.facno='K' and d.prono='1' and h.facno='K' and h.prono='1' and (h.trtype in ('IAF' ,'IAG'))");
            ////sqlStr.Append(" and h.resno IN ('1002','1001','1003','1004','1013','1014','0003') and h.kfno <> '' AND year(h.trdate)=2018 AND datediff(mm,h.trdate,getdate())<=12  ) vip GROUP BY vip.kfno ;");
            //sqlStr.Append(" and h.resno IN ('1002','1001','1003','1004','1013','1014','0003') and h.kfno <> '' AND h.trdate>='20180101' AND h.trdate<='20190228'  ) vip GROUP BY vip.kfno ;");
            //总表 把OA的数据作为总表
            StringBuilder sqlOAStr = new StringBuilder();
            sqlOAStr.Append(" select BQ197,BQ001,''as trno,'' as resno, (CASE WHEN BQ500 <> '' then BQ500 else BQ129 end ) as BQ500,'' as itnbr,'' as itdsc,'' as varnr,'' as trnqy1,'' as tramt, ");
            sqlOAStr.Append(" BQ023C, (CASE WHEN BQ504 <> '' then concat(BQ504,BQ504C) else concat(BQ133,BQ133C) end ) as BQ504C,propotion,BQ002C,'' as MY008,'' as total,(CASE when BQ501<>'' then BQ501 else BQ130  end ) as BQ501 ");
            sqlOAStr.Append(" from SERI12 where BQ035 = 'Y' and convert(varchar(7),BQ021,112)>='2018/01' AND convert(varchar(7),BQ021,112)<='2019/04' ");
            Fill(sqlOAStr.ToString(), ds, "SRtlb");
            
            //StringBuilder ERPYfsql = new StringBuilder();
            ////上海汉钟数据
            //ERPYfsql.Append("select kfno,fwno,freight as 'total',h.cusno,s.cusna from cdrlnhad h LEFT JOIN cdrfre c on c.shpno = h.trno and c.facno = h.facno LEFT JOIN cdrcus s on h.cusno = s.cusno ");
            //ERPYfsql.Append(" where  h.facno='C' AND h.status ='Y' and  h.resno = '03' and ( h.fwno <> ''and h.fwno <> '-') and (h.kfno <> '' and h.kfno <> '-') and c.freight> 0 ");
            ////ERPYfsql.Append(" AND year(h.cfmdate)=2018 AND datediff(mm,h.cfmdate,getdate())<=12 ");
            //ERPYfsql.Append(" AND h.cfmdate>='20180101' AND h.cfmdate<='20190228' ");
            //Fill(ERPYfsql.ToString(), ds, "ERPYF");

            //柯茂ERP数据
            //ERPYfsql.Append("select kfno,fwno,freight as 'total',h.cusno,s.cusna from cdrlnhad h LEFT JOIN cdrfre c on c.shpno = h.trno and c.facno = h.facno LEFT JOIN cdrcus s on h.cusno = s.cusno ");
            //ERPYfsql.Append(" where  h.facno='K' AND h.status ='Y' and  h.resno = '03' and ( h.fwno <> ''and h.fwno <> '-') and (h.kfno <> '' and h.kfno <> '-') and c.freight> 0 ");
            ////ERPYfsql.Append(" AND year(h.cfmdate)=2018 AND datediff(mm,h.cfmdate,getdate())<=12 ");
            //ERPYfsql.Append(" AND h.cfmdate>='20180101' AND h.cfmdate<='20190228' ");

        }
    }
}
