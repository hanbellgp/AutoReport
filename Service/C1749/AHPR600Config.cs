using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class AHPR600Config : NotificationConfig
    {
        public AHPR600Config(DBServerType dbType, string ConnName, string notifaction)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(ConnName));
            this.ds = new AHPR600DS();
            this.args = Base.GetParameter(notifaction, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(" SELECT *, (t.W01 + t.EW01 + t.FAAFL + t.FAA09 + t.FAASC + t.FTZ01 + t.FTZ02 + t.FAAJY) AS rowsum FROM ( ");
            //sb.Append(" SELECT cdrdmmodel.cmcmodel,a.itnbr,a.fixnr, ");
            //sb.Append(" isnull(sum((CASE a.wareh WHEN 'W01' THEN a.onhand1 end )),0) AS 'W01', ");
            //sb.Append(" isnull(sum((CASE a.wareh WHEN 'EW01' THEN a.onhand1 end )),0) AS 'EW01', ");
            //sb.Append(" isnull(sum((CASE a.wareh WHEN 'FAAFL发料' THEN a.onhand1 end )),0) AS 'FAAFL', ");
            //sb.Append(" isnull(sum((CASE a.wareh WHEN 'FAA09机体试漏站' THEN a.onhand1 end )),0) AS 'FAA09', ");
            //sb.Append(" isnull(sum((CASE a.wareh WHEN 'FAASC机体试车' THEN a.onhand1 end )),0) AS 'FAASC', ");
            //sb.Append(" isnull(sum((CASE a.wareh WHEN 'FTZ01涂装' THEN a.onhand1 end )),0) AS 'FTZ01', ");
            //sb.Append(" isnull(sum((CASE a.wareh WHEN 'FTZ02涂装下线' THEN a.onhand1 end )),0) AS 'FTZ02', ");
            //sb.Append(" isnull(sum((CASE a.wareh WHEN 'FAAJY检验入库站' THEN a.onhand1 end )),0) AS 'FAAJY' ");
            //sb.Append("   from ( ");
            //sb.Append(" select invbat.itnbr,wareh, ");
            //sb.Append(" case when isnull(invbat.fixnr,'')='' then '0.00'  when invbat.fixnr='%' then '0.00' else invbat.fixnr end  as fixnr ");
            //sb.Append(" ,sum(onhand1) as onhand1,sum(onhand2) as onhand2  ");
            //sb.Append(" from invbat,invmas   ");
            //sb.Append(" where invbat.itnbr=invmas.itnbr AND invbat.itcls = invmas.itcls AND invbat.itclscode = invmas.itclscode ");
            //sb.Append(" and  invmas.itnbr in ('37312-A00X-YC','3712-9QC119B29002','3712-9QC119512001') ");
            //sb.Append(" and (invbat.onhand1<>0) and (wareh='W01' or wareh='ASRS03' or wareh='M01' or wareh='EW01')  ");
            //sb.Append(" group by invbat.itnbr,wareh,fixnr  ");
            //sb.Append(" union all ");
            //sb.Append(" select manwipbat.itnbr,manwipbat.prosscode+borprc.prossname, ");
            //sb.Append(" case when isnull(manwipbat.fixnr,'')='' then '0.00'  when manwipbat.fixnr='%' then '0.00' else manwipbat.fixnr end  as fixnr, ");
            //sb.Append(" sum(onhand1),0 from manwipbat ");
            //sb.Append(" left outer join borprc on borprc.prosscode=manwipbat.prosscode ");
            //sb.Append(" ,invmas,manmas where manwipbat.itnbr=invmas.itnbr and  ");
            //sb.Append(" manwipbat.facno=manmas.facno AND manwipbat.prono=manmas.prono and manwipbat.manno=manmas.manno    ");
            //sb.Append(" and invmas.itnbr in ('37312-A00X-YC','3712-9QC119B29002','3712-9QC119512001') ");
            //sb.Append(" and onhand1<>0 and linecode='AH' group by manwipbat.itnbr,manwipbat.prosscode+borprc.prossname,manwipbat.fixnr) a   ");
            //sb.Append(" left outer join cdrdmmodel on cdrdmmodel.itnbr=a.itnbr and  cdrdmmodel.cmccode=a.fixnr  ");
            //sb.Append(" GROUP BY cdrdmmodel.cmcmodel,a.itnbr,a.fixnr  ");
            //sb.Append(" ) AS t ");

            //sb.Append(" select varnr from manwipbat where facno = 'C' and prono = '1' ");
            //sb.Append(" and itnbr in ('37312-A00X-YC','3712-9QC119B29002','3712-9QC119512001') ");
            //sb.Append(" group by varnr ");



            //sb.Append(" SELECT b.facno,b.itnbr,b.itcls, sum(b.onhand1) AS 'onhandy' ,'0' AS 'inavgcst' ,'0' AS 'insumcst'  FROM invbal b   ");
            //sb.Append(" WHERE b.onhand1 <> 0 AND b.facno = 'C' AND b.prono ='1' ");
            //sb.Append(" and b.itcls in  ('1101','1102','1201','1202','2101','2102','2201','2202','3101','3102','3108','3201','3202',  ");
            //sb.Append(" '1801','1802','2801','2802','3801','3802', ");
            //sb.Append(" '1401','1402','2401','2402','3401','3402','1014','2012','2013','2015','3012','3013','3015') ");
            //sb.Append(" GROUP BY b.facno,b.itnbr,b.itcls   ");
            //Fill(sb.ToString(), ds, "tblinverntry");
            //sb.Length = 0;
            //foreach (DataRow item in ds.Tables["tblinverntry"].Rows)
            //{
            //    sb.Length = 0;
            //    sb.Append(" SELECT TOP 1 round(p.inavgcst,2) as 'unitavgcst'   ");
            //    sb.Append(" FROM invpri p ");
            //    sb.Append(" WHERE  p.facno = 'C' AND itnbr = '").Append(item["itnbr"].ToString()).Append("'");
            //    sb.Append(" AND p.inavgcst>0 AND p.inavgcst>0 ");
            //    sb.Append(" ORDER BY p.yearmon DESC  ");
            //    Fill(sb.ToString(), ds, "tblcst");
            //    string aa = ds.Tables["tblcst"].Rows.Count > 0 ? ds.Tables["tblcst"].Rows[0][0].ToString() : "0";
            //    item["inavgcst"] = ds.Tables["tblcst"].Rows.Count > 0 ? ds.Tables["tblcst"].Rows[0][0].ToString() : "0";
            //    ds.Tables["tblcst"].Clear();
            //}
            //ds.AcceptChanges();

            sb.Append(" SELECT a.itnbr,cdrdmmodel.cmcmodel,a.wareh,a.onhand1 FROM  ");
            sb.Append(" ( ");
            sb.Append(" select invbat.itnbr,wareh, ");
            sb.Append(" case when isnull(invbat.fixnr,'')='' then '0.00'  when invbat.fixnr='%' then '0.00' else invbat.fixnr end  as fixnr, ");
            sb.Append(" sum(onhand1) as onhand1 ");
            sb.Append(" from invbat,invmas   ");
            sb.Append(" where invbat.itnbr=invmas.itnbr AND invbat.itcls = invmas.itcls AND invbat.itclscode = invmas.itclscode ");
            sb.Append(" and  invmas.itnbr in ('37312-A00X-YC','3712-9QC119B29002','3712-9QC119512001') ");
            sb.Append(" and (invbat.onhand1<>0) and (wareh='W01' or wareh='ASRS03' or wareh='M01' or wareh='EW01') ");
            sb.Append(" group by invbat.itnbr,wareh ,invbat.fixnr ");
            sb.Append(" ) a  ");
            sb.Append(" left outer join cdrdmmodel on cdrdmmodel.itnbr=a.itnbr and  cdrdmmodel.cmccode=a.fixnr ");
            Fill(sb.ToString(), ds, "tbl");
            //清除sb 执行另一段SQL
            sb.Length = 0;
            sb.Append(" SELECT DISTINCT manwipbat.varnr FROM manwipbat  ,manmas ");
            sb.Append(" WHERE manwipbat.facno=manmas.facno AND manwipbat.prono=manmas.prono and manwipbat.manno=manmas.manno ");
            sb.Append(" AND manwipbat.facno = 'C' AND manwipbat.prono = '1'   ");
            sb.Append(" AND manmas.linecode = 'AH' ");
            sb.Append(" AND manwipbat.itnbr in ('37312-A00X-YC','3712-9QC119B29002','3712-9QC119512001')  ");
            Fill(sb.ToString(), ds, "varnrtbl");
            sb.Length = 0;



        }

        public string getVarnr()
        {
            // StringBuilder arr = new StringBuilder();


            var table = ds.Tables["varnrtbl"];

            var tableEn = table.AsEnumerable();

            var sltStr = tableEn.Select(o => o["varnr"].ToString()).ToArray<string>();

            var whereStr = "'" + string.Join("','", sltStr) + "'";
            return whereStr;

            //foreach (DataRow item in ds.Tables["varnrtbl"].Rows)
            //{
            //    if (ds.Tables["tblcst"].Rows.Count<> ){
            //        arr.Append("'").Append(item.ToString()).Append("', "); 
            //    }else{
            //        arr.Append("'").Append(item.ToString()).Append("' "); 
            //    }

            //}
            //return arr.ToString();
        }
    }
}
