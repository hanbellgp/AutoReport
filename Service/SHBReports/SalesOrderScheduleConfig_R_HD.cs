using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;

namespace Hanbell.AutoReport.Config
{
    public class SalesOrderScheduleConfig_R_HD : SalesOrderScheduleConfig
    {

        public SalesOrderScheduleConfig_R_HD(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new SalesOrderScheduleDS();
            this.reportList.Add(new SalesOrderScheduleReport());
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            string sqlstr = "select a.cusno,a.cusna,b.mancode,'' as username,a.cuspono,a.cdrno,a.itnbrcus,a.itdsc,a.qty,b.recdate,a.shipday1,a.inqty,a.shipqty,a.manno,a.manday1,a.finday1,a.sn,a.remark1 from cdrschedule a " +
            " left outer join cdrhmas b on left(a.cdrno,len(a.cdrno)-4)=b.cdrno where a.status<>'Y' and  a.kindcode='{0}' and a.areacode='{1}' ";
            Fill(String.Format(sqlstr, args["kindcode"], args["areacode"]), ds, "tblresult");

            Fill("select userno,username from secuser ", ds, "tbluser");
        }

        public override void ConfigData()
        {
            base.ConfigData();

            decimal cdrqty, invqty, shpqty;
            cdrqty = 0M;
            invqty = 0M;
            shpqty = 0M;
            foreach (DataRow item in ds.Tables["tblresult"].Rows)
            {
                cdrqty += decimal.Parse(item["qty"].ToString());
                invqty += decimal.Parse(item["inqty"].ToString());
                shpqty += decimal.Parse(item["shipqty"].ToString());
            }
            DataRow newRow = ds.Tables["tblresult"].NewRow();
            newRow["cusna"] = "合计";
            newRow["qty"] = cdrqty;
            newRow["inqty"] = invqty;
            newRow["shipqty"] = shpqty;
            ds.Tables["tblresult"].Rows.Add(newRow);
        }

    }
}
