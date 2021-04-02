using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class PurQuantityGrowRatioConfig:NotificationConfig
    {
        public PurQuantityGrowRatioConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new PurQuantityGrowRatioDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select a.itnbr,a.itdsc,a.oldyear,a.newyear,a.oldquarterall, ");
            sb.Append(" cast((case when oldquarterall>0 then (newyear-oldquarterall)/oldquarterall else 1 end) as decimal(12,2))  as gr1, ");
            sb.Append(" lastquarter,currentquarter, ");
            sb.Append(" cast((case when lastquarter>0 then (currentquarter-lastquarter)/lastquarter else 1 end) as decimal(12,2)) as gr2, ");
            sb.Append(" oldquarter, ");
            sb.Append(" cast((case when oldquarter>0 then (currentquarter-oldquarter)/oldquarter else 1 end) as decimal(12,2)) as gr3 ");
            sb.Append(" from ( ");
            sb.Append(" select a.itnbr ,a.itdsc , ");
            sb.Append(" isnull(sum(CASE when year(a.trdat)=datepart(YEAR,getdate())-1 then a.payqty end),0) as oldyear, ");
            sb.Append(" isnull(sum(CASE when year(a.trdat)=datepart(YEAR,getdate()) then a.payqty end),0) as newyear, ");
            sb.Append(" isnull(sum(CASE when (year(a.trdat)=datepart(YEAR,getdate())-1 and datepart(QUARTER,a.trdat)<=datepart(QUARTER,getdate())) ");
            sb.Append(" then a.payqty end),0) as oldquarterall, ");
            sb.Append(" isnull(sum(CASE when (year(a.trdat)=2020 and datepart(QUARTER,a.trdat)=CASE when ");
            sb.Append(" datepart(QUARTER,getdate())-1=0 then 4 else datepart(QUARTER,getdate())-1 end) then a.payqty end),0) as lastquarter, ");
            sb.Append(" isnull(sum(CASE when (year(a.trdat)=datepart(YEAR,getdate()) and datepart(QUARTER,a.trdat)=datepart(QUARTER,getdate())) ");
            sb.Append(" then a.payqty end),0) as currentquarter, ");
            sb.Append(" isnull(sum(CASE when (year(a.trdat)=datepart(YEAR,getdate())-1 and datepart(QUARTER,a.trdat)=datepart(QUARTER,getdate())) ");
            sb.Append(" then a.payqty end),0) as oldquarter ");
            sb.Append(" from dbo.apmpyh a ");
            sb.Append(" where a.facno = 'C' and a.prono = '1' and  a.pyhkind = '1' ");
            sb.Append(" and	( a.itnbr <> '9' and a.itnbr not like '%GB%') ");
            sb.Append(" and year(a.trdat)>= datepart(YEAR,getdate())-1 ");
            sb.Append(" GROUP BY a.itnbr,a.itdsc ");
            sb.Append(" ) a ");
            Fill(sb.ToString(), ds, "tbl");

            foreach (DataRow item in ds.Tables["tbl"].Rows)
            {
                Double jn = Double.Parse(item["newyear"].ToString());
                Double qn = Double.Parse(item["oldquarterall"].ToString());
                Double sj = Double.Parse(item["lastquarter"].ToString());
                Double dj = Double.Parse(item["currentquarter"].ToString());
                Double tj = Double.Parse(item["oldquarter"].ToString());
                if (jn > 0.0000 && qn<=0.0000) {
                    item["gr1"] = "100%";
                }
                else if (jn == qn)
                {
                    item["gr1"] = "0%";
                }
                else {
                    item["gr1"] = Double.Parse(item["gr1"].ToString()) * 100 + "%";
                }
                if (dj > 0.0000 && sj <= 0.0000)
                {
                    item["gr2"] = "100%";
                }
                else if (dj == sj)
                {
                    item["gr2"] = "0%";
                }
                else
                {
                    item["gr2"] = Double.Parse(item["gr2"].ToString()) * 100 + "%";
                }
                if (dj > 0.0000 && tj <= 0.0000)
                {
                    item["gr3"] = "100%";
                }
                else if (dj == tj)
                {
                    item["gr3"] = "0%";
                }
                else
                {
                    item["gr3"] = Double.Parse(item["gr3"].ToString()) * 100 + "%";
                }
                ds.Tables["tbl"].AcceptChanges();
            }
        }
    }
}
