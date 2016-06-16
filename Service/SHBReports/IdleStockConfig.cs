using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;
using System.Data.Common;

namespace Hanbell.AutoReport.Config
{
    public class IdleStockConfig : NotificationConfig
    {

        string date00, date03, date05, date06, date12, date24, date36;

        public IdleStockConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new IdleStockDS();
            this.reportList.Add(new IdleStockReport_PP());
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {

            date00 = GetQueryString(this.dbtype, this.dbconnstr, "SELECT  LEFT(CONVERT(CHAR(8), DATEADD(mm, -1, GETDATE()), 112),6) ");
            date03 = GetQueryString(this.dbtype, this.dbconnstr, "SELECT  LEFT(CONVERT(CHAR(8), DATEADD(mm, -4, GETDATE()), 112),6) ");
            date05 = GetQueryString(this.dbtype, this.dbconnstr, "SELECT  LEFT(CONVERT(CHAR(8), DATEADD(mm, -6, GETDATE()), 112),6) ");
            date06 = GetQueryString(this.dbtype, this.dbconnstr, "SELECT  LEFT(CONVERT(CHAR(8), DATEADD(mm, -7, GETDATE()), 112),6) ");
            date12 = GetQueryString(this.dbtype, this.dbconnstr, "SELECT  LEFT(CONVERT(CHAR(8), DATEADD(mm, -13, GETDATE()), 112),6) ");
            date24 = GetQueryString(this.dbtype, this.dbconnstr, "SELECT  LEFT(CONVERT(CHAR(8), DATEADD(mm, -25, GETDATE()), 112),6) ");
            date36 = GetQueryString(this.dbtype, this.dbconnstr, "SELECT  LEFT(CONVERT(CHAR(8), DATEADD(mm, -37, GETDATE()), 112),6) ");

            string sqlstr = "select wareh,itnbr,itcls,itclscode," +
                " sum(case when yearmon >'{1}' and yearmon <='{0}' then trnqys else 0 end) as period2," +
                " sum(case when yearmon >'{2}' and yearmon <='{1}' then trnqys else 0 end) as period3," +
                " sum(case when yearmon >'{3}' and yearmon <='{2}' then trnqys else 0 end) as period4," +
                " sum(case when yearmon >'{4}' and yearmon <='{3}' then trnqys else 0 end) as period5, " +
                " sum(case when yearmon >'{5}' and yearmon <='{4}' then trnqys else 0 end) as period6, " +
                " sum(case when yearmon >'{6}' and yearmon <='{5}' then trnqys else 0 end) as period7 " +
                " from invmon " +
                " where facno = '{7}' and prono = '1' and wareh in {8} and trtype not in {9}  and yearmon <='{0}' and yearmon >'{6}' and trnqys>0 " +
                " group by wareh,itnbr,itcls,itclscode ";
            Fill(String.Format(sqlstr, date00, date03, date05, date06, date12, date24, date36, args["facno"], args["wareh"], args["trtype"]), ds, "tblinvmonhistory");

            sqlstr = "select wareh,itnbr,itcls,itclscode," +
                " sum(case when yearmon >'{1}' and yearmon <='{0}' then trnqys else 0 end) as period2," +
                " sum(case when yearmon >'{2}' and yearmon <='{1}' then trnqys else 0 end) as period3," +
                " sum(case when yearmon >'{3}' and yearmon <='{2}' then trnqys else 0 end) as period4," +
                " sum(case when yearmon >'{4}' and yearmon <='{3}' then trnqys else 0 end) as period5, " +
                " sum(case when yearmon >'{5}' and yearmon <='{4}' then trnqys else 0 end) as period6, " +
                " sum(case when yearmon >'{6}' and yearmon <='{5}' then trnqys else 0 end) as period7 " +
                " from invmonh " +
                " where facno = '{7}' and prono = '1' and wareh in {8} and trtype not in {9}  and yearmon <='{0}' and yearmon >'{6}' and trnqys>0 " +
                " group by wareh,itnbr,itcls,itclscode ";
            Fill(String.Format(sqlstr, date00, date03, date05, date06, date12, date24, date36, args["facno"], args["wareh"], args["trtype"]), ds, "tblinvmonhistory");

            sqlstr = "select yearmon ='" + date00 + "',m.wareh,h.whdsc,m.itclscode,m.itcls,n.genre1,m.itnbr,n.itdsc," +
                " price = round(i.unitavgcst,4),period1 = m.trnqys,total1 = round((i.unitavgcst * m.trnqys),2), " +
                " period2=0.0,total2=0.0,period3=0.0,total3=0.0,period4=0.0,total4=0.0,period5=0.0,total5=0.0," +
                " period6=0.0,total6=0.0,period7=0.0,total7=0.0,period8=0.0,total8=0.0 " +
                " from invmon m, invpri i,invmas n,invwh h where m.trtype = 'ZZZ' and m.iocode = 'Z' and m.trnqys > 0 " +
                " and m.yearmon = i.yearmon and m.facno = i.facno and m.itnbr = i.itnbr and m.itnbr=n.itnbr " +
                " and m.wareh = h.wareh and h.costyn='Y' " +
                " and m.facno = '{0}' and m.yearmon = '" + date00 + "' and m.prono = '1' and m.wareh in {1} " +
                " order by m.wareh,m.itclscode,m.itnbr ";
            Fill(String.Format(sqlstr, args["facno"], args["wareh"]), ds, "tblinvmon");


        }

        public override void ConfigData()
        {
            string computeqty, itnbr, wareh;
            string filter = " itnbr='{0}' ";
            decimal qty, qty1;//, qty2, qty3,qty4;
            foreach (DataRow item in ds.Tables["tblinvmon"].Rows)
            {
                qty1 = decimal.Parse(item["period1"].ToString());
                itnbr = item["itnbr"].ToString();
                wareh = item["wareh"].ToString();
                qty = 0;
                computeqty = ds.Tables["tblinvmonhistory"].Compute("SUM(period2)", string.Format(filter, itnbr)).ToString();
                if (computeqty != null && computeqty != "")
                {
                    qty = decimal.Parse(computeqty);
                }
                //if (qty > qty1)
                //{
                //    item["period2"] = qty1;
                //    continue;
                //}
                //else
                //{
                //    item["period2"] = qty;
                //    qty2 = qty1 - qty;
                //}取消累进算法
                if (qty > 0)
                {
                    item["period2"] = qty1;
                    continue;
                }

                qty = 0;
                computeqty = ds.Tables["tblinvmonhistory"].Compute("SUM(period3)", string.Format(filter, itnbr)).ToString();
                if (computeqty != null && computeqty != "")
                {
                    qty = decimal.Parse(computeqty);
                }
                //if (qty > qty2)
                //{
                //    item["period3"] = qty2;
                //    continue;
                //}
                //else
                //{
                //    item["period3"] = qty;
                //    qty3 = qty2 - qty;
                //}
                if (qty > 0)
                {
                    item["period3"] = qty1;
                    continue;
                }

                qty = 0;
                computeqty = ds.Tables["tblinvmonhistory"].Compute("SUM(period4)", string.Format(filter, itnbr)).ToString();
                if (computeqty != null && computeqty != "")
                {
                    qty = decimal.Parse(computeqty);
                }
                //if (qty > qty3)
                //{
                //    item["period4"] = qty3;
                //    continue;
                //}
                //else
                //{
                //    item["period4"] = qty;
                //    qty4 = qty3 - qty;
                //}
                if (qty > 0)
                {
                    item["period4"] = qty1;
                    continue;
                }

                qty = 0;
                computeqty = ds.Tables["tblinvmonhistory"].Compute("SUM(period5)", string.Format(filter, itnbr)).ToString();
                if (computeqty != null && computeqty != "")
                {
                    qty = decimal.Parse(computeqty);
                }
                if (qty > 0)
                {
                    item["period5"] = qty1;
                    continue;
                }

                qty = 0;
                computeqty = ds.Tables["tblinvmonhistory"].Compute("SUM(period6)", string.Format(filter, itnbr)).ToString();
                if (computeqty != null && computeqty != "")
                {
                    qty = decimal.Parse(computeqty);
                }
                if (qty > 0)
                {
                    item["period6"] = qty1;
                    continue;
                }

                qty = 0;
                computeqty = ds.Tables["tblinvmonhistory"].Compute("SUM(period7)", string.Format(filter, itnbr)).ToString();
                if (computeqty != null && computeqty != "")
                {
                    qty = decimal.Parse(computeqty);
                }
                if (qty > 0)
                {
                    item["period7"] = qty1;
                    continue;
                }
                else
                {
                    item["period8"] = qty1;
                }
            }


            DataRow newrow;
            foreach (DataRow item in ds.Tables["tblinvmon"].Rows)
            {
                newrow = ds.Tables["tblresult_PP"].NewRow();
                newrow["yearmon"] = item["yearmon"];
                newrow["wareh"] = item["wareh"];
                newrow["whdsc"] = item["whdsc"];
                newrow["itclscode"] = item["itclscode"];
                newrow["itcls"] = item["itcls"];
                newrow["itnbr"] = item["itnbr"];
                newrow["itdsc"] = item["itdsc"];
                newrow["baseqty"] = item["period1"];
                newrow["period1"] = item["period2"];
                newrow["period2"] = item["period3"];
                newrow["period3"] = item["period4"];
                newrow["period4"] = item["period5"];
                newrow["period5"] = item["period6"];
                newrow["period6"] = item["period7"];
                newrow["period7"] = item["period8"];
                newrow["genre1"] = item["genre1"];
                ds.Tables["tblresult_PP"].Rows.Add(newrow);
            }

        }

    }
}
