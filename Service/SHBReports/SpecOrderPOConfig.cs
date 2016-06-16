using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class SpecOrderPOConfig : SpecOrderNotificationConfig
    {

        public SpecOrderPOConfig(DBServerType dbType, string connName,string notification)
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
                "cdrspecdetail.seq,cdrspecdetail.itdsc,cdrspecdetail.itnbr,cdrspecdetail.qty,cdrspecdetail.purman as man," +
                "isnull(cdrspecdetail.purday2,cdrspecdetail.purday1) as day1,cdrspecdetail.remark FROM cdrspec,cdrspecdetail   " +
                "WHERE ( cdrspec.facno = cdrspecdetail.facno ) and  ( cdrspec.id = cdrspecdetail.id ) and (cdrspec.status='Y') and (cdrspecdetail.state=0) " +
                "AND datediff(day,getdate(),isnull(purday2,purday1))<={0} AND cdrspec.kindcode='{1}'　ORDER BY cdrspec.facno ASC, cdrspec.id ASC    ";
            Fill(string.Format(sqlstr, args["purday1"], args["kindcode"]), ds, "tblcdrspec");

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
