using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class IdleProductConfig : NotificationConfig
    {
        public IdleProductConfig()
        {
        }

        public IdleProductConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new IdleProductDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT cdrschedule.cdrno,cdrschedule.itnbrcus,cdrschedule.recdate,cdrschedule.cfmdate,cdrschedule.shipday1,");
            sqlBuilder.Append("cdrschedule.remark1,cdrschedule.cusno,cdrschedule.cusna,cdrschedule.areacode,mancdrmap.mapqty,mancdrmap.varnr ");
            sqlBuilder.Append(" FROM manmas,mancdrmap,cdrschedule ");
            sqlBuilder.Append(" WHERE (manmas.facno = mancdrmap.facno) and (manmas.manno = mancdrmap.manno) and (manmas.manstatus <> 'X') ");
            sqlBuilder.Append(" AND (mancdrmap.varnr in (select distinct varnr from invbat where onhand1<>0 and isnull(varnr,'')<>'' and wareh in {0})) ");
            sqlBuilder.Append(" AND cdrschedule.facno = mancdrmap.facno ");
            sqlBuilder.Append(" AND cdrschedule.cdrno = mancdrmap.cdrno + '-' + (left('000',3 - len(convert(varchar(3),mancdrmap.ctrseq))) + convert(varchar(3),mancdrmap.ctrseq)) ");
            sqlBuilder.Append(" AND datediff(day,cdrschedule.recdate,getdate()) >= {1}");

            Fill(String.Format(sqlBuilder.ToString(),this.args["wareh"],int.Parse(this.args["delaydays"].ToString())), this.ds, "tblresult");

            Fill("select code,cdesc from miscode where ckind='CD' and status='Y' ", this.ds, "tblarea");

            sqlBuilder.Length = 0;
            sqlBuilder.Append("select invbat.itcls,invbat.itnbr,invmas.itdsc,invbat.varnr,invbat.onhand1 from ");
            sqlBuilder.Append(" invbat,invmas where invbat.itnbr=invmas.itnbr and isnull(varnr,'')<>'' and onhand1<>0 and wareh='W01' ");
            sqlBuilder.Append(" and invbat.itcls in {0}");

            Fill(String.Format(sqlBuilder.ToString(),this.args["itcls"]), this.ds, "tblstock");

        }

        public override void ConfigData()
        {
            foreach (DataRow item in ds.Tables["tblresult"].Rows)
            {
                foreach (DataRow row in ds.Tables["tblarea"].Rows)
                {
                    if (item["areacode"].ToString() == row["code"].ToString())
                    {
                        item["areacode"] = row["cdesc"].ToString();
                    }
                }

            }
            DataRow stockitem;
            Redo:
            for (int i = 0; i < ds.Tables["tblstock"].Rows.Count; i++)
            {
                if (ds.Tables["tblstock"].Rows[i].RowState == DataRowState.Deleted) continue;
                stockitem = ds.Tables["tblstock"].Rows[i];
                foreach (DataRow row in ds.Tables["tblresult"].Rows)
                {
                    if (stockitem["varnr"].ToString() == row["varnr"].ToString())
                    {
                        ds.Tables["tblstock"].Rows.RemoveAt(i);
                        goto Redo;
                    }
                }
            }
            this.ds.AcceptChanges();
        }
    }
}
