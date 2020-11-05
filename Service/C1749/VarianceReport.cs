using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class VarianceReport : NotificationContent
    {
        public VarianceReport() { }

        protected override void Init()
        {
            base.Init();
            nc = new VarianceReportConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            if (nc.GetReportList().Count > 0)
            {
                SetAttachment();
            }
            String[] title = { "公司别", "SHB本日", "集团本日", "差异", "SHB本月","集团本月","差异" };
            int[] width = { 100, 100, 100, 100, 100,100,100 };
            DataTable resultQtyDt = cyQtyTbl();
            DataTable resultAmtDt = cyAmtTbl();
            this.content = GetContent(resultQtyDt, resultAmtDt, title, width);

            if (resultQtyDt.Rows.Count > 0 && resultAmtDt.Rows.Count > 0)
            {
                AddNotify(new MailNotify());
            }
        }
        //数量差异表
        protected DataTable cyQtyTbl() {
            DataTable dt1 = nc.GetDataTable("shbqyt");
            DataTable dt2 = jtQtyTlb();
            DataTable dt3 = dt1.Copy();
            try {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    string facno = dt3.Rows[i]["facno"].ToString().Trim();
                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                       string hangrp = dt2.Rows[j]["hangrp"].ToString().Trim();
                       if (facno.Equals(hangrp))
                       {
                            dt3.Rows[i]["num2"] = dt2.Rows[j]["num2"].ToString();
                            dt3.Rows[i]["num4"] = dt2.Rows[j]["num4"].ToString();
                            dt3.AcceptChanges();
                        }
                    }
                }
                //得到新的datatable 算差异
                foreach (DataRow item in dt3.Rows)
                {
                    item["cy1"] = (Convert.ToDouble(item["num1"].ToString()) - Convert.ToDouble(item["num2"].ToString())).ToString("0.00");
                    item["cy2"] = (Convert.ToDouble(item["num3"].ToString()) - Convert.ToDouble(item["num4"].ToString())).ToString("0.00");
                }
                dt3.AcceptChanges();

                foreach (DataRow item in dt3.Rows)
                {
                    if (Convert.ToDouble(item["cy1"].ToString()) != 0) 
                    {
                        item["cy1"] = "<font color=red>" + item["cy1"] + "</font>";
                    }
                    if (Convert.ToDouble(item["cy2"].ToString()) != 0)
                    {
                        item["cy2"] = "<font color=red>" + item["cy2"] + "</font>";
                    }
                }
                dt3.AcceptChanges();

            }catch(Exception ex){

            }
            return dt3;
        
        }

        //金额差异表
        protected DataTable cyAmtTbl()
        {
            DataTable dt1 = nc.GetDataTable("shbamt");
            DataTable dt2 = jtQtyAmt();
            DataTable dt3 = dt1.Copy();
            try
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    string facno = dt3.Rows[i]["facno"].ToString().Trim();
                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        string hangrp = dt2.Rows[j]["hangrp"].ToString().Trim();
                        if (facno.Equals(hangrp))
                        {
                            dt3.Rows[i]["num2"] = dt2.Rows[j]["num2"].ToString();
                            dt3.Rows[i]["num4"] = dt2.Rows[j]["num4"].ToString();
                            dt3.AcceptChanges();
                        }
                    }
                }
                //得到新的datatable 算差异
                foreach (DataRow item in dt3.Rows)
                {
                    item["cy1"] = (Convert.ToDouble(item["num1"].ToString()) - Convert.ToDouble(item["num2"].ToString())).ToString("0.00");
                    item["cy2"] = (Convert.ToDouble(item["num3"].ToString()) - Convert.ToDouble(item["num4"].ToString())).ToString("0.00");
                }
                dt3.AcceptChanges();
                foreach (DataRow item in dt3.Rows)
                {
                    if (Convert.ToDouble(item["cy1"].ToString()) != 0)
                    {
                        item["cy1"] = "<font color=red>" + item["cy1"] + "</font>";
                    }
                    if (Convert.ToDouble(item["cy2"].ToString()) != 0)
                    {
                        item["cy2"] = "<font color=red>" + item["cy2"] + "</font>";
                    }
                }
                dt3.AcceptChanges();

            }
            catch (Exception ex)
            {

            }
            return dt3;
        }

        protected DataTable jtQtyTlb() { 
            StringBuilder sb = new StringBuilder();
            sb.Append(" select  hangrp , 0 as num1, ");
            sb.Append(" sum(CASE when convert(VARCHAR(6),trdate,112) = convert(VARCHAR(6),getdate(),112) and trdate=convert(char,dateadd(DD,-1,getdate()),112) then saleqty end ) as num2 , ");
            sb.Append(" 0 as cy1, ");
            sb.Append(" 0 as num3, ");
            sb.Append(" sum(CASE when convert(VARCHAR(6),trdate,112) = convert(VARCHAR(6),getdate(),112) and trdate<=convert(char,dateadd(DD,-1,getdate()),112) then saleqty end ) as num4, ");
            sb.Append(" 0 as cy2   from  N_RPT_grpsdailytmp ");
            sb.Append(" GROUP BY hangrp ");
            return nc.GetQueryTable(DBServerType.SybaseASE, Base.GetDBConnectionString("THBERP"), sb.ToString());
            
        }

        protected DataTable jtQtyAmt()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select  hangrp , 0 as num1, ");
            sb.Append(" convert(DECIMAL,sum(CASE when convert(VARCHAR(6),trdate,112) = convert(VARCHAR(6),getdate(),112) and trdate=convert(char,dateadd(DD,-1,getdate()),112) then saleamtfs end ) / (case hangrp when 'V' then 1000000 else 10000 end  ),0) as num2 , ");
            sb.Append(" 0 as cy1, ");
            sb.Append(" 0 as num3, ");
            sb.Append(" convert(DECIMAL,sum(CASE when convert(VARCHAR(6),trdate,112) = convert(VARCHAR(6),getdate(),112) and trdate<=convert(char,dateadd(DD,-1,getdate()),112) then saleamtfs end ) / (case hangrp when 'V' then 1000000 else 10000 end  ))  as num4, ");
            sb.Append(" 0 as cy2   from  N_RPT_grpsdailytmp ");
            sb.Append(" GROUP BY hangrp ");
            return nc.GetQueryTable(DBServerType.SybaseASE, Base.GetDBConnectionString("THBERP"), sb.ToString());

        }

        protected string GetContent(DataTable tbl, DataTable tbl2, string[] title, int[] width)
        {
            if (title != null && width != null && title.Length != width.Length)
            {
                return "指定的标题与栏位宽度设定不一致";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(GetContentHead());
            sb.Append("<p>集团报表数量差异</p>");
            sb.Append("<p>制表时间" + DateTime.Now.ToString("yyyy年MM月dd日") + "</p>");
            sb.Append(GetHTMLTable(tbl, title, width));

            sb.Append("<p>集团报表金额差异</p>");
            sb.Append("<p>制表时间" + DateTime.Now.ToString("yyyy年MM月dd日") + "</p>");
            sb.Append(GetHTMLTable(tbl2, title, width));
            sb.Append(GetContentFooter());
            return sb.ToString();
        }


    }
}
