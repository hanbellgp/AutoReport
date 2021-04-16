using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class AHPR600:NotificationContent
    {
        public AHPR600() { }

        protected override void Init()
        {
            base.Init();
            nc = new AHPR600Config(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            if (nc.GetReportList().Count > 0)
            {
                SetAttachment();
            }
            DataTable dt = nc.GetDataTable("tbl");
            //var obj = dt.AsEnumerable();
            ////增加最后一行合计
            //DataRow row = dt.NewRow();
            //if (dt != null)
            //{
            //    row["cmcmodel"] = "";
            //    row["itnbr"] = "";
            //    row["fixnr"] = "合计";
            //}
            //foreach( DataColumn column in dt.Columns ){
            //    int columnRow = dt.Columns.Count;
            //    if (dt.Columns.IndexOf(column)>2)
            //    {
            //        var va = obj.Sum(o => Convert.ToDecimal(o[column.ColumnName] == DBNull.Value ? 0 : o[column.ColumnName]));
            //        row[column.ColumnName] = va;
            //    }
            //}
            //dt.Rows.Add(row);
            //dt.AcceptChanges();
            this.content = GetContentHead() + GetContentFooter();
            string varnrArr = getVarnr();
            if (dt.Rows.Count > 0 && dt.Rows.Count > 0)
            {
                string fileFullName = Base.GetServiceInstallPath() + "\\Data\\" + "铸件" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                DataTableToExcel(dt, fileFullName, true);
                AddNotify(new MailNotify());
            }
        }

        //合并ERP和MES的数据为一个Datatable 
        protected DataTable getData() 
        {
            //并且合并单元格
            var MESTable = getMESCheckData();
            var ERPTable = nc.GetDataTable("tbl");
            var newDataTable = MESTable.Clone();
            newDataTable.Clear();
            object[] obj = new object[newDataTable.Columns.Count];
            for (int i = 0; i < MESTable.Rows.Count; i++)
            {
                MESTable.Rows[i].ItemArray.CopyTo(obj,0);
                newDataTable.Rows.Add(obj);
            }

            for (int i = 0; i < ERPTable.Rows.Count; i++)
            {
                ERPTable.Rows[i].ItemArray.CopyTo(obj,0);
                newDataTable.Rows.Add(obj);
            }
            //排序
            DataView dv = newDataTable.DefaultView;
            dv.Sort = " itnbr DESC ";
            newDataTable = dv.ToTable();
            //合并单元格
            
            newDataTable.AcceptChanges();
            return newDataTable;
        }

        //MES的检验站表
        protected DataTable getMESCheckData()
        {
            if (nc == null) return null;
            StringBuilder sqlstr = new StringBuilder();
            sqlstr.Append(" select PRODUCTID ,'' as cmcmodel, STEPID , count(STEPID) as 'count' from PROCESS where PRODUCTCOMPID in  ");
            sqlstr.Append(getVarnr());
            sqlstr.Append(" GROUP BY PRODUCTID, STEPID ");
            sqlstr.Append(" order by PRODUCTID ");
            return nc.GetQueryTable(DBServerType.SybaseASE, Base.GetDBConnectionString("SHBMES"), sqlstr.ToString());
        }

        //获取ERP的制造号码
        public string getVarnr() 
        {
            var table = nc.GetDataTable("varnrtbl");
            var tableEn = table.AsEnumerable();
            var sltStr = tableEn.Select(o => o["varnr"].ToString()).ToArray<string>();
            var whereStr = "('" + string.Join("','", sltStr) + "')";
            return whereStr;
        }

    }
}
