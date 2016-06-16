using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class SpecOrderAnalyzeConfig : SpecOrderNotificationConfig
    {

        public SpecOrderAnalyzeConfig()
        {
        }

        public SpecOrderAnalyzeConfig(DBServerType dbType, string connName,string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSSpecOrderAnalyze();
            this.reportList.Add(new SpecOrderAnalyzeReport());
            this.args = Base.GetParameter(notification,this.ToString());
        }

        public override void InitData()
        {
            ResetSpecialOrderState();

            string sqlstr ;
            sqlstr = "SELECT cdrspec.id,cdrspec.cusno,cdrcus.cusna,cdrspec.orderid,cdrspec.itdsc,cdrspec.qty,cdrspec.shipday1,cdrspec.shipday2," +
                "datediff(day,cdrspec.shipday1,cdrspec.shipday2) as shipdelay,cdrspec.finday1,cdrspec.finday2,datediff(day,cdrspec.finday1,cdrspec.finday2) as findelay, " +
                "(select max(datediff(day,a.rdday1,a.rdday2)) from cdrspecdetail a where a.state=1 and a.facno=cdrspec.facno and a.id=cdrspec.id ) as rddelay," +
                "(select max(datediff(day,a.prday1,a.prday2)) from cdrspecdetail a where a.state=1 and a.facno=cdrspec.facno and a.id=cdrspec.id ) as prdelay," +
                "(select max(datediff(day,a.purday1,isnull(a.accday2,a.purday2))) from cdrspecdetail a where a.state=1 and a.facno=cdrspec.facno and a.id=cdrspec.id ) as purdelay " +
                " FROM cdrspec left outer join cdrcus on cdrspec.cusno = cdrcus.cusno " +
                " WHERE  (convert(char(6),isnull(cdrspec.shipday2,'2099/01/01'),112)=convert(char(6),getdate(),112)) AND cdrspec.kindcode='{0}' ORDER BY cdrspec.id ASC ";
            Fill(String.Format(sqlstr,args["kindcode"]), ds, "tblcdrspec");

            sqlstr = "SELECT cdrspec.id,cdrspec.projname,cdrspec.cusno,cdrspec.itdsc as prodname,cdrspec.shipday1,cdrspec.shipday2,cdrspec.shipid,datediff(day,shipday1,shipday2) as shipdelay," +
              "cdrspec.finday1,cdrspec.finday2,cdrspec.manid,datediff(day,finday1,finday2) as findelay,cdrspecdetail.seq,cdrspecdetail.itdsc,cdrspecdetail.itnbr,cdrspecdetail.qty," +
              "cdrspecdetail.rdman,cdrspecdetail.rdday1,cdrspecdetail.rdday2,datediff(day,cdrspecdetail.rdday1,cdrspecdetail.rdday2) as rddelay," +
              "cdrspecdetail.prman,cdrspecdetail.prday1,cdrspecdetail.prday2,datediff(day,cdrspecdetail.prday1,cdrspecdetail.prday2) as prdelay," +
              "cdrspecdetail.purman,cdrspecdetail.purday1,cdrspecdetail.purday2,datediff(day,cdrspecdetail.purday1,isnull(cdrspecdetail.recday2,cdrspecdetail.purday2)) as purdelay," +
              "cdrspecdetail.acceptno,cdrspecdetail.accday2,cdrspecdetail.remark FROM cdrspec,cdrspecdetail " +
              "WHERE ( cdrspec.facno = cdrspecdetail.facno) and  (cdrspec.id = cdrspecdetail.id ) and  (convert(char(6),isnull(cdrspec.shipday2,'2099/01/01'),112)=convert(char(6),getdate(),112)) " +
              " AND cdrspec.kindcode='{0}' ORDER BY cdrspec.id ASC    ";
            Fill(String.Format(sqlstr, args["kindcode"]), ds, "tblcdrspecdetail");

            Fill("select userno,username from secuser ", ds, "tbluser");
        }

        public override void ConfigData()
        {

            foreach (DataRow item in ds.Tables["tblcdrspec"].Rows)
            {
                if (item["prdelay"].ToString() != "" && item["rddelay"].ToString() != "")
                {
                    item["prdelay"] = int.Parse(item["prdelay"].ToString()) - int.Parse(item["rddelay"].ToString());
                }
                if (item["purdelay"].ToString() != "" && item["prdelay"].ToString() != "")
                {
                    item["purdelay"] = int.Parse(item["purdelay"].ToString()) - int.Parse(item["prdelay"].ToString());
                }                
            }

            foreach (DataRow row in ds.Tables["tblcdrspecdetail"].Rows)
            {
                foreach (DataRow user in ds.Tables["tbluser"].Rows)
                {
                    if (row["rdman"].Equals(user["userno"]))
                    {
                        row["rdman"] = user["username"];
                    }
                    if (row["prman"].Equals(user["userno"]))
                    {
                        row["prman"] = user["username"];
                    }
                    if (row["purman"].Equals(user["userno"]))
                    {
                        row["purman"] = user["username"];
                    }
                }
             }
            ds.AcceptChanges();

        }

    }
}
