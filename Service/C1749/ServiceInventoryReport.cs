using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class ServiceInventoryReport:NotificationContent
    {
        public ServiceInventoryReport() { }

        protected override void Init()
        {
            base.Init();
            nc = new ServiceInventoryReportConfig(DBServerType.SybaseASE, "SHBERP", this.ToString());
            nc.InitData();
            nc.ConfigData();
            DataTable zjtbl = nc.GetDataTable("zjtbl");
            DataTable djtbl = nc.GetDataTable("djtbl");
            DataTable zctbl = nc.GetDataTable("zctbl");
            DataTable yptbl = nc.GetDataTable("yptbl");
            DataTable newZjtbl = setDableSumItem(zjtbl);
            DataTable newDjtbl = setDableSumItem(djtbl);
            DataTable newZctbl = setDableSumItem(zctbl);
            DataTable newYptbl = setDableSumItem(yptbl);

            string fileFullName1 = Base.GetServiceInstallPath() + "\\Data\\" + "铸件统计报表" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
            DataTableToExcel(newZjtbl, fileFullName1, true);
            string fileFullName2 = Base.GetServiceInstallPath() + "\\Data\\" + "电机统计报表" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
            DataTableToExcel(newDjtbl, fileFullName2, true);
            string fileFullName3 = Base.GetServiceInstallPath() + "\\Data\\" + "轴承统计报表" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
            DataTableToExcel(newZctbl, fileFullName3, true);
            string fileFullName4 = Base.GetServiceInstallPath() + "\\Data\\" + "油品统计报表" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
            DataTableToExcel(newYptbl, fileFullName4, true);
            this.content = GetContentHead() + GetContentFooter();
            AddNotify(new MailNotify());
            

        }

        //增加合计项
        public DataTable setDableSumItem(DataTable dt) 
        {
            DataRow newRow = dt.NewRow();
            newRow[0] = "库存金额";
            var data = dt.AsEnumerable();
            var sum = data.Sum(o => string.IsNullOrEmpty(o["cst"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(o["cst"]), 4));
            newRow["wareh"] = sum;
            dt.Rows.Add(newRow);
            dt.Columns.Remove("cst");
            dt.AcceptChanges();
            return dt;
        }
    }
}
