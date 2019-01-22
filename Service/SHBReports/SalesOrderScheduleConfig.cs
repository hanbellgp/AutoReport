﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class SalesOrderScheduleConfig : NotificationConfig
    {
        public SalesOrderScheduleConfig()
        {
        }

        public SalesOrderScheduleConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new SalesOrderScheduleDS();
            this.reportList.Add(new SalesOrderScheduleReport());
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            string sqlstr = "select a.cusno,a.cusna,(case b.mancode when 'C0960' then 'C1826' else b.mancode end) as mancode ,'' as username,a.cuspono,"+
            "a.cdrno,a.itnbrcus,a.itdsc,a.qty,a.shipday1,a.inqty,a.shipqty,a.manno,a.manday1,a.finday1,a.sn,a.remark1 from cdrschedule a"+
             "left outer join cdrhmas b on left(a.cdrno,len(a.cdrno)-4)=b.cdrno where a.status<>'Y' and  a.kindcode='{0}' ";
            Fill(String.Format(sqlstr, args["kindcode"]), ds, "tblresult");

            Fill("select userno,username from secuser ", ds, "tbluser");
        }

        public override void ConfigData()
        {
            foreach (DataRow row in ds.Tables["tblresult"].Rows)
            {
                foreach (DataRow user in ds.Tables["tbluser"].Rows)
                {
                    if (row["mancode"].Equals(user["userno"]))
                    {
                        row["username"] = user["username"];
                    }
                }
            }
            ds.AcceptChanges();
        }
    }
}
