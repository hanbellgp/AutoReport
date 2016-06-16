using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class SpecOrderPPConfig : SpecOrderNotificationConfig
    {

        public SpecOrderPPConfig(DBServerType dbType, string connName,string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSSpecOrderWBS();
            this.args = Base.GetParameter(notification,this.ToString());
        }

        public override void InitData()
        {
            //被SpecOrderUpdateState取代
            //ResetSpecialOrderDetailState();

            string sqlstr = "SELECT cdrspec.id,cdrspec.projname as project,cdrspec.itdsc as prodname,cdrspec.shipday1," +
                 "cdrspecdetail.seq,cdrspecdetail.itdsc,cdrspecdetail.itnbr,cdrspecdetail.qty,cdrspecdetail.prman as man," +
                 "cdrspecdetail.prday1 as day1,cdrspecdetail.remark  FROM cdrspec,cdrspecdetail   " +
                 "WHERE ( cdrspec.facno = cdrspecdetail.facno ) and  ( cdrspec.id = cdrspecdetail.id ) and (cdrspec.status='Y') and (cdrspecdetail.state=0) " +
                 "AND isnull(cdrspecdetail.itnbr,'')<>'' AND prday2 is null AND datediff(day,getdate(),prday1)<={0} ORDER BY cdrspec.facno ASC, cdrspec.id ASC    ";
            Fill(string.Format(sqlstr, args["prday1"]), ds, "tblcdrspec");

            Fill("select userno,username from secuser ", ds, "tbluser");

        }

        public override void ConfigData()
        {
            if (ds.Tables["tblcdrspec"].Rows.Count == 0) return;

            foreach (DataRow row in ds.Tables["tblcdrspec"].Rows)
            {
                foreach (DataRow user in ds.Tables["tbluser"].Rows)
                {
                    if (row["man"].Equals(user["userno"]))
                    {
                        row["name"] = user["username"];
                    }
                }
            }
        }

    }
}
