using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class StandCostAbnormalityConfig : NotificationConfig
    {

        public StandCostAbnormalityConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new StandCostAbnormalityDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            string sqlstr;
            //外购件
            sqlstr = "select distinct invmas.itcls,invmas.morpcode,invmas.itnbr,invmas.itdsc,invmas.spdsc," +
                " cost=isnull(cost,0),asscost=isnull(asscost,0),cpl=isnull(cpl,0),cpg=isnull(cpg,0),cpf=isnull(cpf,0),cpt=isnull(cpt,0) " +
                " from bomasd,invmas where bomasd.itnbr=invmas.itnbr and invmas.morpcode='P' and isnull(invmas.cost,0)=0 " +
                " and invmas.indate >= '{0}' and invmas.itclscode in {1} ";
            Fill(string.Format(sqlstr, args["indate"].ToString(), args["itclscode"].ToString()), ds, "tblsc");
            //托工件
            sqlstr = "select invmas.itcls,invmas.morpcode,invmas.itnbr,invmas.itdsc,invmas.spdsc," +
               " cost=isnull(cost,0),asscost=isnull(asscost,0),cpl=isnull(cpl,0),cpg=isnull(cpg,0),cpf=isnull(cpf,0),cpt=isnull(cpt,0) " +
               " from bomash,invmas where bomash.itnbrf=invmas.itnbr and bomash.itnbrf not in (select itnbr from  borgrp) " +
               " and invmas.morpcode='A' and isnull(invmas.asscost,0)=0 and invmas.indate >= '{0}' and invmas.itclscode in {1} ";
            Fill(string.Format(sqlstr, args["indate"].ToString(), args["itclscode"].ToString()), ds, "tblsc");

        }

        public override void ConfigData()
        {
            DataTable tbl;
            string sqlstr = "select itnbr,(totamt - taxamt)/payqy as cost from puracd where itnbr = '{0}' AND dateadd(month,{1},acceptdate) >= getdate() order by acceptdate DESC";
            foreach (DataRow item in this.ds.Tables["tblsc"].Rows)
            {
                if (item["morpcode"].ToString()!="P")
                {
                    continue;
                }
                tbl = GetQueryTable(string.Format(sqlstr, item["itnbr"].ToString(),args["count"]));
                if (tbl != null && tbl.Rows.Count > 0)
                {
                    item["cost"] = tbl.Rows[0]["cost"];
                    if (this.args["updatecost"].ToString() == "true")
                    {
                        ExecSql(this.dbtype, this.dbconnstr, "update invmas set cost=" + Decimal.Parse(tbl.Rows[0]["cost"].ToString()) + " where itnbr='" + item["itnbr"].ToString() + "'");
                    }
                }
            }
            this.ds.Tables["tblsc"].AcceptChanges();
        }



    }
}
