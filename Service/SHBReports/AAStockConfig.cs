using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    public class AAStockConfig : NotificationConfig
    {
        public AAStockConfig()
        {
        }

        public AAStockConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new AAStockDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            string invsql = "select itnbr,sum(onhand1) as invqty from invbal where facno='{0}' and itnbr in (select distinct itnbr from cdrdmmodel where kind in {1}) and wareh in {2} group by itnbr";
            Fill(String.Format(invsql, args["facno"], args["kind"], args["wareh"]), ds, "tblinv");

            string sfcsql = "select itnbr,sum(onhand1) as sfcqty from manwipbal where facno='{0}' and itnbr in (select distinct itnbr from cdrdmmodel where kind in {1}) and prosscode in {2} group by itnbr";
            Fill(String.Format(sfcsql, args["facno"], args["kind"], args["prosscode"]), ds, "tblsfc");

            string cdrsql = "select itnbr,sum(cdrqy1 - shpqy1) as cdrqty from cdrdmas where facno='{0}' and itnbr in (select distinct itnbr from cdrdmmodel " +
                " where kind in {1}) and drecsta<>'95' and drecsta<>'98' and drecsta<>'99' and datediff(day,cdrdate,getdate())<={2} and datediff(day,getdate(),cdrdate)<={3} group by itnbr";
            Fill(String.Format(cdrsql, args["facno"], args["kind"], args["days"], args["days"]), ds, "tblcdr");

            string sqlstr = "select distinct itnbr,cmcmodel from cdrdmmodel where kind in {0} order by kind,itnbr";
            Fill(String.Format(sqlstr, args["kind"]), ds, "tblresult");
        }

        public override void ConfigData()
        {

            foreach (DataRow item in ds.Tables["tblresult"].Rows)
            {
                foreach (DataRow inv in ds.Tables["tblinv"].Rows)
                {
                    if (item["itnbr"].Equals(inv["itnbr"]))
                    {
                        item["invqty"] = inv["invqty"];
                        break;
                    }
                }

                foreach (DataRow sfc in ds.Tables["tblsfc"].Rows)
                {
                    if (item["itnbr"].Equals(sfc["itnbr"]))
                    {
                        item["sfcqty"] = sfc["sfcqty"];
                        break;
                    }
                }

                foreach (DataRow cdr in ds.Tables["tblcdr"].Rows)
                {
                    if (item["itnbr"].Equals(cdr["itnbr"]))
                    {
                        item["cdrqty"] = cdr["cdrqty"];
                        break;
                    }
                }

            }

            foreach (DataRow item in ds.Tables["tblresult"].Rows)
            {
                item["qty"] = double.Parse(item["invqty"].ToString()) + double.Parse(item["sfcqty"].ToString()) - double.Parse(item["cdrqty"].ToString());
            }

            ds.Tables["tblresult"].Columns.Remove("itnbr");

        }

    }
}
