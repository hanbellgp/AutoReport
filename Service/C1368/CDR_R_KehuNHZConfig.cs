using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data.Common;
using System.Data;

namespace Hanbell.AutoReport.Config
{

    public class CDR_R_KehuNHZConfig : NotificationConfig
    {

        public CDR_R_KehuNHZConfig()
        {
        }

        public CDR_R_KehuNHZConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSCDR_R_KehuNHZ();
            this.args = Base.GetParameter(notification,this.ToString());
        }


        public override void InitData()
        {
            //Updatepros();
            String sqlstr = "select protype,itnbrcus,cusna,sum(armqy) as qty,sum(shpamts) as amts  from cdr_rs_rqymx " + 
            " where left(depno,2)in ('{0}')  " +
                " and convert(varchar(4),shpmonq,112) =convert(varchar(4),dateadd(month,-13,getdate()),112)" +
                " and convert(varchar(6),shpmonq,112) <=convert(varchar(6),dateadd(month,-13,getdate()),112)" +
            //" and convert(varchar(4),shpmonq,112) =convert(varchar(4),dateadd(month,-1,getdate()),112)" + 
            //" and convert(varchar(6),shpmonq,112) <=convert(varchar(6),dateadd(month,-1,getdate()),112)" + 
            " group by protype,itnbrcus,cusna  " 
            ;


            Fill(String.Format(sqlstr, args["depno"]), ds, "CDR_R_KehuNHZ");


        }

        public override void ConfigData()
        {

            DataRow newRow;
            DataColumn colQty = new DataColumn();
            DataColumn colAmts = new DataColumn();
            String itnbrcus = "", cusna = "";
            foreach (DataRow item in ds.Tables["CDR_R_KehuNHZ"].Rows)
            {
                cusna = item["cusna"].ToString();
                if (!ds.Tables["tblresult"].Columns.Contains(cusna + "数量"))
                {
                    colQty = new DataColumn(cusna + "数量", System.Type.GetType("System.Decimal"));
                    colQty.DefaultValue = 0D;
                    colAmts = new DataColumn(cusna + "金额", System.Type.GetType("System.Decimal"));
                    colAmts.DefaultValue = 0D;
                    ds.Tables["tblresult"].Columns.Add(colQty);
                    ds.Tables["tblresult"].Columns.Add(colAmts);
                }
                else
                {
                    colQty = ds.Tables["tblresult"].Columns[cusna + "数量"];
                    colAmts = ds.Tables["tblresult"].Columns[cusna + "金额"];
                }
                if (itnbrcus != item["itnbrcus"].ToString())
                {
                    newRow = ds.Tables["tblresult"].NewRow();
                    newRow["protype"] = item["protype"];
                    newRow["itnbrcus"] = item["itnbrcus"];
                    newRow[colQty.ColumnName] = Decimal.Parse(item["qty"].ToString());
                    newRow[colAmts.ColumnName] = Decimal.Parse(item["amts"].ToString());
                    ds.Tables["tblresult"].Rows.Add(newRow);
                    itnbrcus = item["itnbrcus"].ToString();
                }
                else
                {
                    newRow = ds.Tables["tblresult"].Select("itnbrcus='" + itnbrcus + "'")[0];
                    newRow[colQty.ColumnName] = Decimal.Parse(item["qty"].ToString());
                    newRow[colAmts.ColumnName] = Decimal.Parse(item["amts"].ToString());
                }

            }
        }

        protected int Updatepros()
        {

            DbCommand sqlcomm = CreateDbCommand(this.dbtype);
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = this.dbconn;
            sqlcomm.CommandText = "rs_cdrrchuhuo_7";
            if (dbconn.State == ConnectionState.Closed)
            {
                dbconn.Open();
            }
            try
            {
                sqlcomm.ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                if (dbconn.State == ConnectionState.Open)
                {
                    dbconn.Close();
                }
            }
        }


    }
}

