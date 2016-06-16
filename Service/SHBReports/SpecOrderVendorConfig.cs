using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class SpecOrderVendorConfig : SpecOrderNotificationConfig
    {

        public SpecOrderVendorConfig(DBServerType dbType, string connName,string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSSpecOrderVendor();
            this.args = Base.GetParameter(notification,this.ToString());
        }

        public override void InitData()
        {

            ResetSpecialOrderDetailState();

            string sqlstr = "SELECT cdrspec.id,purdtamap.pono,purdask.vdrno,'' as vdrna," +
                "cdrspecdetail.itdsc,cdrspecdetail.itnbr,cdrspecdetail.qty,cdrspecdetail.purman as man," +
                "cdrspecdetail.purday1 as day1 FROM cdrspec,cdrspecdetail,purdask  LEFT OUTER JOIN" +
                " purdtamap ON purdask.facno=purdtamap.facno AND purdask.prno=purdtamap.srcno AND purdask.trseq=purdtamap.srcseq " +
                " WHERE ( cdrspec.facno = cdrspecdetail.facno ) AND ( cdrspec.id = cdrspecdetail.id ) AND (cdrspec.status='Y') " +
                " AND (cdrspecdetail.facno = purdask.facno and cdrspecdetail.prno = purdask.prno and cdrspecdetail.itnbr=purdask.itnbr) " +
                " AND (cdrspecdetail.state=0) AND datediff(day,getdate(),isnull(purday2,purday1))<={0} " +
                " AND (purdask.vdrno in {1}) ORDER BY cdrspec.facno ASC, cdrspec.id ASC ";

            Fill(string.Format(sqlstr, args["purday1"], args["vdrno"]), ds, "tblcdrspec");

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
            foreach (DataRow row in ds.Tables["tblcdrspec"].Rows)
            {
                row["vdrna"] = GetVendorName(row["vdrno"].ToString());
                row["email"] = GetVendorEMail12(row["vdrno"].ToString());
            }
            ds.AcceptChanges();
        }

        private string GetVendorName(string vdrno)
        {
            DataTable tbl = GetQueryTable("select vdrna from purvdr where vdrno='" + vdrno + "';");
            if ((tbl != null) && (tbl.Rows.Count > 0))
            {
                return tbl.Rows[0]["vdrna"].ToString();
            }
            else
            {
                return "";
            }
        }

        private string GetVendorEMail12(string vdrno)
        {
            DataTable tbl = GetQueryTable("select [email] from scmvdrmail where [syscode]='12' and vdrno='" + vdrno + "';");
            if ((tbl != null) && (tbl.Rows.Count > 0))
            {
                return tbl.Rows[0]["email"].ToString();
            }
            else
            {
                return "";
            }
        }

    }
}
